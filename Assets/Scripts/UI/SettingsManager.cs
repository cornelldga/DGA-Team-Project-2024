using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolumeKey", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicKey", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXKey", 1f);
    }

    public void OnMasterSliderChanged()
    {
        PlayerPrefs.SetFloat("MasterVolumeKey", masterSlider.value);
        audioManager.LoadVolume();
        
    }

    public void OnMusicSliderChanged()
    {
        PlayerPrefs.SetFloat("MusicKey", musicSlider.value);
        audioManager.LoadVolume();
    }

    public void OnSFXSliderChanged()
    {
        PlayerPrefs.SetFloat("SFXKey", sfxSlider.value);
        audioManager.LoadVolume();
    }
}
