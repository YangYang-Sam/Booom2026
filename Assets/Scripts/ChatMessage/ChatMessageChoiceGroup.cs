using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMessageChoiceGroup : MonoBehaviour
{
    [SerializeField] ChatMessageChoice chatMessageChoicePrefab;
    [SerializeField] Transform chatMessageChoiceParent;

    private ChatChoiceGroup chatChoiceGroup;
    public ChatChoiceGroup ChatChoiceGroup => chatChoiceGroup;
    Action<ChatChoice> onChoiceClicked;

    public void Setup(ChatChoiceGroup chatChoiceGroup, Action<ChatChoice> onChoiceClicked)
    {
        this.chatChoiceGroup = chatChoiceGroup;
        this.onChoiceClicked = onChoiceClicked;
        foreach (var choice in chatChoiceGroup.choices)
        {
            ChatMessageChoice chatMessageChoice = Instantiate(chatMessageChoicePrefab, chatMessageChoiceParent);
            chatMessageChoice.Setup(choice, OnChoiceClicked);
        }
    }

    public void OnChoiceClicked(ChatChoice choice)
    {
        onChoiceClicked?.Invoke(choice);
        gameObject.SetActive(false);
    }
}
