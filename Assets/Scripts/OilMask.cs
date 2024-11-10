using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OilMask : MonoBehaviour
{
    private int oilAmount;
    
    private float initialYPosition;
    private float initialXPosition;
    private float initialZPosition;
    [SerializeField] private float finalYPosition;
    void Start()
    {

        //get its local position
        initialYPosition = transform.localPosition.y;
        initialXPosition = transform.localPosition.x;
        initialZPosition = transform.localPosition.z;
        Debug.Log("Initial Y Position: " + initialYPosition);
    }

    // Update is called once per frame
    void Update()
    {
        oilAmount = (int)GameManager.Instance.getPlayer().GetOil() - (int)GameManager.Instance.getPlayer().GetMaxOil();
        transform.localPosition = new Vector3(initialXPosition, initialYPosition + oilAmount, initialZPosition);
    }
}
