using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JTUtility;


public class FancyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image slide;
    [SerializeField] Button button;
    [SerializeField] float completeDuration = 1f;
    [SerializeField] GameObject activeOnShowing;

    List<TMPro.TMP_Text> texts;
    List<Color> colors;
    List<Color> revertedColors;

    bool isFilling = false;
    bool isActive = true;
    float time = 0;

    void Awake()
    {
        texts = new List<TMPro.TMP_Text>();
        colors = new List<Color>();
        revertedColors = new List<Color>();
        foreach (var text in GetComponentsInChildren<TMPro.TMP_Text>(true))
        {
            texts.Add(text);
            colors.Add(text.color);
            revertedColors.Add(new Color(1 - text.color.r, 1 - text.color.g, 1 - text.color.b, text.color.a));
        }
    }

    void OnEnable()
    {
        time = 0;
        slide.fillAmount = time;
        isFilling = false;
    }

    void OnDisable()
    {
        time = 0;
        slide.fillAmount = time;
        isFilling = false;
    }

    void Update()
    {
        var speed = 1f / completeDuration;
        isActive = button.interactable && button.isActiveAndEnabled;

        if (isFilling && isActive)
        {
            time += speed * Time.unscaledDeltaTime;
        }
        else
        {
            time -= speed * Time.unscaledDeltaTime;
        }

        time = Mathf.Clamp01(time);

        slide.fillAmount = time;
        if (activeOnShowing.IsNotNull())
        {
            activeOnShowing.SetActive(slide.fillAmount >= 0.99f);
        }

        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].color = Color.Lerp(colors[i], revertedColors[i], time);
        }
    }

    public void Click()
    {
        if (!button.interactable || !button.isActiveAndEnabled)
            return;

        button.onClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isFilling = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isFilling = false;
    }
}
