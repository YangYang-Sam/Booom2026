using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class WatchEvent : MonoBehaviour
{
    enum WatchType
    {
        None,
        On,
        Off,
        Change
    }

    [Header("========== Button ==========")]
    [SerializeField] private Button watchButton;

    [Header("========== Toggle ==========")]
    [SerializeField] private Toggle watchToggle;
    [SerializeField] private WatchType watchToggleType;

    [Header("========== GameObject ==========")]
    [SerializeField] private GameObject watchGameObject;
    [SerializeField] private WatchType watchGameObjectType;

    [Header("========== LookAt ==========")]
    [SerializeField] private Collider watchCollider;
    [SerializeField] private WatchType watchLookAtType;

    [Header("========== Invoke ==========")]

    [SerializeField] private UnityEvent<bool> onEvent;

    [SerializeField][TextArea(2, 8)] private string onEventLua;
    [SerializeField] private bool canRepeat = false;
    [SerializeField] private bool disableOnInvoke = true;

    private bool invoked = false;

    private bool isGameObjectActive = false;
    private ViewRaycaster viewRaycaster;
    private bool isRaycastHit = false;

    void OnEnable()
    {
        if (watchButton != null)
        {
            watchButton.onClick.AddListener(OnButtonClick);
        }
        if (watchToggle != null)
        {
            watchToggle.onValueChanged.AddListener(OnToggleChange);
        }
        if (watchGameObject != null)
        {
            isGameObjectActive = watchGameObject.activeSelf;
        }
        if (watchCollider != null)
        {
            if (viewRaycaster.IsNull())
            {
                viewRaycaster = FindObjectOfType<ViewRaycaster>();
            }
        }
    }

    void OnDisable()
    {
        if (watchButton != null)
        {
            watchButton.onClick.RemoveListener(OnButtonClick);
        }
        if (watchToggle != null)
        {
            watchToggle.onValueChanged.RemoveListener(OnToggleChange);
        }
    }

    void Update()
    {
        WatchGameObject();
        WatchCollider();
    }
    
    private void WatchGameObject()
    {
        if (watchGameObject.IsNull())
            return;

        if (watchGameObject.activeSelf == isGameObjectActive)
            return;

        isGameObjectActive = watchGameObject.activeSelf;

        switch (watchGameObjectType)
        {
            case WatchType.On:
                if (isGameObjectActive)
                {
                    InvokeEvent(true);
                }
                break;
            case WatchType.Off:
                if (!isGameObjectActive)
                {
                    InvokeEvent(false);
                }
                break;
            case WatchType.Change:
                InvokeEvent(isGameObjectActive);
                break;
        }
    }

    private void WatchCollider()
    {
        if (watchCollider.IsNull())
            return;

        bool hit = false;
        for (int i = 0; i < viewRaycaster.HitCount; i++)
        {
            var raycastHit = viewRaycaster.RaycastHits[i];
            if (raycastHit.collider == watchCollider)
            {
                hit = true;
                break;
            }
        }

        if (hit == isRaycastHit)
            return;

        isRaycastHit = hit;

        switch (watchLookAtType)
        {
            case WatchType.On:
                if (hit)
                {
                    InvokeEvent(true);
                }
                break;
            case WatchType.Off:
                if (!hit)
                {
                    InvokeEvent(false);
                }
                break;
            case WatchType.Change:
                InvokeEvent(hit);
                break;
        }
    }

    private void OnButtonClick()
    {
        InvokeEvent(true);
    }

    private void OnToggleChange(bool value)
    {
        switch (watchToggleType)
        {
            case WatchType.On:
                if (value)
                {
                    InvokeEvent(true);
                }
                break;
            case WatchType.Off:
                if (!value)
                {
                    InvokeEvent(false);
                }
                break;
            case WatchType.Change:
                InvokeEvent(value);
                break;
        }
    }

    private void InvokeEvent(bool value)
    {
        if (!canRepeat && invoked)
        {
            return;
        }

        onEvent.Invoke(value);

        if (disableOnInvoke)
        {
            gameObject.SetActive(false);
        }

        invoked = true;
    }
}
