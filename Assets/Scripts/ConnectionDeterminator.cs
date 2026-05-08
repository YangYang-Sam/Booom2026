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

    public bool IsConnected { get => isConnected; }

    public event Action OnConnected;
    public event Action OnDisconnected;

    bool isConnected = false;
    bool shouldConnect = false;
    float lastStableTime = 0;

    private void Update()
    {
        shouldConnect = Vector3.Angle(cameraTransform.forward, -Vector3.up) > angleThreshold;
        
        if (shouldConnect == isConnected)
        {
            lastStableTime = Time.time;
        }

        if (Time.time - lastStableTime > timeThreshold)
        {
            isConnected = shouldConnect;
            if (isConnected)
            {
                onConnected.Invoke();
            }
            else
            {
                onDisconnected.Invoke();
            }
        }
    }
}
