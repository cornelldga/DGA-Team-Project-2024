using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public enum OrderState
    {
        Empty,
        Cooking,
        Delivering
    }

    [SerializeField] TextMeshProUGUI timerLabel;
    [SerializeField] TextMeshProUGUI timerCount;
    [SerializeField] Image slotBorder;

    // Timers
    private int cookTime = 5;
    private float cookProgress = 10;

    private int patienceTime = 7;
    private float patienceProgress = 15;

    private Customer2 order;

    public bool isOpen = true;

    private OrderState state = OrderState.Empty;

    // Start is called before the first frame update
    void Start()
    {
        timerLabel.enabled = false;
        timerCount.enabled = false;
        Deselect();
    }

    // Update is called once per frame
    void Update()
    {
        // Update timers based on what state the order is in
        switch (state)
        {
            case OrderState.Cooking:
                if (cookProgress > 0)
                {
                    cookProgress -= Time.deltaTime;
                    UpdateTimer(cookProgress);
                }
                else
                {
                    FinishCooking();
                }
          
                break;
            case OrderState.Delivering:
                if (patienceProgress > 0)
                {
                    patienceProgress -= Time.deltaTime;
                    UpdateTimer(patienceProgress);
                }
                else
                {
                    FailOrder();
                }
                break;
        }
        
    }

    // Adds the order to the hotbar
    public void AddOrder()
    {
        timerLabel.text = "Cook";
        timerLabel.enabled = true;
        timerCount.text = "10";
        timerCount.enabled = true;
        //isOpen = false;
        state = OrderState.Cooking;
        patienceProgress = patienceTime;
        cookProgress = cookTime;

    }

    // Called when the order is finished cooking and transitions to patience phase
    public void FinishCooking()
    {
        cookProgress = cookTime;
        state = OrderState.Delivering;
        timerLabel.text = "Patience";
        timerCount.text = "15";
    }

    // Updates the value being displayed in the timer
    void UpdateTimer(float time)
    { 
        time += 1;
        float seconds = Mathf.FloorToInt(time % 60);
        timerCount.text = seconds.ToString();
    }

    // Returns the state of the order
    public OrderState GetState()
    {
        return state;
    }

    // Removes the order from the hotbar
    void RemoveOrder()
    {
        timerLabel.enabled = false;
        timerCount.enabled = false;
        state = OrderState.Empty;
    }

    // Called when the order fails
    void FailOrder()
    {
        RemoveOrder();
    }

    // Selects this slot as the player's active order slot to track
    public void Select()
    {
        slotBorder.color = Color.green;
    }

    // Deselects this slot as the player's active order slot to track
    public void Deselect()
    {
        slotBorder.color = Color.gray;
    }
}
