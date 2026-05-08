using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatMessageUnit : MonoBehaviour
{
    [SerializeField] Transform ownerIcon;
    [SerializeField] Transform messageHolder;
    [SerializeField] LayoutGroup messageHolderLayout;
    [SerializeField] TMP_Text messageText;
    [SerializeField] TMP_Text ownerNameText;
    [SerializeField] Image ownerIconImage;

    public string MessageText
    {
        get => messageText.text;
        set => messageText.text = value;
    }

    public string OwnerName
    {
        get => ownerNameText.text;
        set => ownerNameText.text = value;
    }

    public Sprite OwnerIcon
    {
        get => ownerIconImage.sprite;
        set => ownerIconImage.sprite = value;
    }

    public void SetSendFromOther()
    {
        messageHolder.SetAsLastSibling();
        ownerIcon.SetAsFirstSibling();
        messageHolderLayout.childAlignment = TextAnchor.UpperLeft;
    }

    public void SetSendFromSelf()
    {
        messageHolder.SetAsFirstSibling();
        ownerIcon.SetAsLastSibling();
        messageHolderLayout.childAlignment = TextAnchor.UpperRight;
    }

    public void Setup(string message, string ownerName, Sprite ownerIcon, bool isSendFromOther = true)
    {
        MessageText = message;
        OwnerName = ownerName;
        OwnerIcon = ownerIcon;
        if (isSendFromOther)
        {
            SetSendFromOther();
        }
        else
        {
            SetSendFromSelf();
        }
    }
}
