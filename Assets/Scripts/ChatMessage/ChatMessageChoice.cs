using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JTUtility;

public class ChatMessageChoice : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TMPro.TMP_Text text;

    ChatChoice choice;
    Action<ChatChoice> onClick;

    public void Setup(ChatChoice choice, Action<ChatChoice> onClick)
    {
        this.choice = choice;
        this.onClick = onClick;
        text.text = choice.choiceText;

        if (button.IsNotNull())
        {
            button.onClick.AddListener(() => this.onClick?.Invoke(choice));
        }
    }
}
