using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTUtility;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> sfxClips;
    [SerializeField] private bool playOnEnable = false;
    [SerializeField] private bool playOnDisable = false;
    [SerializeField] private bool playOnStart = false;
    [SerializeField] private bool playOnDestroy = false;

    private List<AudioSource> sfxSources = new List<AudioSource>();

    void OnEnable()
    {
        if (playOnEnable)
        {
            PlaySFX2();
        }
    }

    void OnDisable()
    {
        if (playOnDisable)
        {
            PlaySFX2();
        }
        StopAllSFXLoop();
    }

    void OnStart()
    {
        if (playOnStart)
        {
            PlaySFX2();
        }
    }

    void OnDestroy()
    {
        if (playOnDestroy)
        {
            PlaySFX2();
        }
        StopAllSFXLoop();
    }

    public void PlaySFX(AudioClip clip)
    {
        AudioManager.instance.PlaySFX(clip);
    }

    public void PlaySFX2()
    {
        var clip = sfxClips.PickRandom();
        if (clip == null)
        {
            Debug.LogWarning($"SFXPlayer: {gameObject.name} No sfx clips found");
            return;
        }

        AudioManager.instance.PlaySFX(clip);
    }

    public void PlaySFXLoop(AudioClip clip)
    {
        var sfxSource = AudioManager.instance.PlayLoopSFX(clip);
        sfxSources.Add(sfxSource);
    }

    public void StopAllSFXLoop()
    {
        foreach (var sfxSource in sfxSources)
        {
            sfxSource.Stop();
            Destroy(sfxSource);
        }

        sfxSources.Clear();
    }
}
