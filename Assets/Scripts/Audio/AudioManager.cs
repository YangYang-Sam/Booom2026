using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JTUtility;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] List<AudioClip> sfxClips;
    [SerializeField] AudioClip normalBgm;
    [SerializeField] float bgmVolume = 1f;

    public float fadeTime = 2f;

    private AudioSource bgmSource;
    private AudioSource bgmCrossfadeSource;

    private static AudioManager _instance;
    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AudioManager>();
            }
            return _instance;
        }
    }

    private Dictionary<AudioClip, int> playingSFX = new Dictionary<AudioClip, int>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (bgmSource.IsNull())
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
        }

        if (bgmSource.IsNotNull() && normalBgm.IsNotNull())
        {
            bgmSource.clip = normalBgm;
            bgmSource.Play();
        }
    }

    void Update()
    {
        if (playingSFX.Count > 0)
        {
            playingSFX.Clear();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (playingSFX.ContainsKey(clip) && playingSFX[clip] > 1)
        {
            return;
        }
        playingSFX[clip] = playingSFX.ContainsKey(clip) ? playingSFX[clip] + 1 : 1;

        var sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.PlayOneShot(clip);
        Destroy(sfxSource, clip.length);
    }

    public void PlaySFX(string clipName)
    {
        var clip = sfxClips.Find(c => c.name == clipName);
        if (clip != null)
        {
            PlaySFX(clip);
        }
        else
        {
            Debug.LogError($"SFX: \"{clipName}\" not found");
        }
    }
    
    public AudioSource PlayLoopSFX(AudioClip clip)
    {
        var sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.clip = clip;
        sfxSource.Play();
        sfxSource.loop = true;
        return sfxSource;
    }

    public AudioSource PlayLoopSFX(string clipName)
    {
        var clip = sfxClips.Find(c => c.name == clipName);
        if (clip != null)
        {
            return PlayLoopSFX(clip);
        }
        
        return null;
    }
    
    public void PlayBGM(AudioClip clip)
    {
        if (clip.IsNull() || bgmSource.IsNull() || bgmSource.clip == clip)
        {
            return;
        }

        KillBgmFadeTweens();

        if (!bgmSource.isPlaying || bgmSource.clip == null)
        {
            bgmSource.volume = bgmVolume;
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
            return;
        }

        EnsureBgmCrossfadeSource();

        float targetVolume = bgmSource.volume > 0.001f ? bgmSource.volume : bgmVolume;

        bgmCrossfadeSource.clip = clip;
        bgmCrossfadeSource.loop = bgmSource.loop;
        bgmCrossfadeSource.volume = 0f;
        bgmCrossfadeSource.time = 0f;
        bgmCrossfadeSource.Play();

        bgmSource.DOFade(0f, fadeTime).SetEase(Ease.Linear).SetUpdate(true);
        bgmCrossfadeSource.DOFade(targetVolume, fadeTime).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            bgmSource.Stop();
            bgmSource.clip = clip;
            bgmSource.time = bgmCrossfadeSource.time;
            bgmSource.volume = targetVolume;
            bgmSource.Play();

            bgmCrossfadeSource.Stop();
            bgmCrossfadeSource.clip = null;
            bgmCrossfadeSource.volume = 0f;
        });
    }

    private void KillBgmFadeTweens()
    {
        if (bgmSource.IsNotNull())
        {
            bgmSource.DOKill();
        }

        if (bgmCrossfadeSource.IsNotNull())
        {
            bgmCrossfadeSource.DOKill();
        }
    }

    private void EnsureBgmCrossfadeSource()
    {
        if (bgmCrossfadeSource.IsNotNull())
        {
            return;
        }

        bgmCrossfadeSource = gameObject.AddComponent<AudioSource>();
        bgmCrossfadeSource.playOnAwake = false;
    }
}
