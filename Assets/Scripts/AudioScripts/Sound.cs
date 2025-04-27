using UnityEngine.Audio;
using UnityEngine;

//This class is used to store information about a sound

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    //Is this music?
    private bool music;

    //Choke Group
    public int chokeGroup;

    //Does this have reverb?
    public bool reverb;

    [Range(-10000, 0)]
    public int dryLevel = 0;

    [Range(-10000, 0)]
    public int room = 0;

    [Range(0.1f, 20f)]
    public float decayTime = 1f;

    [Range(-10000, 1000)]
    public float reflectionsLevel = 0f;

    [Range(0f, 0.3f)]
    public int reflectionsDelay = 0;

    [Range(-10000, 2000)]
    public int reverbLevel = 0;

    [Range(0f, 0.1f)]
    public float reverbDelay = 0f;

    /// <summary>
    /// Returns if music clip
    /// </summary>
    
    public void setMusic(bool v)
    {
        music = v;
    }
    public bool isMusic()
    {
        return music;
    }

    /// <summary>
    /// Returns if has reverb
    /// </summary>
    public bool hasReverb()
    {
        return reverb;
    }
}
