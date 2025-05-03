using System;
using UnityEngine;


[System.Serializable]
public enum CustomerType
{
    Beetle = 0,
    Grasshopper = 1,
    Ladybug = 2,
    Rolypoly = 3
}

public class CustomerTypes : MonoBehaviour
{
    [SerializeField] Customer customer;

    [SerializeField] Animator animator;
    [SerializeField] RuntimeAnimatorController[] animatorControllers;

    void Start()
    {
        switch (customer.customerType)
        {
            case CustomerType.Beetle:
                animator.runtimeAnimatorController = animatorControllers[0];
                break;
            case CustomerType.Grasshopper:
                animator.runtimeAnimatorController = animatorControllers[1];
                break;
            case CustomerType.Ladybug:
                animator.runtimeAnimatorController = animatorControllers[2];
                break;
            case CustomerType.Rolypoly:
                animator.runtimeAnimatorController = animatorControllers[3];
                break;
        }
    }

    // Public methods to retrieve sound clips for this pedestrian type
    public string GetRandomOrderCompleteSound()
    {
        return "sfx_complete_" + nameof(customer.customerType) + UnityEngine.Random.Range(1,3);
    }

    public string GetRandomHurtSound()
    {
        return "sfx_hurt_" + nameof(customer.customerType) + UnityEngine.Random.Range(1, 3);
    }

    public string GetRandomTakeOrderSound()
    {
        return "sfx_order_" + nameof(customer.customerType) + UnityEngine.Random.Range(1, 3);
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            return clips[UnityEngine.Random.Range(0, clips.Length)];
        }
        return null; // Return null if no clips are found (Handle this in sound manager)
    }
}