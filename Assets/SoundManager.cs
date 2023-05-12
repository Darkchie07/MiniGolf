using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    public static SoundManager Instance { get; private set; }

    [SerializeField] private static AudioSource bgmsource;
    [SerializeField] private static AudioSource sfxsource;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource sfx;
    private const string MUTE_PREF_KEY = "MutePreference";
    private const int MUTED = 1;
    private const int UN_MUTED = 0;
    private const string BGM_VOLUME = "BGMVolume";
    private const string SFX_VOLUME = "SFXVolume";
    private bool muted;

    public static float BgmVolume { get => bgmsource.volume; }
    public static float SfxVolume { get => sfxsource.volume; }
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        bgmsource = bgm;
        sfxsource = sfx;
        if (!PlayerPrefs.HasKey(MUTE_PREF_KEY))
        {
            SetMute(false);
        }
        else
        {
            SetMute(IsMuted());
        }
    }

    public void ToggleMute()
    {
        if (IsMuted())
        {
            PlayerPrefs.SetInt(MUTE_PREF_KEY, UN_MUTED);
            bgmsource.volume = PlayerPrefs.GetFloat(BGM_VOLUME);
            sfxsource.volume = PlayerPrefs.GetFloat(SFX_VOLUME);
            Debug.Log(PlayerPrefs.GetFloat(BGM_VOLUME));
            bgmsource.mute = false;
            sfxsource.mute = false;
            if (muted)
            { 
                bgmsource.Play();
            }
        }
        else
        {
            PlayerPrefs.SetInt(MUTE_PREF_KEY, MUTED);
            bgmsource.volume = 0;
            sfxsource.volume = 0;
        }
    }

    public static bool IsMuted()
    {
        bool i = (PlayerPrefs.GetInt(MUTE_PREF_KEY, UN_MUTED) == MUTED);
        return i;
    }

    void SetMute(bool isMuted)
    {
        bgmsource.mute = isMuted;
        sfxsource.mute = isMuted;
        if (isMuted)
        {
            bgmsource.volume = 0;
            sfxsource.volume = 0;
        }
        else
        {
            LoadBGM();
            LoadSFX();
        }
    }

    public void playSFX(AudioClip _clip)
    {
        sfxsource.clip = _clip;
        sfxsource.Play();
    }

    public void PlayBgm()
    {
        bgmsource.Play();
        bgmsource.loop = true;
    }
    
    public void SetBgmVolume(float value)
    { 
        bgmsource.volume = value;
        PlayerPrefs.SetFloat(BGM_VOLUME, value);
    }
    
    public void SetSfxVolume(float value)
    {
        sfxsource.volume = value;
        PlayerPrefs.SetFloat(SFX_VOLUME, value);
    }
    
    public void LoadBGM()
    {
        if (PlayerPrefs.HasKey(BGM_VOLUME))
        {
            bgmsource.volume = PlayerPrefs.GetFloat(BGM_VOLUME);
            Debug.Log(PlayerPrefs.GetFloat(BGM_VOLUME));
        }
        else
        {
            bgmsource.volume = 1;
            PlayerPrefs.SetFloat(BGM_VOLUME, 1);
        }
    }
    
    public void LoadSFX()
    {
        if (PlayerPrefs.HasKey(SFX_VOLUME))
        {
            sfxsource.volume = PlayerPrefs.GetFloat(SFX_VOLUME);
        }
        else
        {
            sfxsource.volume = 1;
            PlayerPrefs.SetFloat(SFX_VOLUME, 1);
        }
    }
    
}
