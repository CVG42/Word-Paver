using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<IAudioSource>, IAudioSource
{
    private const float DEFAULT_SFX_VOLUME = 0.8f;
    private const float DEFAULT_MUSIC_VOLUME = 0.8f;
    private const float DEFAULT_MASTER_VOLUME = 0.6f;

    private const string MASTER_VOLUME_KEY = "MASTER_VOL";
    private const string SFX_VOLUME_KEY = "SFX_VOL";
    private const string MUSIC_VOLUME_KEY = "BGM_VOL";

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioDatabase _audioDatabase;
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private AudioSource _bgmAudioSource;

    public float CurrentSFXVolume { get; private set; }
    public float CurrentMusicVolume { get; private set; }
    public float CurrentMasterVolume { get; private set; }

    public event Action<float> OnSFXVolumeChange;
    public event Action<float> OnMusicVolumeChange;
    public event Action<float> OnMasterVolumeChange;

    private void Start()
    {
        LoadAudioSettings();
    }

    private void LoadAudioSettings()
    {
        CurrentMasterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_MASTER_VOLUME);
        CurrentSFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, DEFAULT_SFX_VOLUME);
        CurrentMusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_MUSIC_VOLUME);

        ApplyVolume("master_vol", CurrentMasterVolume);
        ApplyVolume("sfx_vol", CurrentSFXVolume);
        ApplyVolume("bgm_vol", CurrentMusicVolume);
    }

    public void SetMasterVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        ApplyVolume("master_vol", volume);

        CurrentMasterVolume = volume;

        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        PlayerPrefs.Save();

        OnMasterVolumeChange?.Invoke(volume);
    }

    public void SetSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        ApplyVolume("sfx_vol", volume);

        CurrentSFXVolume = volume;

        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
        PlayerPrefs.Save();

        OnSFXVolumeChange?.Invoke(volume);
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        ApplyVolume("bgm_vol", volume);

        CurrentMusicVolume = volume;

        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        PlayerPrefs.Save();

        OnMusicVolumeChange?.Invoke(volume);
    }

    public void PlayLevelMusic(string audioName)
    {
        _bgmAudioSource.clip = (_audioDatabase.GetAudio(audioName));
        _bgmAudioSource.Play();
    }

    public void PlayOneShot(string audioName)
    {
        _sfxAudioSource.PlayOneShot(_audioDatabase.GetAudio(audioName));
    }

    private void ApplyVolume(string parameter, float value)
    {
        float dB = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        _audioMixer.SetFloat(parameter, dB);
    }

    public void PlayHoverSFX() => PlayOneShot("Hover");
    public void PlaySelectedSFX() => PlayOneShot("Select");
    public void PlaySpawnSFX() => PlayOneShot("Spawn");
    public void PlayTypeSFX() => PlayOneShot("Type");
}

public interface IAudioSource
{
    event Action<float> OnMasterVolumeChange;
    event Action<float> OnSFXVolumeChange;
    event Action<float> OnMusicVolumeChange;

    float CurrentMasterVolume { get; }
    float CurrentSFXVolume { get; }
    float CurrentMusicVolume { get; }

    void PlayLevelMusic(string audioName);
    void PlayOneShot(string audioName);
    void SetMasterVolume(float volume);
    void SetSFXVolume(float volume);
    void SetMusicVolume(float volume);

    void PlayHoverSFX();
    void PlaySelectedSFX();
    void PlaySpawnSFX();
    void PlayTypeSFX();
}
