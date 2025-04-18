using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class manages the UI for how the player keeps track of orders currently picked up.
/// </summary>
public class CookBarManager : MonoBehaviour
{
    // List of images to be used as timer placeholders
    //[SerializeField] Sprite[] timerImages = new Sprite[2];

    [Header("Hotbar Inputs")]
    [SerializeField] Sprite emptySlotSprite;
    [SerializeField] CookSlot[] slots = new CookSlot[3];  // Hotbar slots


    private int openIndex = 0;  // The index of the next available slot
    private bool canAddOrder = true;

    public CookSlot selectedSlot { get; set; }
    private int selectedIndex = 0;
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

    /// <summary>
    /// Adds an order to the hotbar if possible
    /// </summary>
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

    /// <summary>
    /// Removes an order from the hotbar based on customer reference.
    /// </summary>
    public void RemoveFromHotBar(Customer c)
    {
        foreach (CookSlot s in slots)
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

    /// <summary>
    /// Finds the next open slot to put an order into.
    /// </summary>
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

    /// <summary>
    /// Changes the selection of which slot of player is actively tracking.
    /// Will only successfully select a slot with an order associated to it.
    /// </summary>
    /// <param name="amount"></param>
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
