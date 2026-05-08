using Cinemachine;
using UnityEngine;

[AddComponentMenu("")] // 在组件的Add Component菜单中隐藏，使其更整洁
[SaveDuringPlay] // 允许在编辑器运行模式下保存参数的修改
public class CinemachineRotationLimiter : CinemachineExtension
{
    [Tooltip("限制水平旋转的最小角度（-90表示向左最多转90度）")]
    public float horizontalMin = -45f; 
    [Tooltip("限制水平旋转的最大角度（90表示向右最多转90度）")]
    public float horizontalMax = 45f;
    [Tooltip("限制垂直旋转的最小角度（例如，-30表示最多向下看30度）")]
    public float verticalMin = -30f;
    [Tooltip("限制垂直旋转的最大角度（例如，60表示最多向上看60度）")]
    public float verticalMax = 60f;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        // 仅在“Aim”(瞄准)阶段进行限制，这是处理相机旋转的正确时机
        if (stage == CinemachineCore.Stage.Aim)
        {
            // 获取当前的欧拉角表示
            Vector3 eulerAngles = state.RawOrientation.eulerAngles;

            // 将0-360的角度映射到-180到180的范围内，以便使用Mathf.Clamp
            float clampedX = NormalizeAngle(eulerAngles.x);
            float clampedY = NormalizeAngle(eulerAngles.y);

            // 对角度进行限制
            clampedY = Mathf.Clamp(clampedY, horizontalMin, horizontalMax);
            clampedX = Mathf.Clamp(clampedX, verticalMin, verticalMax);

            // 应用限制后的旋转
            state.RawOrientation = Quaternion.Euler(clampedX, clampedY, 0f);
        }
    }

    // 将0-360的角度转换为-180到180的范围，以便正确处理0度附近的限制
    private float NormalizeAngle(float angle)
    {
        if (angle > 180)
            return angle - 360;
        return angle;
    }
}