using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script that controls the functioning of the HealthBar 
public class HealthBarController : MonoBehaviour
{
    public Slider healthBar;
    private Player player;

    void Start()
    {
        // Find the player object and get the Player script from the GameManager
        player = GameManager.Instance.getPlayer();



    }

    // Update is called once per frame
    void Update()
    {
        // Update the health bar value based on the player's oil level
        healthBar.value = player.GetOil();
    }
}
