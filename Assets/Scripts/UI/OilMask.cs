using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// This script is responsible for updating the oil mask UI element based on the player's oil level
/// </summary>
public class OilMask : MonoBehaviour
{
    private int oilAmount;
    
    private float initialYPosition;
    private float initialXPosition;
    private float initialZPosition;

    //reference to the low oil warning image
    [SerializeField] Image lowOilWarning;
    [SerializeField] Image oilImage;
    [Tooltip("The percentage the oil must be at or lower to show the low warning image")]
    [SerializeField] float warningPercentage = .5f;

    private Player player;
    void Start()
    {
        player = GameManager.Instance.getPlayer();
        lowOilWarning.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //the way this script works is essentially, we are decreasing the y position of the mask to slowly make it appear as if the oil is decreasing.
        //we are putting the mask on top of the oil bar, so it looks like the oil is decreasing.

        //get the difference between the player's oil and the player's max oil (use this to determine how much the mask should move)
        oilImage.fillAmount = player.GetOil() / player.GetMaxOil();

        if(oilImage.fillAmount < warningPercentage)
        {
            lowOilWarning.enabled = true;
        }
        else
        {
            lowOilWarning.enabled = false;
        }
        
    }
}
