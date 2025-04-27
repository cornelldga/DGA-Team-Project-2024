using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

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
    public Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    private GameObject tempChildObj;
    private float globalPitch = 1f;

    public List<AudioSource> tempAudioSourceList = new List<AudioSource>();

    private float lowestPitch = 0.5f; //The lowest pitch that can be set, used by PlaySoundAtPoint

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += StopSoundsOnLoad;

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
        //Debug.Log("Size of sfx: " + soundEffects.Length);
        // Initialize sound effects
        foreach (Sound s in soundEffects)
        {
            //Debug.Log("Sound effect: " + s.name);
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

            if(s.name.ContainsInsensitive("Beetle") ||
                s.name.ContainsInsensitive("Grasshopper") || 
                s.name.ContainsInsensitive("Ladybug") ||
                s.name.ContainsInsensitive("Rolypoly"))
            {
                //Pedestrian sounds
                s.source.volume = s.volume + 0.4f;
                if (s.name.ContainsInsensitive("hurt"))
                {
                    s.source.volume = s.volume + 0.15f;
                }
            }
            else
            {
                s.source.volume = s.volume;
            }

            s.source.clip = s.clip;
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
        //Debug.Log("PlaySound Called with name " + name);
        if (!soundDictionary.ContainsKey(name))
        {
            //Debug.Log("Sound effect " + name + " not found!");
            return;
        }
        //Debug.Log("Playing sound: " + name);
        soundDictionary[name].source.Play();
    }

    public void PlaySoundAtPoint(string name, Vector3 position)
    {
        if (!soundDictionary.ContainsKey(name))
        {
            Debug.LogWarning($"Sound effect '{name}' not found!");
            return;
        }

        //Clear out old audio sources
        foreach (AudioSource a in tempAudioSourceList.ToArray())
        {
            if (a == null || !a.isPlaying)
            {
                //This is NOT what is removing the audio source for playing voicelines
                if (a == null) Debug.Log("Audio source is null, removing");
                else Debug.Log("Audio source " +a.name+ " is not playing, removing");
                tempAudioSourceList.Remove(a);
            }
        }
        //Debug.Log("Temp audio source length" + tempAudioSourceList.Count);

        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource tempAudioSourceObj = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        tempAudioSourceObj.clip = soundDictionary[name].source.clip;
        tempAudioSourceObj.volume = soundDictionary[name].source.volume;

        //Custom settings for spatialized audio
        tempAudioSourceObj.dopplerLevel = 0f;
        tempAudioSourceObj.spatialBlend = 1f; //This is actually in the default implementation, but is still relevant for spatialization
        tempAudioSourceObj.minDistance = 40f;
        tempAudioSourceObj.maxDistance = 500f;

        tempAudioSourceList.Add(tempAudioSourceObj);

        tempAudioSourceObj.Play();
        UnityEngine.Object.Destroy(gameObject, soundDictionary[name].source.clip.length * (1.0f/lowestPitch));
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
        //Debug.Log(soundDictionary.Keys.Count);
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

    public void SetLooping(string name, bool doesLoop)
    {
        if (!soundDictionary.TryGetValue(name, out Sound sound))
        {
            Debug.LogWarning($"Sound effect '{name}' not found!");
            return;
        }

        sound.source.loop = doesLoop;
    }

    private System.Collections.IEnumerator FadeIn(AudioSource audioSource, float targetVolume, float duration)
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolumeKey", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicKey", 1f);
        audioSource.volume = 0f;
        audioSource.Play();

        float startTime = Time.time;
        float startVolume = 0f;

        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            float normalizedTime = elapsed / duration;
            audioSource.volume = Mathf.Lerp(startVolume, masterVolume * musicVolume * targetVolume, normalizedTime);
            yield return null;
        }

        audioSource.volume = masterVolume * musicVolume * targetVolume;
    }

    private System.Collections.IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolumeKey", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicKey", 1f);
        float startTime = Time.time;
        float startVolume = audioSource.volume;

        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            float normalizedTime = elapsed / duration;
            audioSource.volume = Mathf.Lerp(masterVolume * musicVolume * startVolume, 0f, normalizedTime);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = masterVolume * musicVolume * startVolume;
    }

    // Utility methods for volume control
    public void LoadVolume()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolumeKey", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicKey", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXKey", 1f);
        foreach (KeyValuePair<string, Sound> s in soundDictionary)
        {
            if (s.Value.isMusic())
            {
                s.Value.source.volume = masterVolume * musicVolume * s.Value.volume;
            }
            else
            {
                s.Value.source.volume = masterVolume * sfxVolume * s.Value.volume;
            }
        }
    }

    //Make all sfx play slower or faster
    public System.Collections.IEnumerator ChangePitch(float pitch, float duration)
    {
        Assert.IsTrue(pitch >= lowestPitch);
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
            foreach (AudioSource a in tempAudioSourceList)
            {
                if(a != null) a.pitch = newPitch;
            }
            globalPitch = newPitch;
            yield return null;
        }

        foreach (KeyValuePair<string, Sound> s in soundDictionary)
        {
            s.Value.source.pitch = pitch;
        }
        foreach (AudioSource a in tempAudioSourceList)
        {
            if (a != null) a.pitch = pitch;
        }
        globalPitch = pitch;
    }

    void StopSoundsOnLoad(Scene scene, LoadSceneMode loadMode)
    {
        StopSound("sfx_SirenLong");
    }
}