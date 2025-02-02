using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    instance = go.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    public Sound[] musicTracks;
    public Sound[] soundEffects;

    private Sound currentMusic;
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    private GameObject tempChildObj;
    private float globalPitch = 1f;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize all audio sources
        InitializeAudioSources();
    }

    private void InitializeAudioSources()
    {
        // Initialize music tracks
        foreach (Sound s in musicTracks)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.setMusic(true);
            soundDictionary[s.name] = s;
        }

        // Initialize sound effects
        foreach (Sound s in soundEffects)
        {
            if (!s.hasReverb())
            {
                s.source = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                tempChildObj = new GameObject();
                tempChildObj.transform.SetParent(gameObject.transform);
                s.source = tempChildObj.AddComponent<AudioSource>();
            }
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.setMusic(false);
            soundDictionary[s.name] = s;

        }
        foreach (Sound s in soundEffects)
        {
            if (s.hasReverb())
            {
                soundDictionary[s.name].source.AddComponent<AudioReverbFilter>();
            }
        }
    }

    public void PlayMusic(string name, float fadeInDuration = 0f)
    {
        if (!soundDictionary.TryGetValue(name, out Sound newMusic))
        {
            Debug.LogWarning($"Music track '{name}' not found!");
            return;
        }

        // If there's currently playing music, fade it out
        if (currentMusic != null && currentMusic.source.isPlaying)
        {
            StartCoroutine(FadeOut(currentMusic.source, fadeInDuration));
        }

        // Start playing new music
        StartCoroutine(FadeIn(newMusic.source, newMusic.volume, fadeInDuration));
        currentMusic = newMusic;
    }

    public void PlaySound(string name)
    {
        if (!soundDictionary.TryGetValue(name, out Sound sound))
        {
            Debug.LogWarning($"Sound effect '{name}' not found!");
            return;
        }

        sound.source.Play();
    }

    //Deprecated way of playing sfx
    public void Play(string name)
    {
        PlaySound(name);
    }

    public void StopMusic(float fadeOutDuration = 0f)
    {
        if (currentMusic != null && currentMusic.source.isPlaying)
        {
            StartCoroutine(FadeOut(currentMusic.source, fadeOutDuration));
        }
    }

    public void StopSound(string name)
    {
        foreach (string key in soundDictionary.Keys) {
            Debug.Log("key: " + key);
        }
        Debug.Log(soundDictionary.Keys.Count);
        if (!soundDictionary.TryGetValue(name, out Sound sound))
        {
            Debug.LogWarning($"Sound effect '{name}' not found!");
            return;
        }

        sound.source.Stop();
    }

    // Returns whether or not this sound is currently playing
    public bool IsSoundPlaying(string name)
    {
        if (!soundDictionary.TryGetValue(name, out Sound sound))
        {
            Debug.LogWarning($"Sound effect '{name}' not found!");
            return false;
        }
        return soundDictionary[name].source.isPlaying;
    }

    private System.Collections.IEnumerator FadeIn(AudioSource audioSource, float targetVolume, float duration)
    {
        audioSource.volume = 0f;
        audioSource.Play();

        float startTime = Time.time;
        float startVolume = 0f;

        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            float normalizedTime = elapsed / duration;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, normalizedTime);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private System.Collections.IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startTime = Time.time;
        float startVolume = audioSource.volume;

        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            float normalizedTime = elapsed / duration;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, normalizedTime);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    // Utility methods for volume control
    public void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicKey", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXKey", 1f);
        foreach (KeyValuePair<string, Sound> s in soundDictionary)
        {
            if (s.Value.isMusic())
            {
                s.Value.source.volume = musicVolume * s.Value.volume;
            }
            else
            {
                s.Value.source.volume = sfxVolume * s.Value.volume;
            }
        }
    }

    //Make all sfx play slower or faster
    public System.Collections.IEnumerator ChangePitch(float pitch, float duration)
    {
        float startTime = Time.time;
        float startPitch = globalPitch;

        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            float normalizedTime = elapsed / duration;
            float newPitch = Mathf.Lerp(startPitch, pitch, normalizedTime);
            foreach (KeyValuePair<string, Sound> s in soundDictionary)
            {
                s.Value.source.pitch = newPitch;
            }
            globalPitch = newPitch;
            yield return null;
        }

        foreach (KeyValuePair<string, Sound> s in soundDictionary)
        {
            s.Value.source.pitch = pitch;
        }
        globalPitch = pitch;
    }
}