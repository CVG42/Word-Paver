using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Start()
    {
        LoadVolumeValues();
        _masterSlider.onValueChanged.AddListener(AudioManager.Source.SetMasterVolume);
        _sfxSlider.onValueChanged.AddListener(AudioManager.Source.SetSFXVolume);
        _bgmSlider.onValueChanged.AddListener(AudioManager.Source.SetMusicVolume);
    }

    private void LoadVolumeValues()
    {
        _masterSlider.value = AudioManager.Source.CurrentMasterVolume;
        _sfxSlider.value = AudioManager.Source.CurrentSFXVolume;
        _bgmSlider.value = AudioManager.Source.CurrentMusicVolume;
    }
}
