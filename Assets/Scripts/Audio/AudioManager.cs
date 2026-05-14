using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] List<AudioClip> sfxClips;

    [Header("Audio Clips")]
    public AudioClip normalBgm;
    public AudioClip bossIntro;
    public AudioClip bossLoop;
    public AudioClip flyIntro;
    public AudioClip flyLoop;

    public float normalBgmVolume;
    public float bossBgmVolume;
    public float flyBgmVolume;

    public float fadeTime = 2f;

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
        if (musicSource != null)
        {
            musicSource.clip = normalBgm;
            musicSource.Play();
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

    public void ChangeBGMToNormal()
    {
        var newSource = musicSource.gameObject.AddComponent<AudioSource>();

        newSource.clip = normalBgm;
        newSource.Play();
        newSource.volume = 0;
        newSource.DOFade(normalBgmVolume, fadeTime);

        musicSource.DOFade(0, fadeTime).OnComplete(() => {
            Destroy(musicSource);
            musicSource = newSource;
        });
    }

    public void ChangeBGMToBoss()
    {
        var newSource = musicSource.gameObject.AddComponent<AudioSource>();

        newSource.PlayOneShot(bossIntro);   
        newSource.clip = bossLoop;
        newSource.PlayScheduled(AudioSettings.dspTime + bossIntro.length);
        newSource.volume = 0;
        newSource.DOFade(bossBgmVolume, fadeTime);

        musicSource.DOFade(0, fadeTime).OnComplete(() => {
            Destroy(musicSource);
            musicSource = newSource;
        });
    }

    public void ChangeBGMToFly()
    {
        var newSource = musicSource.gameObject.AddComponent<AudioSource>();

        newSource.PlayOneShot(flyIntro);   
        newSource.clip = flyLoop;
        newSource.PlayScheduled(AudioSettings.dspTime + flyIntro.length);
        newSource.volume = 0;
        newSource.DOFade(flyBgmVolume, fadeTime);

        musicSource.DOFade(0, fadeTime).OnComplete(() => {
            Destroy(musicSource);
            musicSource = newSource;
        });
    }
}
