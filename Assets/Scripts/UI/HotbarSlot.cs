using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


/// <summary>
/// This class is used to define the individual Hotbar nodes. Each node is
/// associated to a customer's order picked up by the player.
/// </summary>
public class HotbarSlot : MonoBehaviour
{
    [Header("Hotbar Slot Inputs")]
    [SerializeField] TextMeshProUGUI timerLabel;
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

        if (customer)
        {
            UpdateTimer(customer.waitTime, patienceTimerCount);

            if (customer.cookTime < 0 && !isReady)
            {
                isReady = true;
                slotIcon.color = readyColor;
                cookDoneAnim.enabled = true;
            }

            // Update cook fill bar
            progress.fillAmount = 1 - (customer.cookTime / maxCookTime);
        }


    }

    /// <summary>
    /// Adds the order to the hotbar.
    /// </summary>
    /// <param name="c"></param>
    public void AddOrder(Customer c)
    {
        customer = c;
        orderInfo.text = orderNum;
        timerLabel.enabled = true;
        maxCookTime = c.cookTime;
        cookProgress = maxCookTime;
        patienceTimerCount.enabled = true;
        isOpen = false;
        patienceTime = c.waitTime;
        patienceProgress = patienceTime;
    }

    /// <summary>
    /// Updates the value being displayed in the timer.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="label"></param>
    void UpdateTimer(float time, TextMeshProUGUI label)
    {
        time += 1;
        float seconds = Mathf.FloorToInt(time % 60);
        label.text = seconds.ToString();
    }

    /// <summary>
    /// Removes the order from the hotbar.
    /// </summary>
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

    /// <summary>
    /// Called when the order fails.
    /// </summary>

    void FailOrder()
    {
        RemoveOrder();
    }

    /// <summary>
    ///  Selects this slot as the player's active order slot to track.
    /// </summary>
    public void Select()
    {
        
        slotBorder.enabled = true;
        isSelected = true;
        

        // Move the icon
        RectTransform rectTransform = slotIcon.rectTransform;
        Vector3 currentPosition = rectTransform.anchoredPosition;
        Vector3 newPosition = new Vector3(currentPosition.x + moveX, currentPosition.y, currentPosition.z);
        rectTransform.anchoredPosition = newPosition;

        // Move the outline
        RectTransform rectTransform2 = slotBorder.rectTransform;
        Vector3 currentPosition2 = rectTransform2.anchoredPosition;
        Vector3 newPosition2 = new Vector3(currentPosition2.x + moveX, currentPosition2.y, currentPosition2.z);
        rectTransform2.anchoredPosition = newPosition2;

        // Move the shadow
        RectTransform rectTransform3 = shadow.rectTransform;
        Vector3 currentPosition3 = rectTransform3.anchoredPosition;
        Vector3 newPosition3 = new Vector3(currentPosition3.x + sMoveX, currentPosition3.y + sMoveY, currentPosition3.z);
        rectTransform3.anchoredPosition = newPosition3;



    }

    /// <summary>
    /// Deselects this slot as the player's active order slot to track.
    /// </summary>
    public void Deselect()
    {
        
        slotBorder.enabled = false;
        isSelected = false;
        

        RectTransform rectTransform = slotIcon.rectTransform;
        Vector3 currentPosition = rectTransform.anchoredPosition;
        Vector3 newPosition = new Vector3(currentPosition.x - moveX, currentPosition.y, currentPosition.z);
        rectTransform.anchoredPosition = newPosition;

        // Move the outline
        RectTransform rectTransform2 = slotBorder.rectTransform;
        Vector3 currentPosition2 = rectTransform2.anchoredPosition;
        Vector3 newPosition2 = new Vector3(currentPosition2.x - moveX, currentPosition2.y, currentPosition2.z);
        rectTransform2.anchoredPosition = newPosition2;

        // Move the shadow
        RectTransform rectTransform3 = shadow.rectTransform;
        Vector3 currentPosition3 = rectTransform3.anchoredPosition;
        Vector3 newPosition3 = new Vector3(currentPosition3.x - sMoveX, currentPosition3.y - sMoveY, currentPosition3.z);
        rectTransform3.anchoredPosition = newPosition3;

    }

    /// <summary>
    /// Returns the customer associated with this slot.
    /// </summary>
    /// <returns></returns>
    public Customer GetCustomerUI()
    {
        return customer;
    }
}
