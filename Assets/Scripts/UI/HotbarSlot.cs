using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timerLabel;
    //[SerializeField] TextMeshProUGUI cookTimerCount;
    [SerializeField] TextMeshProUGUI patienceTimerCount;
    [SerializeField] TextMeshProUGUI orderInfo;
    [SerializeField] Image slotBorder;
    [SerializeField] Image slotIcon;
    [SerializeField] Image shadow;
    [SerializeField] Image progress;
    [SerializeField] Animator cookDoneAnim;

    [SerializeField] string orderNum;

    // Timers
    private float maxCookTime = 0;
    private float cookProgress = 0;

    private float patienceTime = 0;
    private float patienceProgress = 0;

    private Color readyColor = new Color(184, 233, 173);
    private Color normColor;

    private Customer customer = null;

    public bool isOpen = true;
    public bool isSelected;
    private bool isReady = false;
    private int moveX = 50;

    private int sMoveX = -3;
    private int sMoveY = -20;

    // Start is called before the first frame update
    void Start()
    {
        timerLabel.enabled = false;
        //cookTimerCount.enabled = false;
        patienceTimerCount.enabled = false;
        slotBorder.enabled = false;
        normColor = slotIcon.color;
        progress.fillAmount = 0;
        cookDoneAnim.enabled = false;
        //Deselect();
    }

    // Update is called once per frame
    void Update()
    {
        //if (cookProgress > 0)
        //{
        //    cookProgress -= Time.deltaTime;
        //    UpdateTimer(cookProgress, cookTimerCount);
        //    if (cookProgress <= 0)
        //    {
        //        FinishCooking();
        //    }
        //}

        if (customer)
        {
            UpdateTimer(customer.waitTime, patienceTimerCount);
            //UpdateTimer(customer.cookTime, cookTimerCount);

            if (customer.cookTime < 0 && !isReady)
            {
                isReady = true;
                slotIcon.color = readyColor;
                cookDoneAnim.enabled = true;
            }

            //Debug.Log("Cook Progress: " + customer.cookTime);
            // Update cook fill bar
            progress.fillAmount = 1 - (customer.cookTime / maxCookTime);
        }


    }

    // Adds the order to the hotbar
    public void AddOrder(Customer c)
    {
        customer = c;
        //timerLabel.text = "Patience";
        orderInfo.text = orderNum;
        timerLabel.enabled = true;
        maxCookTime = c.cookTime;
        cookProgress = maxCookTime;
        //cookTimerCount.text = cookProgress.ToString();
        //cookTimerCount.enabled = true;
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
        cookDoneAnim.enabled = false;
        //cookTimerCount.enabled = false;
        patienceTimerCount.enabled = false;
        isOpen = true;
        customer = null;
        orderInfo.text = "";
        isReady = false;
        slotIcon.color = normColor;
        progress.fillAmount = 0;
        if (isSelected)
        {
            Deselect();
        }
        
    }

    // Called when the order fails
    void FailOrder()
    {
        RemoveOrder();
    }

    // Selects this slot as the player's active order slot to track
    public void Select()
    {
        
        slotBorder.enabled = true;
        isSelected = true;
        

        // Move the icon
        RectTransform rectTransform = slotIcon.rectTransform;
            
        // Get current position
        Vector3 currentPosition = rectTransform.anchoredPosition;
        // Add 10 units to x position
        Vector3 newPosition = new Vector3(currentPosition.x + moveX, currentPosition.y, currentPosition.z);
        // Set the new position
        rectTransform.anchoredPosition = newPosition;

        // Move the outline
        RectTransform rectTransform2 = slotBorder.rectTransform;

        // Get current position
        Vector3 currentPosition2 = rectTransform2.anchoredPosition;
        // Add 10 units to x position
        Vector3 newPosition2 = new Vector3(currentPosition2.x + moveX, currentPosition2.y, currentPosition2.z);
        // Set the new position
        rectTransform2.anchoredPosition = newPosition2;

        // Move the shadow
        RectTransform rectTransform3 = shadow.rectTransform;
        // Get current position
        Vector3 currentPosition3 = rectTransform3.anchoredPosition;
        // Add 10 units to x position
        Vector3 newPosition3 = new Vector3(currentPosition3.x + sMoveX, currentPosition3.y + sMoveY, currentPosition3.z);
        // Set the new position
        rectTransform3.anchoredPosition = newPosition3;



    }

    // Deselects this slot as the player's active order slot to track
    public void Deselect()
    {
        
        slotBorder.enabled = false;
        isSelected = false;
        

        RectTransform rectTransform = slotIcon.rectTransform;

        // Get current position
        Vector3 currentPosition = rectTransform.anchoredPosition;
        // Sub 10 units to x position
        Vector3 newPosition = new Vector3(currentPosition.x - moveX, currentPosition.y, currentPosition.z);
        // Set the new position
        rectTransform.anchoredPosition = newPosition;

        // Move the outline
        RectTransform rectTransform2 = slotBorder.rectTransform;
        // Get current position
        Vector3 currentPosition2 = rectTransform2.anchoredPosition;
        // Add 10 units to x position
        Vector3 newPosition2 = new Vector3(currentPosition2.x - moveX, currentPosition2.y, currentPosition2.z);
        // Set the new position
        rectTransform2.anchoredPosition = newPosition2;

        // Move the shadow
        RectTransform rectTransform3 = shadow.rectTransform;
        // Get current position
        Vector3 currentPosition3 = rectTransform3.anchoredPosition;
        // Add 10 units to x position
        Vector3 newPosition3 = new Vector3(currentPosition3.x - sMoveX, currentPosition3.y - sMoveY, currentPosition3.z);
        // Set the new position
        rectTransform3.anchoredPosition = newPosition3;

    }

    public Customer GetCustomerUI()
    {
        return customer;
    }
}
