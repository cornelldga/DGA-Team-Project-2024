using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

//This class is used to manage sound effects and music 

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    Sound bgm;

    //Vestiges of the summer project, might be important later
    //
    //[SerializeField] public List<AudioClip> SFXClips = new List<AudioClip>();
    //[SerializeField] public List<AudioClip> MusicClips = new List<AudioClip>();

    // Start is called before the first frame update
    void Awake()
    {
        //If there is no instance of the AudioManager, make this the instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                if (s.hasReverb())
                {
                    //s.source.clip.AddComponent<AudioReverbFilter>();
                    gameObject.AddComponent<AudioReverbFilter>();
                }
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        LoadVolume();
    }

    /// <summary>
    /// Find the sound in the array of sounds and play it if it exists
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
    /// <summary>
    /// Find the sound in the array of sounds and stop it if it exists
    /// </summary>
    /// <param name="name"></param>
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (s != null)
        {
            s.source.Stop();
        }
    }
    /// <summary>
    /// Stop all other music and play the music with the name passed in
    /// </summary>
    /// <param name="name"></param>
    public void PlayMusic(string name)
    {
        if (bgm != null)
        {
            if (bgm.name == name)
            {
                return;
            }
        }
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                bgm = s;
            }
        }
        StopMusic();
        Play(name);
    }

    /// <summary>
    /// Stop all music
    /// </summary>
    private void StopMusic()
    {
        foreach (Sound s in sounds)
        {
            if (s.isMusic())
            {
                s.source.Stop();
            }
        }
    }

    /// <summary>
    /// Load the volume from the player prefs
    /// </summary>
    public void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicKey", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXKey", 1f);

        foreach (Sound s in sounds)
        {
            if (s.isMusic())
            {
                s.source.volume = musicVolume * s.volume;
            }
            else
            {
                s.source.volume = sfxVolume * s.volume;
            }
        }

    }
}
