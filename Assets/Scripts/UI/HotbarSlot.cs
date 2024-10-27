using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timerLabel;
    [SerializeField] TextMeshProUGUI cookTimerCount;
    [SerializeField] TextMeshProUGUI patienceTimerCount;
    [SerializeField] Image slotBorder;

    // Timers
    private float cookTime = 0;
    private float cookProgress = 0;

    private float patienceTime = 0;
    private float patienceProgress = 0;

    private Customer customer = null;

    public bool isOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        timerLabel.enabled = false;
        cookTimerCount.enabled = false;
        patienceTimerCount.enabled = false;
        Deselect();
    }

    // Update is called once per frame
    void Update()
    {

        if (cookProgress > 0)
        {
            cookProgress -= Time.deltaTime;
            UpdateTimer(cookProgress, cookTimerCount);
            if (cookProgress <= 0)
            {
                FinishCooking();
            }
        }

        if (customer)
        {
            UpdateTimer(customer.waitTime, patienceTimerCount);
        }


    }

    // Adds the order to the hotbar
    public void AddOrder(Customer c)
    {
        customer = c;
        timerLabel.text = "Patience";
        timerLabel.enabled = true;
        cookTime = c.cookTime;
        cookProgress = cookTime;
        cookTimerCount.text = cookProgress.ToString();
        cookTimerCount.enabled = true;
        patienceTimerCount.enabled = true;
        isOpen = false;
        patienceTime = c.waitTime;
        patienceProgress = patienceTime;
    }

    // Called when the order is finished cooking and transitions to patience phase
    public void FinishCooking()
    {
        //cookProgress = cookTime;
        //state = OrderState.Delivering;
        //timerLabel.text = "Patience";
        //timerCount.text = patienceProgress.ToString();
        cookProgress = 0;
    }

    // Updates the value being displayed in the timer
    void UpdateTimer(float time, TextMeshProUGUI label)
    { 
        time += 1;
        float seconds = Mathf.FloorToInt(time % 60);
        label.text = seconds.ToString();
    }


    // Removes the order from the hotbar
    public void RemoveOrder()
    {
        timerLabel.enabled = false;
        cookTimerCount.enabled = false;
        patienceTimerCount.enabled = false;
        isOpen = true;
        customer = null;
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

    public Customer GetCustomerUI()
    {
        return customer;
    }
}
