using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;

public class ViewRaycaster : MonoSingleton<ViewRaycaster>
{
    [SerializeField] private Camera viewCamera;
    [SerializeField] private LayerMask raycastLayerMask = ~0;

    RaycastHit[] raycastHits = new RaycastHit[10];

    public int HitCount { get; private set; }
    public IReadOnlyList<RaycastHit> RaycastHits => raycastHits;

    public Camera ViewCamera
    {
        get
        {
            if (viewCamera.IsNotNull())
            {
                return viewCamera;
            }

            if (viewCamera.IsNull())
            {
                viewCamera = Camera.main;
            }
            if (viewCamera.IsNull())
            {
                viewCamera = GetComponent<Camera>();
            }
            if (viewCamera.IsNull())
            {
                viewCamera = FindObjectOfType<Camera>();
            }
            if (viewCamera.IsNull())
            {
                Debug.LogError("View camera not found");
                return null;
            }

            return viewCamera;
        }
    }

    private void Update()
    {
        Ray ray = ViewCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        HitCount = Physics.RaycastNonAlloc(ray, raycastHits, 100f, raycastLayerMask);
    }
}
