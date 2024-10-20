using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basic Health Script (used for oil) this is most likely a temporary script for testing 
public class PlayerHealth : MonoBehaviour
{
    // Player's health
    [SerializeField] int health = 100;
    // Amount of health to replenish on collision
    [SerializeField] int healthReplenishAmount = 20;

    public void ReplenishHealth()
    {
        health += healthReplenishAmount;
        Debug.Log("Oil replenished! Current Oil Amount is: " + health);
    }
}
