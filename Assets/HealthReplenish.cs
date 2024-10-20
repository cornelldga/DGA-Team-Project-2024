using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReplenish : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Get the PlayerHealth component from the player
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // Replenish the player's health
                player.oil += 20;
                if (player.oil > 100)
                {
                    player.oil = 100;
                }
            }

            // Destroy the health object
            Destroy(gameObject);
        }
    }
}
