using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private GameObject cop;
    [SerializeField] GameObject tooltip;
    [SerializeField] TMP_Text tooltipMessage;
    // the number of seconds a message remains on screen before disappearing.
    [SerializeField] private float messageDuration = 10;
    private float messageTimer = 0;
    private bool activeCop = false;

    // Update is called once per frame
    void Update()
    {
        // only show tooltip message for a set amount of time
        if (messageTimer > 0)
        {
            Debug.Log(messageTimer);
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0)
            {
                tooltip.SetActive(false);
            }
        }


        // spawn cop after first order is taken. 
        if (!activeCop && GameManager.Instance.GetCustomers().Count > 0)
        {
            cop.SetActive(true);
            activeCop = true;
            ShowMessage("They found us! We can't vend food in this city, but that won't stop us. Avoid the cops until the order is ready!");
        }
    }

    // Pause game, show given tutorial message on the screen
    public void ShowMessage(string message)
    {

        //GameManager.Instance.PauseGame();
        tooltipMessage.text = message;
        tooltip.SetActive(true);

        messageTimer = messageDuration;

    }
}
