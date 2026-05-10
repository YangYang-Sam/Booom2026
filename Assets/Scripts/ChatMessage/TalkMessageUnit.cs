using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalkMessageUnit : MonoBehaviour
{
    [SerializeField] TMP_Text messageText;
    
    public void Setup(string message, float time)
    {
        messageText.text = message;
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(TalkCoroutine(time));
    }

    IEnumerator TalkCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
