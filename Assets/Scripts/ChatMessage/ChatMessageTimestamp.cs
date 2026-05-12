using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatMessageTimestamp : MonoBehaviour
{
    [SerializeField] TMP_Text timestampText;

    public void Setup(string timestamp)
    {
        timestampText.text = timestamp;
    }
}
