using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PedAnimManager))]
public class PedSoundManager : MonoBehaviour
{
    private PedAnimManager pedAnimManager;
    private const float soundCooldown = 5f;
    private float prevSoundTime = 0f;

    void Start()
    {
        pedAnimManager = GetComponent<PedAnimManager>();
    }


    public void PlayOrderCompleteSound(Vector3 position)
    {
        if (Time.time - prevSoundTime >= soundCooldown)
        {
            //Debug.Log(pedAnimManager.GetRandomOrderCompleteSound());
            int randomIndex = Random.Range(1, 3);
            string sfx_id = pedAnimManager.GetRandomOrderCompleteSound() + randomIndex;
            AudioManager.Instance.PlaySoundAtPoint(sfx_id, position);
            prevSoundTime = Time.time;
        }
    }

    public void PlayHurtSound(Vector3 position)
    {
        if (Time.time - prevSoundTime >= soundCooldown)
        {
            //Debug.Log(pedAnimManager.GetRandomHurtSound());
            int randomIndex = Random.Range(1, 3);
            string sfx_id = pedAnimManager.GetRandomHurtSound() + randomIndex;
            AudioManager.Instance.PlaySoundAtPoint(sfx_id, position);
            prevSoundTime = Time.time;
        }
    }

    public void PlayTakeOrderSound(Vector3 position)
    {
        if (Time.time - prevSoundTime >= soundCooldown) { 
            //Debug.Log(pedAnimManager.GetRandomTakeOrderSound());
            int randomIndex = Random.Range(1, 3);
            string sfx_id = pedAnimManager.GetRandomTakeOrderSound() + randomIndex;
            AudioManager.Instance.PlaySoundAtPoint(sfx_id, position);
            prevSoundTime = Time.time;
        }
    }

    public void PlayOrderFailedSound(Vector3 position)
    {
        AudioManager.Instance.PlaySoundAtPoint("sfx_failorder", position);
    }
}