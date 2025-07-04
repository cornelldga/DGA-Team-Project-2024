using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] GameObject tooltip;
    [SerializeField] TMP_Text tooltipMessage;
    [SerializeField] Button OKButton;

    [SerializeField] GameObject objectiveLabel;
    [SerializeField] TMP_Text objectiveMessage;

    [SerializeField] private Animator toolTipAnim;

    // cooking tutorial script message order
    private bool playedCookingTutorial1 = false;
    private bool playedCookingTutorial2 = false;
    private bool playedCookingTutorial3 = false;
    private bool playedCookingTutorial4 = false;

    [Header("Cooking Tutorial Portion")]
    [Tooltip("the customer that is visited first.")]
    [SerializeField] private Customer customer1;
    [Tooltip("spawn in this customer after customer 1 is served. ")]
    [SerializeField] private GameObject customer2;
    [Tooltip("spawn in cop after second order is taken")]
    [SerializeField] private GameObject cop;

    [Header("Game Instance items to disable in Tutorial")]
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject radar;


    private void Start()
    {
        objectiveLabel.SetActive(false);
        cop.SetActive(false);
        customer2.SetActive(false);

        timer.SetActive(false);
        radar.SetActive(false);



        GameManager.Instance.FreezeGame();
    }

    // Update is called once per frame
    void Update()
    {
        cookingScript();
    }

    void cookingScript()
    {
        
        if (!playedCookingTutorial1 && GameManager.Instance.GetCustomers().Count > 0)
        {
            playedCookingTutorial1 = true;
            ShowMessage("Now cook their order! Left click the food on the stove until its perfect!");
            setObjectiveMessage("Left click food to cook");
        }

        if (!playedCookingTutorial2 && customer1.IsFoodReady())
        {
            playedCookingTutorial2 = true;
            ShowMessage("Looks great! The order is complete. Press 'E' again to fulfill the order.");
            setObjectiveMessage("Press 'E' to serve customer");
        }

        if (!playedCookingTutorial3 && customer1.IsOrderCompleted())
        {
            playedCookingTutorial3 = true;
            customer2.SetActive(true);

            ShowMessage("There are more hungry bugs. Lets find our next customer!");
            setObjectiveMessage("Find and serve another costumer");
            
        }

        // spawn cop after second order is taken.
        if (playedCookingTutorial3 && !playedCookingTutorial4 && GameManager.Instance.GetCustomers().Count > 0)
        {
            playedCookingTutorial4 = true;
            cop.SetActive(true);

            ShowMessage("The cops! They found us! Keep cooking but step on the gas and speed away with 'SHIFT'!");
            setObjectiveMessage("Finish the order while avoiding cops!");

        }

    }


    // Method called when OK button is pressed
    public void OKButtonPressed()
    {
        tooltip.SetActive(false);
        GameManager.Instance.ResumeGame();

        objectiveLabel.SetActive(true);
    }

    // Pause game, show given tutorial message on the screen
    public void ShowMessage(string message)
    {
        objectiveLabel.SetActive(false);

        GameManager.Instance.FreezeGame();
        tooltipMessage.text = message;
        tooltip.SetActive(true);
        //toolTipAnim.;
        
        //messageTimer = messageDuration;

    }

    public void setObjectiveMessage(string message)
    {
        objectiveMessage.text = message;
    }
}
