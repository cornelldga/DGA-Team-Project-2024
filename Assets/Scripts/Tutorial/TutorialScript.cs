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



    private void Start()
    {
        objectiveLabel.SetActive(false);
        toolTipAnim.Play("TooltipAppearAnim", 0, 0f);
        storedObjMessage = "Hold 'W' to move forward";

        StartCoroutine(ShowTooltipAfterAnimation());
        //GameManager.Instance.FreezeGame();
        

        GameManager.Instance.getPlayer().TurnRight();
        GameManager.Instance.getPlayer().TurnRight();
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
            storedObjMessage = "Press 'E' to deliver an order";
        }
    }

    // Method called when OK button is pressed
    public void OKButtonPressed()
    {
        GameManager.Instance.getPlayer().setCanDrift(false);
        StartCoroutine(HideTooltipAfterAnimation());

        GameManager.Instance.ResumeGame();

        StartCoroutine(ShowObjectiveAnimation());
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

}


