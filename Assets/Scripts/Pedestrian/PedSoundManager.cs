using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PedAnimManager))]
public class PedSoundManager : MonoBehaviour
{
    private PedAnimManager pedAnimManager;
    //private AudioSource tempAudioSource; // Temporary AudioSource for playing sounds //Commented out since we need to use the AudioManager
    [SerializeField] AudioClip[] OrderFailedSounds;

    void Start()
    {
        pedAnimManager = GetComponent<PedAnimManager>();

        // Create an AudioSource dynamically if not already attached
        //tempAudioSource = gameObject.AddComponent<AudioSource>();
    }

    //private void PlayClip(AudioClip clip)
    //{
    //    if (clip != null)
    //    {
    //        tempAudioSource.PlayOneShot(clip);
    //    }
    //}

    public void PlayOrderCompleteSound()
    {
        //Debug.Log(pedAnimManager.GetRandomOrderCompleteSound());
        int randomIndex = Random.Range(1, 3);
        AudioManager.Instance.PlaySound(pedAnimManager.GetRandomOrderCompleteSound() + randomIndex);
    }

    public void PlayHurtSound()
    {
        //Debug.Log(pedAnimManager.GetRandomHurtSound());
        int randomIndex = Random.Range(1, 3);
        AudioManager.Instance.PlaySound(pedAnimManager.GetRandomHurtSound() + randomIndex);
    }

    public void PlayTakeOrderSound()
    {
        //Debug.Log(pedAnimManager.GetRandomTakeOrderSound());
        int randomIndex = Random.Range(1, 3);
        AudioManager.Instance.PlaySound(pedAnimManager.GetRandomTakeOrderSound() + randomIndex);
    }

    public void PlayOrderFailedSound()
    {
        AudioManager.Instance.PlaySound("sfx_failorder");
    }
}