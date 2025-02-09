using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedAnimManager : MonoBehaviour
{
    // Store multiple RuntimeAnimatorControllers (one for each type of pedestrian)
    public RuntimeAnimatorController[] allAnims;

    // Reference to the pedestrian's Animator
    [SerializeField] Animator animator;

    void Start()
    {
        if (allAnims.Length > 0 && animator != null)
        {
            // Choose a random animator from the array
            int randomIndex = Random.Range(0, allAnims.Length);
            animator.runtimeAnimatorController = allAnims[randomIndex];
        }
    }
}