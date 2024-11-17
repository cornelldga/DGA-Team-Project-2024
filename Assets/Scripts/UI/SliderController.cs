using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    // Start is called before the first frame update
    //get the slider attached to this gameobject
    private Slider slider;
    private AudioManager audioManager;
    [SerializeField] private bool isMusicSlider;
    [SerializeField] private bool isSFXSlider;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        this.slider = GetComponent<Slider>();
        if (isMusicSlider)
        {
            this.slider.value = PlayerPrefs.GetFloat("MusicKey", 1);
            audioManager.LoadVolume();
        }
        else if (isSFXSlider)
        {
            this.slider.value = PlayerPrefs.GetFloat("SFXKey", 1);
            audioManager.LoadVolume();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMusicVolume()
    {
        audioManager.SetMusicVolume(slider.value);
        audioManager.LoadVolume();
        Debug.Log("Music Volume: " + slider.value);
    }

    public void SetSFXVolume()
    {
        audioManager.SetSFXVolume(slider.value);
        audioManager.LoadVolume();
    }
}
