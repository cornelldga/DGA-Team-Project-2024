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
    public bool music;

    //Does this have reverb?
    public bool reverb;

    /// <summary>
    /// Returns if music clip
    /// </summary>
    public bool isMusic() {
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
