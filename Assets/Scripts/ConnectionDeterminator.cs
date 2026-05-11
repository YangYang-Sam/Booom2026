using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JTUtility;

public class ConnectionDeterminator : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [Tooltip("角度阈值，当相机与重力方向的夹角大于此阈值时，认为设备可以连接")]
    [SerializeField] float angleThreshold = 30;
    [Tooltip("时间阈值，当当前状态与上次状态不同时，经过此时间后才认为状态稳定")]
    [SerializeField] float timeThreshold = 1f;
    [SerializeField] UnityEvent onConnected = new UnityEvent();
    [SerializeField] UnityEvent onDisconnected = new UnityEvent();
    [SerializeField] float angle;

    public bool IsConnected { get => isConnected; }

    public event Action OnConnected;
    public event Action OnDisconnected;

    bool isConnected = false;
    float lastStableTime = 0;

    void OnEnable()
    {
        StartCoroutine(UpdateOnEnable());
    }

    private void Update()
    {
        var shouldConnect = ShouldConnect();
        
        lastStableTime -= Time.deltaTime;
        if (shouldConnect == isConnected)
        {
            lastStableTime = timeThreshold;
            return;
        }

        if (lastStableTime <= 0)
        {
            isConnected = shouldConnect;
            if (isConnected)
            {
                onConnected.Invoke();
                OnConnected?.Invoke();
            }
            else
            {
                onDisconnected.Invoke();
                OnDisconnected?.Invoke();
            }
        }
    }

    private bool ShouldConnect()
    {
        angle = Vector3.Angle(cameraTransform.forward, -Vector3.up);
        return angle > angleThreshold;
    }

    private IEnumerator UpdateOnEnable()
    {
        yield return null;
        isConnected = ShouldConnect();
        if (isConnected)
        {
            onConnected.Invoke();
            OnConnected?.Invoke();
        }
        else
        {
            onDisconnected.Invoke();
            OnDisconnected?.Invoke();
        }
    }
}
