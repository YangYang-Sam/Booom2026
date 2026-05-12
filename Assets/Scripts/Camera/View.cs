using UnityEngine;

public class View : MonoBehaviour
{
    [Header("互补滤波")]
    [Range(0f, 1f)]
    [SerializeField] private float complementaryGain = 0.02f;
    [SerializeField] private bool useLowFrequencyCorrection = false;
    [SerializeField] private bool lockRoll = true;

    [Header("一欧元滤波")]
    [SerializeField] private float minCutoff = 1.2f;
    [SerializeField] private float beta = 0.08f;
    [SerializeField] private float derivativeCutoff = 1.0f;

    [Header("调试")]
    [SerializeField] private bool useEditorMouseFallback = true;
    [SerializeField] private float mouseSensitivity = 120f;
    [SerializeField] private bool compensateScreenOrientation = true;
    [SerializeField] private bool calibrateOnStart = false;
    [SerializeField] private bool directGyroMode = true;

    private OneEuroAngleFilter yawFilter;
    private OneEuroAngleFilter pitchFilter;
    private OneEuroAngleFilter rollFilter;

    private Quaternion fusedRotation = Quaternion.identity;
    private bool hasFusedRotation;
    private Quaternion referenceRotation = Quaternion.identity;
    private bool hasReferenceRotation;
    private float fallbackYaw;
    private float fallbackPitch;
    private float yawRecenterOffset;
    private bool gyroUnavailableLogged;
    private Quaternion pendingRotation = Quaternion.identity;
    private bool hasPendingRotation;

    private void Awake()
    {
        RecreateFilters();

        Input.gyro.enabled = true;
        Input.compass.enabled = true;
    }

    private void Update()
    {
        float dt = Mathf.Max(Time.deltaTime, 1e-5f);

        if (directGyroMode && SystemInfo.supportsGyroscope)
        {
            if (TryApplyDirectGyroRotation())
            {
                return;
            }
        }

        if (!TryGetSensorRotation(dt, out Quaternion targetRotation))
        {
            if (useEditorMouseFallback)
            {
                UpdateEditorFallback(dt);
            }
            return;
        }

        // 先从 forward 向量提取 yaw/pitch，避免 roll 轴映射异常时被 lockRoll 误杀。
        Vector3 forward = targetRotation * Vector3.forward;
        if (forward.sqrMagnitude < 0.0001f)
        {
            return;
        }
        forward.Normalize();

        float targetYaw = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        float targetPitch = -Mathf.Asin(Mathf.Clamp(forward.y, -1f, 1f)) * Mathf.Rad2Deg;
        float targetRoll = lockRoll ? 0f : targetRotation.eulerAngles.z;

        float filteredYaw = yawFilter.Filter(targetYaw, Time.unscaledTime);
        float filteredPitch = pitchFilter.Filter(targetPitch, Time.unscaledTime);
        float filteredRoll = lockRoll ? 0f : rollFilter.Filter(targetRoll, Time.unscaledTime);

        Quaternion finalRotation = Quaternion.Euler(filteredPitch, filteredYaw, filteredRoll);
        finalRotation = ApplyYawRecenterOffset(finalRotation);
        QueueRotation(finalRotation);
    }

    private void LateUpdate()
    {
        if (hasPendingRotation)
        {
            transform.rotation = pendingRotation;
        }
    }

    private bool TryApplyDirectGyroRotation()
    {
        Quaternion q = Input.gyro.attitude;
        if (Mathf.Abs(q.x) < 0.0001f &&
            Mathf.Abs(q.y) < 0.0001f &&
            Mathf.Abs(q.z) < 0.0001f &&
            Mathf.Abs(q.w) < 0.0001f)
        {
            if (!gyroUnavailableLogged)
            {
                Debug.LogWarning("Gyro attitude is zero; sensor may be unavailable or not initialized.");
                gyroUnavailableLogged = true;
            }
            return false;
        }

        Quaternion gyroRotation = GyroToUnity(q, compensateScreenOrientation);
        Quaternion aligned = ApplyReference(gyroRotation);
        aligned = ApplyYawRecenterOffset(aligned);

        // 直接姿态驱动：先保证真机能稳定跟随，再按需叠加滤波。
        QueueRotation(aligned);
        return true;
    }

    private bool TryGetSensorRotation(float dt, out Quaternion outputRotation)
    {
        #if UNITY_EDITOR
        if (useEditorMouseFallback)
        {
            outputRotation = Quaternion.identity;
            return false;
        }
        #endif

        Quaternion rawGyroRotation = GyroToUnity(Input.gyro.attitude, compensateScreenOrientation);
        Quaternion gyroRotation = ApplyReference(rawGyroRotation);

        Quaternion accelMagRotation = Quaternion.identity;
        bool hasLowFrequencyRotation = false;
        if (useLowFrequencyCorrection)
        {
            hasLowFrequencyRotation = TryComputeAccelMagRotation(out accelMagRotation, compensateScreenOrientation);
        }

        if (hasLowFrequencyRotation)
        {
            accelMagRotation = ApplyReference(accelMagRotation);
        }

        if (!hasFusedRotation)
        {
            fusedRotation = gyroRotation; 
            hasFusedRotation = true;
        }
        else
        {
            fusedRotation = gyroRotation;
            if (hasLowFrequencyRotation)
            {
                float alpha = 1f - Mathf.Exp(-complementaryGain * dt);
                fusedRotation = Quaternion.Slerp(fusedRotation, accelMagRotation, Mathf.Clamp01(alpha));
            }
        }

        outputRotation = fusedRotation;
        return true;
    }

    private Quaternion ApplyReference(Quaternion sensorRotation)
    {
        if (!calibrateOnStart)
        {
            hasReferenceRotation = false;
            return sensorRotation;
        }

        if (!hasReferenceRotation)
        {
            referenceRotation = sensorRotation;
            hasReferenceRotation = true;
        }

        return Quaternion.Inverse(referenceRotation) * sensorRotation;
    }

    private Quaternion ApplyYawRecenterOffset(Quaternion rotation)
    {
        Quaternion yawOffset = Quaternion.Euler(0f, yawRecenterOffset, 0f);
        return yawOffset * rotation;
    }

    private static float ExtractYaw(Quaternion rotation)
    {
        Vector3 forward = rotation * Vector3.forward;
        forward.y = 0f;
        if (forward.sqrMagnitude < 0.0001f)
        {
            return 0f;
        }

        forward.Normalize();
        return Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
    }

    public void RecenterYaw()
    {
        Quaternion baseRotation = GyroToUnity(Input.gyro.attitude, compensateScreenOrientation);
        baseRotation = ApplyReference(baseRotation);
        float currentYaw = ExtractYaw(baseRotation);
        yawRecenterOffset = -currentYaw;
    }

    public void RecenterYawWithOffset(float offset)
    {
        RecenterYaw();
        yawRecenterOffset += offset;
    }

    private void RecreateFilters()
    {
        yawFilter = new OneEuroAngleFilter(minCutoff, beta, derivativeCutoff);
        pitchFilter = new OneEuroAngleFilter(minCutoff, beta, derivativeCutoff);
        rollFilter = new OneEuroAngleFilter(minCutoff, beta, derivativeCutoff);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying && yawFilter != null)
        {
            RecreateFilters();
            hasFusedRotation = false;
        }
    }
#endif

    private static Quaternion GyroToUnity(Quaternion q, bool compensateOrientation)
    {
        // 设备右手坐标系 -> Unity 左手坐标系
        Quaternion converted = new Quaternion(q.x, q.y, -q.z, -q.w);

        // 将设备姿态转换为 Unity 相机常用坐标基准（设备屏幕法线对齐相机 forward）
        Quaternion deviceToCamera = Quaternion.Euler(90f, 0f, 0f);
        Quaternion rotation = deviceToCamera * converted;

        if (compensateOrientation)
        {
            rotation = GetScreenOrientationCompensation() * rotation;
        }

        return rotation;
    }

    private static bool TryComputeAccelMagRotation(out Quaternion rotation, bool compensateOrientation)
    {
        Vector3 gravity = DeviceVectorToUnity(Input.gyro.gravity, compensateOrientation);
        Vector3 magnetic = DeviceVectorToUnity(Input.compass.rawVector, compensateOrientation);

        if (gravity.sqrMagnitude < 0.0001f || magnetic.sqrMagnitude < 0.0001f)
        {
            rotation = Quaternion.identity;
            return false;
        }

        Vector3 up = -gravity.normalized;
        Vector3 magneticOnHorizon = Vector3.ProjectOnPlane(magnetic, up);
        if (magneticOnHorizon.sqrMagnitude < 0.0001f)
        {
            rotation = Quaternion.identity;
            return false;
        }

        Vector3 east = Vector3.Cross(up, magneticOnHorizon).normalized;
        if (east.sqrMagnitude < 0.0001f)
        {
            rotation = Quaternion.identity;
            return false;
        }

        Vector3 north = Vector3.Cross(east, up).normalized;
        if (north.sqrMagnitude < 0.0001f)
        {
            rotation = Quaternion.identity;
            return false;
        }

        rotation = Quaternion.LookRotation(north, up);
        return true;
    }

    private static Vector3 DeviceVectorToUnity(Vector3 deviceVector, bool compensateOrientation)
    {
        Vector3 unity = new Vector3(deviceVector.x, deviceVector.y, -deviceVector.z);
        if (!compensateOrientation)
        {
            return unity;
        }

        return GetScreenOrientationCompensation() * unity;
    }

    private static Quaternion GetScreenOrientationCompensation()
    {
        switch (Screen.orientation)
        {
            case ScreenOrientation.PortraitUpsideDown:
                return Quaternion.Euler(0f, 0f, -180f);
            case ScreenOrientation.LandscapeLeft:
                return Quaternion.Euler(0f, 0f, -90f);
            case ScreenOrientation.LandscapeRight:
                return Quaternion.Euler(0f, 0f, 90f);
            case ScreenOrientation.Portrait:
            default:
                return Quaternion.identity;
        }
    }

    private void UpdateEditorFallback(float dt)
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        fallbackYaw += mx * mouseSensitivity * dt;
        fallbackPitch = Mathf.Clamp(fallbackPitch - my * mouseSensitivity * dt, -85f, 85f);
        Quaternion finalRotation = Quaternion.Euler(fallbackPitch, fallbackYaw, 0f);
        QueueRotation(finalRotation);
    }

    private void QueueRotation(Quaternion rotation)
    {
        pendingRotation = rotation;
        hasPendingRotation = true;
    }

    private sealed class OneEuroAngleFilter
    {
        private readonly OneEuroFilter filter;
        private bool initialized;
        private float unwrapped;

        public OneEuroAngleFilter(float minCutoff, float beta, float derivativeCutoff)
        {
            filter = new OneEuroFilter(minCutoff, beta, derivativeCutoff);
        }

        public float Filter(float wrappedAngle, float time)
        {
            if (!initialized)
            {
                initialized = true;
                unwrapped = wrappedAngle;
            }
            else
            {
                unwrapped += Mathf.DeltaAngle(unwrapped, wrappedAngle);
            }

            float filtered = filter.Filter(unwrapped, time);
            return Mathf.Repeat(filtered, 360f);
        }
    }

    private sealed class OneEuroFilter
    {
        private readonly float minCutoff;
        private readonly float beta;
        private readonly float derivativeCutoff;
        private readonly LowPassFilter xFilter = new LowPassFilter();
        private readonly LowPassFilter dxFilter = new LowPassFilter();
        private float lastTime = -1f;
        private float lastRaw;
        private bool initialized;

        public OneEuroFilter(float minCutoff, float beta, float derivativeCutoff)
        {
            this.minCutoff = Mathf.Max(0.0001f, minCutoff);
            this.beta = Mathf.Max(0f, beta);
            this.derivativeCutoff = Mathf.Max(0.0001f, derivativeCutoff);
        }

        public float Filter(float value, float time)
        {
            if (!initialized)
            {
                initialized = true;
                lastTime = time;
                lastRaw = value;
                xFilter.Reset(value);
                dxFilter.Reset(0f);
                return value;
            }

            float dt = Mathf.Max(time - lastTime, 1e-5f);
            float dx = (value - lastRaw) / dt;
            float edx = dxFilter.Filter(dx, Alpha(dt, derivativeCutoff));
            float cutoff = minCutoff + beta * Mathf.Abs(edx);
            float result = xFilter.Filter(value, Alpha(dt, cutoff));

            lastTime = time;
            lastRaw = value;
            return result;
        }

        private static float Alpha(float dt, float cutoff)
        {
            float tau = 1f / (2f * Mathf.PI * cutoff);
            return 1f / (1f + tau / dt);
        }
    }

    private sealed class LowPassFilter
    {
        private bool initialized;
        private float state;

        public void Reset(float value)
        {
            initialized = true;
            state = value;
        }

        public float Filter(float value, float alpha)
        {
            if (!initialized)
            {
                Reset(value);
                return value;
            }

            state = Mathf.Lerp(state, value, Mathf.Clamp01(alpha));
            return state;
        }
    }
}
