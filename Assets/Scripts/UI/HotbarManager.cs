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

    public HotbarSlot selectedSlot {get; set;}
    private int selectedIndex = 0;
    private int increment = 1;
    private int numOrders = 0;

    private const int MAX_SIZE = 3;

    // Start is called before the first frame update
    void Start()
    {
        selectedSlot = slots[0];
        //slots[0].isSelected = true;
        //selectedSlot.Select();

    }

    // Update is called once per frame
    void Update()
    {

        // If 'E' is pressed, add order to hotbar if possible

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    //FindNextSlot();
        //    AddToHotbar();
        //}

        //if (selectedIndex < (MAX_SIZE - 1) && Input.GetKeyDown(KeyCode.D)){
        //    ChangeSelection(increment);
        //}

        //if (selectedIndex > 0 && Input.GetKeyDown(KeyCode.A))
        //{
        //    ChangeSelection(-increment);
        //}

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeSelection(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeSelection(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeSelection(2);
        }



    }

    // Adds an order to the hotbar if possible
    public void AddToHotbar(Customer c)
    {
        FindNextSlot();
        // Add to next availble slot
        if (canAddOrder)
        {
            slots[openIndex].AddOrder(c);
            if (numOrders == 0)
            {
                selectedSlot = slots[0];
                slots[0].isSelected = true;
            }
            numOrders += 1;
            if (slots[openIndex].isSelected)
            {
                slots[openIndex].Select();
            }
        }

    }

    public void RemoveFromHotBar(Customer c)
    {
        foreach (HotbarSlot s in slots)
        {
            if (!s.isOpen)
            {
                if (s.GetCustomerUI() == c)
                {
                    s.RemoveOrder();
                    numOrders -= 1;
                }
            }
        }
    }

    // Find the next open slot to put an order into
    void FindNextSlot()
    {
        for (int i = 0; i < MAX_SIZE; i++)
        {
            if (slots[i].isOpen)
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
        if (!slots[amount].isOpen && !slots[amount].isSelected)
        {
            if (!selectedSlot.isOpen)
            {
                selectedSlot.Deselect();
            }
            selectedIndex = amount;
            selectedSlot = slots[selectedIndex];
            selectedSlot.Select();
        }
        

       
    }

}
