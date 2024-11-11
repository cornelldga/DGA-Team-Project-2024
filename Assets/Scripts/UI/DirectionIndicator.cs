using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    private Player player;
    private Customer selectedCustomer;
    [SerializeField] private HotbarManager hotbarManager;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.getPlayer();
        if(hotbarManager.selectedSlot != null){
            selectedCustomer = hotbarManager.selectedSlot.GetCustomerUI();
        }
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        if(hotbarManager.selectedSlot.GetCustomerUI() != null){
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.enabled = true;

            selectedCustomer = hotbarManager.selectedSlot.GetCustomerUI();
            Vector3 direction = selectedCustomer.transform.position - player.transform.position;
            direction.y = 0;

            if(direction.sqrMagnitude > 0.001f){
                Quaternion rotation = Quaternion.LookRotation(direction);
                rotation *= Quaternion.Euler(90, 0, 0);
                transform.rotation = rotation;
            }
        }
        else{
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
    }
}
