using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReplenish : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with the player
        if (other.CompareTag("Player"))
        {
            // Get the PlayerHealth component from the player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Replenish the player's health
                playerHealth.ReplenishHealth();
            }

            // Destroy the health object
            Destroy(gameObject);
        }
    }
}
