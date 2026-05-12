using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintDot : MonoBehaviour
{
    [SerializeField] Image ring;

    Material material;

    void OnEnable()
    {
        if (material == null)
        {
            material = ring.material;
        }
    }

    void Update()
    {
        material.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
