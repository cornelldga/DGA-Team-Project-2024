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
    [SerializeField] Button OKButton;

    [SerializeField] GameObject objectiveLabel;
    [SerializeField] TMP_Text objectiveMessage;

    [SerializeField] private Animator toolTipAnim;
    [SerializeField] private Animator objectiveAnim;

    private bool activeCop = false;
    private bool startTutorial = false;
    private string storedObjMessage;

    // Whether another tooltip should be shown after pressing 'ok'
    bool displayAnotherTooltip = false;
    List<string> extraTips = new List<string>();
    int tipIndex = 0;

    float prevOil;





    private void Start()
    {
        objectiveLabel.SetActive(false);
        toolTipAnim.Play("TooltipAppearAnim", 0, 0f);
        storedObjMessage = "Hold 'W' to move forward";

        StartCoroutine(ShowTooltipAfterAnimation());
        //GameManager.Instance.FreezeGame();
        

        GameManager.Instance.getPlayer().TurnRight();
        GameManager.Instance.getPlayer().TurnRight();
        prevOil = GameManager.Instance.getPlayer().GetMaxOil();

        // Allocate extra tool tip messages
        // Tips for learning how to boost (Index 0)
        extraTips.Add("While we get that order cooking, I'll show you how to give the cops a good 'ol run around.");
        extraTips.Add("Hold SHIFT to use fuel and boost forward!");

        // Tips for refueling (Index 2)
        extraTips.Add("You'll need fuel for cooking and speeding through the city, " +
            "so make sure to stay stocked up! Drive into any oil containers to fuel up!");

        // Tips for interacting with the second customer and radar (Index 3)
        extraTips.Add("Now that you've got a handle of the 'ol cart, let's pick up " +
            "another hungry customer's order.");
        extraTips.Add("Use the radar to locate the next waiting customer. You can " +
            "also use it to find oil canisters!");

        // Tips for patience and hotbar (Index 5)
        extraTips.Add("Each customer has a patience meter. Don't let it get " +
            "too low or they'll find their food elsewhere!");
        extraTips.Add("Use num keys '1', '2', or '3' to hone in " +
            "on a specific customer's location.");

        // Final farewell (Index 7)
        extraTips.Add("I've taught you all I know. Go sling some grub " +
            "to those hungry bugs!");
    }

    // Update is called once per frame
    void Update()
    {

        // spawn cop after first order is taken. 
        if (!activeCop && GameManager.Instance.GetCustomers().Count > 0)
        {
            cop.SetActive(true);
            activeCop = true;
            ShowMessage("They found us! We can't vend food in this city, but that won't stop us. Avoid the cops until the order is ready!");
            displayAnotherTooltip = true;
            storedObjMessage = "Hold SHIFT to boost";
            //storedObjMessage = "Press 'E' to deliver an order";
        }

        // Check if player has refueled after they've been told to
        if (tipIndex == 3) {
            float currOil = GameManager.Instance.getPlayer().GetOil();

            if (currOil > prevOil)
            {
                // Trigger completion of refuel goal
                ShowMessage(extraTips[tipIndex]);
                setObjectiveMessage("Find the next customer");
                displayAnotherTooltip = true;
            }

            prevOil = currOil;
        }

        
    }

    // Method called when OK button is pressed
    public void OKButtonPressed()
    {
        GameManager.Instance.getPlayer().setCanDrift(false);

        if (displayAnotherTooltip)
        {
            tooltipMessage.text = extraTips[tipIndex];
            tipIndex++;

            // Finish tips for teaching boost
            if (tipIndex == 2)
            {
                displayAnotherTooltip = false;
                // Start countdown for showing refuel tip
                StartCoroutine(WaitNextTip());
            }

            //Finish tips for teaching radar
            if (tipIndex == 5)
            {

            }
        }

        else
        {
            StartCoroutine(HideTooltipAfterAnimation());

            GameManager.Instance.ResumeGame();

            StartCoroutine(ShowObjectiveAnimation());
        }

        
    }

    // Pause game, show given tutorial message on the screen
    public void ShowMessage(string message)
    {
        //objectiveLabel.SetActive(false);

        tooltipMessage.text = message;

        GameManager.Instance.getPlayer().setCanDrift(false);
        StartCoroutine(ShowTooltipAfterAnimation());
        StartCoroutine(HideObjectiveAnimation());


        //messageTimer = messageDuration;

    }

    public void setObjectiveMessage(string message)
    {
        //objectiveMessage.text = message;
        storedObjMessage = message;
    }


    // Coroutine to hide the tooltip after the animation plays
    private IEnumerator HideTooltipAfterAnimation()
    {
        toolTipAnim.Play("TooltipDisappearAnim", 0, 0f);
        toolTipAnim.enabled = true;

        
        // Wait for the animation to finish
        yield return new WaitForSeconds(1.28f);

        // Hide the tooltip after animation completes
        toolTipAnim.enabled = false;
        tooltip.SetActive(false);
    }

    // Coroutine to show the tooltip after the animation plays
    private IEnumerator ShowTooltipAfterAnimation()
    {
        tooltip.SetActive(true);
        toolTipAnim.Play("TooltipAppearAnim", 0, 0f);

        //tooltip.SetActive(true);
        toolTipAnim.enabled = true;


        // Wait for the animation to finish
        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.getPlayer().setCanDrift(true);

        toolTipAnim.enabled = false;

        if (!startTutorial)
        {
            GameManager.Instance.FreezeGame();
            startTutorial = true;
        }
    }

    // Coroutine to hide the objective after the animation plays
    private IEnumerator HideObjectiveAnimation()
    {
        objectiveAnim.enabled = true;
        objectiveAnim.Play("ObjectiveDisappear", 0, 0f);
        // Wait for the animation to finish
        yield return new WaitForSeconds(1.0f);

        // Hide the objective after animation completes
        objectiveLabel.SetActive(false);
        GameManager.Instance.FreezeGame();
    }

    // Coroutine to show the objective after the animation plays
    private IEnumerator ShowObjectiveAnimation()
    {
        objectiveMessage.text = storedObjMessage;
        objectiveAnim.Play("ObjectiveAppear", 0, 0f);
        objectiveLabel.SetActive(true);
        objectiveAnim.enabled = true;

        // Wait for the animation to finish
        yield return new WaitForSeconds(1.0f);

        GameManager.Instance.getPlayer().setCanDrift(true);

        // Show the objective after animation completes
        objectiveAnim.enabled = false;
    }


    // Coroutine for waiting between tool tips
    private IEnumerator WaitNextTip()
    {
        // Wait to give next tip
        yield return new WaitForSeconds(5.0f);

        // Display the next tip
        showNextMessage();
    }


    private void showNextMessage()
    {

        ShowMessage(extraTips[tipIndex]);

        // Update objective for refuel
        if (tipIndex == 2)
        {
            setObjectiveMessage("Collect oil to refuel");
            tipIndex++;
        }

        // Update objective for finding next customer

    }

}


