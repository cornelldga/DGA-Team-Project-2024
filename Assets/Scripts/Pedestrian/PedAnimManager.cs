using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PedestrianAudioSet
{
    public string name; // For debugging, assign pedestrian type name
    public RuntimeAnimatorController animatorController;
    //public AudioClip[] orderCompleteSounds;
    //public AudioClip[] hurtSounds;
    //public AudioClip[] takeOrderSounds;
}

public class PedAnimManager : MonoBehaviour
{
    public PedestrianAudioSet[] pedestrianAudioSets; // Holds data for each pedestrian type

    [SerializeField] Animator animator;
    private PedestrianAudioSet currentAudioSet;

    void Start()
    {
        if (pedestrianAudioSets.Length > 0 && animator != null)
        {
            // Choose a random pedestrian type
            int randomIndex = Random.Range(0, pedestrianAudioSets.Length);
            currentAudioSet = pedestrianAudioSets[randomIndex];

            // Assign the animator controller
            animator.runtimeAnimatorController = currentAudioSet.animatorController;
        }
    }

    // Public methods to retrieve sound clips for this pedestrian type
    public string GetRandomOrderCompleteSound()
    {
        return "sfx_complete_"+name;
    }

    public string GetRandomHurtSound()
    {
        return "sfx_hurt_"+name;
    }

    public string GetRandomTakeOrderSound()
    {
        return "sfx_order_"+name;
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            return clips[Random.Range(0, clips.Length)];
        }
        return null; // Return null if no clips are found (Handle this in sound manager)
    }
}