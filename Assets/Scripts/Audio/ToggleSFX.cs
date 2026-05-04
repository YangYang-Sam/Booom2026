using JTUtility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleSFX : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private List<AudioClip> onSFX;
    [SerializeField] private List<AudioClip> offSFX;
    [SerializeField] private List<AudioClip> hoverSFX;

    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(bool isOn)
    {
        AudioClip sfx;
        if (isOn)
        {
            sfx = onSFX.PickRandom();
        }
        else
        {
            sfx = offSFX.PickRandom();
        }

        if (sfx.IsNotNull())
        {
            AudioManager.instance.PlaySFX(sfx);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toggle.interactable == false || hoverSFX.IsNullOrEmpty())
            return;

        AudioManager.instance.PlaySFX(hoverSFX.PickRandom());
    }
}