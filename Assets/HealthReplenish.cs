using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReplenish : MonoBehaviour
{
    // Player's health
    [SerializeField] int health = 100;
    // Amount of health to replenish on collision
    [SerializeField] int healthReplenishAmount = 20;

    void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with a specific object or tag
        if (other.gameObject.CompareTag("Player"))
        {
            // Replenish health
            health += healthReplenishAmount;
            Debug.Log("Oil replenished!");

            // Destroy the health object
            Destroy(gameObject);
        }
    }
}
