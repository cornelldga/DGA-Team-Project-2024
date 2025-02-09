using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PedAnimManager))]
public class PedSoundManager : MonoBehaviour
{
    private PedAnimManager pedAnimManager;
    private AudioSource tempAudioSource; // Temporary AudioSource for playing sounds
    [SerializeField] AudioClip[] OrderFailedSounds;

    void Start()
    {
        pedAnimManager = GetComponent<PedAnimManager>();

        // Create an AudioSource dynamically if not already attached
        tempAudioSource = gameObject.AddComponent<AudioSource>();
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip != null)
        {
            tempAudioSource.PlayOneShot(clip);
        }
    }

    public void PlayOrderCompleteSound()
    {
        PlayClip(pedAnimManager.GetRandomOrderCompleteSound());
    }

    public void PlayHurtSound()
    {
        PlayClip(pedAnimManager.GetRandomHurtSound());
    }

    public void PlayTakeOrderSound()
    {
        PlayClip(pedAnimManager.GetRandomTakeOrderSound());
    }

    public void PlayOrderFailedSound()
    {
        if (OrderFailedSounds.Length > 0)
        {
            PlayClip(OrderFailedSounds[Random.Range(0, OrderFailedSounds.Length)]);
        }
    }
}