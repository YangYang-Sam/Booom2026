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
    public string message;
    public string id;
    public string ownerId;
    public float time;
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

[Serializable]
public class ChatMessageSequence
{
    public string id;
    public float delay;
    public List<ChatMessage> messages;
    public UnityEvent onSequenceStarted;
    public UnityEvent onSequenceFinished;
}

public class ChatMessageManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject chatBox;
    [SerializeField] Button chatButton;
    [SerializeField] GameObject chatButtonGO;
    [SerializeField] GameObject chatMessageHintGO;
    [SerializeField] Button talkButton;
    [SerializeField] GameObject talkButtonGO;
    [SerializeField] GameObject talkMessageHintGO;

    [SerializeField] ConnectionDeterminator connectionDeterminator;
    [SerializeField] Transform verticleLayoutSpace;
    [SerializeField] GameObject inputingPromptGO;

    [Space]
    [SerializeField] ChatMessageUnit chatMessageUnitPrefab;
    [SerializeField] Transform chatMessageUnitParent;

    [Space]
    [SerializeField] TalkMessageUnit talkMessageUnitPrefab;
    [SerializeField] Transform talkMessageUnitParent;

    [Space]
    [SerializeField] ChatMessageChoiceGroup chatMessageChoiceGroupPrefab;
    [SerializeField] Transform chatMessageChoiceGroupParent;

    [Space]
    [SerializeField] ChatMessageTimestamp chatMessageTimestampPrefab;

    [Space]
    [SerializeField] ScrollRect chatMessageScrollRect;

    [Header("Data")]
    [SerializeField] List<ChatOwner> chatOwners;
    [SerializeField] List<ChatMessage> chatMessages;
    [SerializeField] List<ChatMessageSequence> chatMessageSequences;
    [SerializeField] List<ChatMessageSequence> talkSequences;
    [SerializeField] List<ChatChoiceGroup> chatChoiceGroups;
    
    private ChatMessageChoiceGroup currentChatMessageChoiceGroup;

    private ChatMessageSequence currentChatMessageSequence;
    private Coroutine chatMessageSequenceCoroutine;
    private ChatMessageSequence currentTalkSequence;
    private Coroutine talkSequenceCoroutine;
    private TalkMessageUnit currentTalkMessageUnit;
    private ChatMessageSequence currentTalkResponseSequence;

    private bool hasQueuedChatMesssage
    {
        get => chatMessageSequenceCoroutine != null;
    }

    private bool hasQueuedTalkMessage
    {
        get => talkSequenceCoroutine != null;
    }

    void OnEnable()
    {
        connectionDeterminator.OnConnected += OnConnected;
        connectionDeterminator.OnDisconnected += OnDisconnected;
        talkButton.onClick.AddListener(OnTalkButtonClicked);
        chatButton.onClick.AddListener(OnChatButtonClicked);
    }

    void OnDisable()
    {
        connectionDeterminator.OnConnected -= OnConnected;
        connectionDeterminator.OnDisconnected -= OnDisconnected;
        talkButton.onClick.RemoveListener(OnTalkButtonClicked);
        chatButton.onClick.RemoveListener(OnChatButtonClicked);
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

        chatMessageUnit.Setup(chatMessage.message, chatOwner.name, chatOwner.icon, !chatOwner.isSelf);
        chatMessageScrollRect.verticalNormalizedPosition = 0f;
        verticleLayoutSpace.SetAsLastSibling();
    }

    private void SendChatMessage(ChatMessage chatMessage)
    {
        if (chatMessage == null)
        {
            Debug.LogError("Chat message is null");
            return;
        }

        if (!chatBox.activeSelf && chatMessageHintGO.IsNotNull())
        {
            chatMessageHintGO.SetActive(true);
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
            //chatBox.SetActive(true);
        }

        chatMessageUnit.Setup(chatMessage.message, chatOwner.name, chatOwner.icon, !chatOwner.isSelf);
        chatMessageScrollRect.verticalNormalizedPosition = 0f;
        verticleLayoutSpace.SetAsLastSibling();
    }

    private void SendTalkMessage(string sequenceId, ChatMessage chatMessage)
    {
        if (chatMessage == null)
        {
            Debug.LogError("Chat message is null");
            return;
        }

        TalkMessageUnit talkMessageUnit = Instantiate(talkMessageUnitPrefab, talkMessageUnitParent);
        talkMessageUnit.Setup(chatMessage.message, chatMessage.time);

        if (currentTalkMessageUnit != null && currentTalkMessageUnit.gameObject.activeSelf)
        {
            currentTalkMessageUnit.gameObject.SetActive(false);
        }

        if (!string.IsNullOrEmpty(chatMessage.id) && AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(sequenceId + "-" + chatMessage.id);
        }

        currentTalkMessageUnit = talkMessageUnit;
    }

    public void QueueChatMessageSequence(string sequenceId)
    {
        var sequence = chatMessageSequences.Find(sequence => sequence.id == sequenceId);
        if (sequence == null)
        {
            Debug.LogError($"Chat message sequence with id {sequenceId} not found");
            return;
        }

        currentChatMessageSequence = sequence;
        
        if (chatMessageSequenceCoroutine != null)
        {
            StopCoroutine(chatMessageSequenceCoroutine);
        }
        chatMessageSequenceCoroutine = StartCoroutine(ChatMessageSequenceCoroutine());
    }

    public void QueueTalkSequence(string sequenceId)
    {
        var sequence = talkSequences.Find(sequence => sequence.id == sequenceId);
        if (sequence == null)
        {
            Debug.LogError($"Talk sequence with id {sequenceId} not found");
            return;
        }

        currentTalkSequence = sequence;
        
        if (talkSequenceCoroutine != null)
        {
            StopCoroutine(talkSequenceCoroutine);
        }
        talkSequenceCoroutine = StartCoroutine(TalkSequenceCoroutine());
    }

    public void QueueTalkResponse(string messageId)
    {
        var sequence = talkSequences.Find(sequence => sequence.id == messageId);
        if (sequence == null)
        {
            Debug.LogError($"Talk response message with id {messageId} not found");
            return;
        }

        currentTalkResponseSequence = sequence;
        if (!connectionDeterminator.IsConnected)
        {
            //talkButtonGO.SetActive(true);
        }

        if (talkMessageHintGO.IsNotNull())
        {
            talkMessageHintGO.SetActive(true);
        }
    }

    public void InsertChatMessageTimestamp(string timestamp)
    {
        ChatMessageTimestamp chatMessageTimestamp = Instantiate(chatMessageTimestampPrefab, chatMessageUnitParent);
        chatMessageTimestamp.Setup(timestamp);
    }

    public void OnTalkButtonClicked()
    {
        if (currentTalkResponseSequence != null)
        {
            QueueTalkSequence(currentTalkResponseSequence.id);
            currentTalkResponseSequence = null;
            //talkButtonGO.SetActive(false);
        }

        if (talkMessageHintGO.IsNotNull())
        {
            talkMessageHintGO.SetActive(false);
        }
    }

    public void OnChatButtonClicked()
    {
        if (connectionDeterminator.IsNotNull() && connectionDeterminator.IsConnected)
        {
            chatBox.SetActive(true);
        }

        if (chatMessageHintGO.IsNotNull())
        {
            chatMessageHintGO.SetActive(false);
        }
    }

    public void ShowChatChoiceGroup(string choiceGroupId)
    {
        ChatChoiceGroup chatChoiceGroup = chatChoiceGroups.Find(group => group.id == choiceGroupId);
        if (chatChoiceGroup == null)
        {
            Debug.LogError($"Chat choice group with id {choiceGroupId} not found");
            return;
        }

        if (chatMessageHintGO.IsNotNull() && !chatBox.activeSelf)
        {
            chatMessageHintGO.SetActive(true);
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
        if (hasQueuedChatMesssage && chatBox.IsNotNull())
        {
            //chatBox.SetActive(true);
        }

        if (chatButton.IsNotNull())
        {
            chatButton.interactable = true;
        }

        if (talkButton.IsNotNull())
        {
            talkButton.interactable = false;
        }

        if (chatButtonGO.IsNotNull())
        {
            //chatButtonGO.SetActive(true);
        }

        if (talkButtonGO.IsNotNull())
        {
            //talkButtonGO.SetActive(false);
        }
    }

    void OnDisconnected()
    {
        if (chatBox.IsNotNull())
        {
            chatBox.SetActive(false);
        }

        if (chatButton.IsNotNull())
        {
            chatButton.interactable = false;
        }

        if (talkButton.IsNotNull())
        {
            talkButton.interactable = true;
        }

        if (chatButtonGO.IsNotNull())
        {
            //chatButtonGO.SetActive(false);
        }

        if (currentTalkResponseSequence != null && talkButtonGO.IsNotNull())
        {
            //talkButtonGO.SetActive(true);
        }
    }

    IEnumerator ChatMessageSequenceCoroutine()
    {
        var sequence = currentChatMessageSequence;
        while (!connectionDeterminator.IsConnected)
        {
            yield return null;
        }

        if (sequence == null || sequence.messages.Count == 0)
        {
            Debug.LogError("No chat message sequence to play");
            yield break;
        }

        yield return new WaitForSeconds(sequence.delay);

        if (chatMessageHintGO.IsNotNull() && !chatBox.activeSelf)
        {
            chatMessageHintGO.SetActive(true);
        }

        sequence.onSequenceStarted?.Invoke();
        inputingPromptGO.SetActive(true);

        var time = 0f;
        for (int i = 0; i < sequence.messages.Count; i++)
        {
            SendChatMessage(sequence.messages[i]);
            time = sequence.messages[i].time;

            while (time > 0)
            {
                if (connectionDeterminator.IsConnected && chatBox.activeSelf)
                {
                    time -= Time.deltaTime;
                }
                yield return null;
            }
        }

        chatMessageSequenceCoroutine = null;
        sequence.onSequenceFinished?.Invoke();
        inputingPromptGO.SetActive(false);
    }

    IEnumerator TalkSequenceCoroutine()
    {
        var sequence = currentTalkSequence;
        while (connectionDeterminator.IsConnected)
        {
            yield return null;
        }

        if (sequence == null || sequence.messages.Count == 0)
        {
            Debug.LogError("No chat message sequence to play");
            yield break;
        }

        yield return new WaitForSeconds(sequence.delay);

        sequence.onSequenceStarted?.Invoke();

        var time = 0f;
        for (int i = 0; i < sequence.messages.Count; i++)
        {
            SendTalkMessage(sequence.id, sequence.messages[i]);
            time = sequence.messages[i].time;

            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
        }

        talkSequenceCoroutine = null;
        sequence.onSequenceFinished?.Invoke();
    }
}
