using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
