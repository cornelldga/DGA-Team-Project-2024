using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    // List of images to be used as timer placeholders
    [SerializeField] Sprite[] timerImages = new Sprite[2];
    [SerializeField] Sprite emptySlotSprite;

    [SerializeField] HotbarSlot[] slots = new HotbarSlot[3];  // Hotbar slots
    private int openIndex = 0;  // The index of the next available slot
    private bool canAddOrder = true;

    private HotbarSlot selectedSlot;
    private int selectedIndex = 0;
    private int increment = 1;

    private const int MAX_SIZE = 3;

    // Start is called before the first frame update
    void Start()
    {
        selectedSlot = slots[0];
        selectedSlot.Select();

    }

    // Update is called once per frame
    void Update()
    {

        // If 'E' is pressed, add order to hotbar if possible
    
        if (Input.GetKeyDown(KeyCode.E))
        {
            //FindNextSlot();
            AddToHotbar();
        }

        if (selectedIndex < (MAX_SIZE - 1) && Input.GetKeyDown(KeyCode.RightArrow)){
            ChangeSelection(increment);
        }

        if (selectedIndex > 0 && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeSelection(-increment);
        }

    }

    // Adds an order to the hotbar if possible
    void AddToHotbar()
    {
        FindNextSlot();
        // Add to next availble slot
        if (canAddOrder)
        {
            slots[openIndex].AddOrder();
        }

    }

    // Find the next open slot to put an order into
    void FindNextSlot()
    {
        for (int i = 0; i < MAX_SIZE; i++)
        {
            if (slots[i].GetState() == HotbarSlot.OrderState.Empty)
            {
                openIndex = i;
                canAddOrder = true;
                return;
            }
        }
        canAddOrder = false;
    }

    void ChangeSelection(int amount)
    {
        selectedSlot.Deselect();
        selectedIndex += amount;
        selectedSlot = slots[selectedIndex];
        selectedSlot.Select();
    }
}
