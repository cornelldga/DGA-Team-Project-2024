using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class OilMask : MonoBehaviour
{
    private int oilAmount;
    
    private float initialYPosition;
    private float initialXPosition;
    private float initialZPosition;
    [SerializeField] private float finalYPosition;
    [SerializeField] Image lowOilWarning;

    private Player player;
    void Start()
    {

        //get its local position
        initialYPosition = transform.localPosition.y;
        initialXPosition = transform.localPosition.x;
        initialZPosition = transform.localPosition.z;
        Debug.Log("Initial Y Position: " + initialYPosition);
        player = GameManager.Instance.getPlayer();
        lowOilWarning.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        oilAmount = (int)player.GetOil() - (int)player.GetMaxOil();
        transform.localPosition = new Vector3(initialXPosition, initialYPosition + oilAmount, initialZPosition);

        float oilPercent = player.GetOil() / player.GetMaxOil();
        if (oilPercent < 0.5)
        {
            lowOilWarning.enabled = true;
        }
        else
        {
            lowOilWarning.enabled = false;
        }
    }
}
