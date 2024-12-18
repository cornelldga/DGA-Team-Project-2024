using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is responsible for updating the direction indicator UI element based on the player's selected customer
/// </summary>
public class DirectionIndicator : MonoBehaviour
{
    private Player player;
    private Customer selectedCustomer;

    //reference to the hotbar manager, later this should be done through the GameManager, but it's fine for now
    [SerializeField] CookBarManager cookbarManager;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.enabled = false;
        player = GameManager.Instance.getPlayer();
        //if (somehow) there is already a selected customer, set the selectedCustomer to that customer
        if (cookbarManager.selectedSlot != null){
            selectedCustomer = cookbarManager.selectedSlot.GetCustomerUI();
        }
    }

    void Update()
    {
        //set the position of the direction indicator to the player's position
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        //if a customer is selected
        if(cookbarManager.selectedSlot.GetCustomerUI() != null){
            //get the sprite renderer of the direction indicator, and enable it
            spriteRenderer.enabled = true;

            selectedCustomer = cookbarManager.selectedSlot.GetCustomerUI();

            //get the direction from the player to the selected customer
            Vector3 direction = selectedCustomer.transform.position - player.transform.position;

            //set the y value to 0, so the direction indicator is always pointing towards the customer
            direction.y = 0;

            //if the direction is not zero, rotate the direction indicator to face the customer
            if(direction.sqrMagnitude > 0.001f){
                Quaternion rotation = Quaternion.LookRotation(direction);
                //rotate the direction indicator 90 degrees on the x axis, so it's facing the customer
                rotation *= Quaternion.Euler(90, 0, 0);
                //set the rotation of the direction indicator
                transform.rotation = rotation;
            }
        }
        else{
            //if there is no selected customer, disable the direction indicator
            spriteRenderer.enabled = false;
        }
    }
}
