using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using JTUtility;

public class ConnectionWatch : MonoBehaviour
{
    [SerializeField][Range(0, 10)] float stayConnectedForSec = 0f;
    [SerializeField][Range(0, 10)] float stayDisconnectedForSec = 0f;
    [SerializeField] bool tryTriggerConnectedOnEnable = false;
    [SerializeField] bool tryTriggerDisconnectedOnEnable = false;
    [SerializeField] UnityEvent onConnected = new UnityEvent();
    [SerializeField] UnityEvent onDisconnected = new UnityEvent();

    [HideInInspector][SerializeField] ConnectionDeterminator connectionDeterminator;

    public bool IsConnected { get => connectionDeterminator?.IsConnected ?? false; }

    private float connectionTime;

    void OnValidate()
    {
        FindConnectionDeterminator();
    }

    void Awake()
    {
        FindConnectionDeterminator();
    }

    void OnEnable()
    {
        if (connectionDeterminator.IsNull())
        {
            return;
        }
        
        connectionDeterminator.OnConnected += OnConnected;
        connectionDeterminator.OnDisconnected += OnDisconnected;
        connectionTime = 0;
        if (tryTriggerConnectedOnEnable && connectionDeterminator.IsConnected && stayConnectedForSec == 0)
        {
            onConnected.Invoke();
        }

        if (tryTriggerDisconnectedOnEnable && !connectionDeterminator.IsConnected && stayDisconnectedForSec == 0)
        {
            onDisconnected.Invoke();
        }
    }

    void OnDisable()
    {
        if (connectionDeterminator.IsNotNull())
        {
            connectionDeterminator.OnConnected -= OnConnected;
            connectionDeterminator.OnDisconnected -= OnDisconnected;
        }
    }

    void Update()
    {
        bool waitToTriggerConnected = connectionTime < stayConnectedForSec;
        bool waitToTriggerDisconnected = connectionTime < stayDisconnectedForSec;
        connectionTime += Time.deltaTime;
        if (waitToTriggerConnected && connectionTime >= stayConnectedForSec)
        {
            onConnected.Invoke();
        }
        if (waitToTriggerDisconnected && connectionTime >= stayDisconnectedForSec)
        {
            onDisconnected.Invoke();
        }
    }

    private void OnConnected()
    {
        if (stayConnectedForSec == 0)
        {
            onConnected.Invoke();
        }

        connectionTime = 0;
    }

    private void OnDisconnected()
    {
        if (stayDisconnectedForSec == 0)
        {
            onDisconnected.Invoke();
        }

        connectionTime = 0;
    }

    private void FindConnectionDeterminator()
    {
        if (connectionDeterminator.IsNull())
        {
            connectionDeterminator = FindObjectOfType<ConnectionDeterminator>();
        }
    }
}
