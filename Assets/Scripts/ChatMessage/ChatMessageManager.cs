using System;
using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class ChatOwner
{
    public string id;
    public string name;
    public Sprite icon;
    public bool isSelf;
}

[Serializable]
public class ChatMessage
{
    public string id;
    public string ownerId;
    public string message;
}

[Serializable]
public class ChatChoice
{
    public string id;
    public string choiceText;
    public string messageId;
}

[Serializable]
public class ChatChoiceGroup
{
    public string id;
    public List<ChatChoice> choices;
    public UnityEvent<string> onChoiceGroupSelected;
}

public class ChatMessageManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject chatBox;
    [SerializeField] ConnectionDeterminator connectionDeterminator;
    [SerializeField] Transform verticleLayoutSpace;

    [Space]
    [SerializeField] ChatMessageUnit chatMessageUnitPrefab;
    [SerializeField] Transform chatMessageUnitParent;

    [Space]
    [SerializeField] ChatMessageChoiceGroup chatMessageChoiceGroupPrefab;
    [SerializeField] Transform chatMessageChoiceGroupParent;

    [Space]
    [SerializeField] ScrollRect chatMessageScrollRect;

    [Header("Data")]
    [SerializeField] List<ChatOwner> chatOwners;
    [SerializeField] List<ChatMessage> chatMessages;
    [SerializeField] List<ChatChoiceGroup> chatChoiceGroups;
    

    private ChatMessageChoiceGroup currentChatMessageChoiceGroup;

    void OnEnable()
    {
        connectionDeterminator.OnConnected += OnConnected;
        connectionDeterminator.OnDisconnected += OnDisconnected;
    }

    void OnDisable()
    {
        connectionDeterminator.OnConnected -= OnConnected;
        connectionDeterminator.OnDisconnected -= OnDisconnected;
    }

    public void SendChatMessage(string messageId)
    {
        ChatMessage chatMessage = chatMessages.Find(message => message.id == messageId);
        if (chatMessage == null)
        {
            Debug.LogError($"Chat message with id {messageId} not found");
            return;
        }
        ChatMessageUnit chatMessageUnit = Instantiate(chatMessageUnitPrefab, chatMessageUnitParent);
        ChatOwner chatOwner = chatOwners.Find(owner => owner.id == chatMessage.ownerId);
        if (chatOwner == null)
        {
            Debug.LogError($"Chat owner with id {chatMessage.ownerId} not found");
            return;
        }

        if (!chatBox.activeSelf)
        {
            chatBox.SetActive(true);
        }

        chatMessageUnit.Setup(chatMessage.message, chatOwner.name, chatOwner.icon, !chatOwner.isSelf);
        chatMessageScrollRect.verticalNormalizedPosition = 0f;
        verticleLayoutSpace.SetAsLastSibling();
    }

    public void ShowChatChoiceGroup(string choiceGroupId)
    {
        ChatChoiceGroup chatChoiceGroup = chatChoiceGroups.Find(group => group.id == choiceGroupId);
        if (chatChoiceGroup == null)
        {
            Debug.LogError($"Chat choice group with id {choiceGroupId} not found");
            return;
        }

        ChatMessageChoiceGroup chatMessageChoiceGroup = Instantiate(chatMessageChoiceGroupPrefab, chatMessageChoiceGroupParent);
        chatMessageChoiceGroup.Setup(chatChoiceGroup, OnChoiceClicked);
        currentChatMessageChoiceGroup = chatMessageChoiceGroup;
    }

    public void OnChoiceClicked(ChatChoice choice)
    {
        SendChatMessage(choice.messageId);
        if (currentChatMessageChoiceGroup == null)
        {
            Debug.LogError("No current chat message choice group");
            return;
        }

        currentChatMessageChoiceGroup.ChatChoiceGroup.onChoiceGroupSelected?.Invoke(choice.id);
        currentChatMessageChoiceGroup = null;
    }

    void OnConnected()
    {
        if (currentChatMessageChoiceGroup != null)
        {
            currentChatMessageChoiceGroup.gameObject.SetActive(true);
        }
    }

    void OnDisconnected()
    {
        if (currentChatMessageChoiceGroup != null)
        {
            currentChatMessageChoiceGroup.gameObject.SetActive(false);
        }
    }
}
