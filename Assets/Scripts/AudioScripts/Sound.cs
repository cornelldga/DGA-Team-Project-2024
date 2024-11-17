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
}
