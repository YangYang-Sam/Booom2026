using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerAgent : MonoBehaviour
{
    public void PlaySFX(AudioClip clip)
    {
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager not found");
            return;
        }
        
        AudioManager.instance.PlaySFX(clip);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager not found");
            return;
        }

        AudioManager.instance.PlayBGM(clip);
    }

    public void ClearBGM()
    {
        if (AudioManager.instance == null)
        {
            Debug.LogError("AudioManager not found");
            return;
        }
        
        AudioManager.instance.ClearBGM();
    }
}
