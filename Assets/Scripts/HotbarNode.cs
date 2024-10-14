using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

/// <summary>
/// The HotbarNode class is essentially one hotbar item. This class is allows for easier getting and setting of the hotbar item's properties.
public class HotbarNode : MonoBehaviour
{
    [SerializeField] private GameObject orderTextField;
    [SerializeField] private GameObject customerImage;
    [SerializeField] private GameObject cookTime;
    [SerializeField] private GameObject patienceTime;

    [SerializeField] private GameObject selectedBorder;

    private Boolean isSelected = false;
    // Start is called before the first frame update
    private GameObject customer;
    void Start()
    {
        
    }

    
    /// <summary>
    /// Update is called once per frame. This function updates the selected border based on whether the node is selected or not.
    /// </summary>
    void Update()
    {
        if(isSelected){
            selectedBorder.SetActive(true);
        } else {
            selectedBorder.SetActive(false);
        }
    }

    /// <summary>
    /// Sets the order of the hotbar node.
    /// </summary>
    public void setOrder(String order){
        orderTextField.GetComponent<TMPro.TextMeshProUGUI>().text = order;
    }


    /// <summary>
    /// Sets the customer image of the hotbar node.
    /// </summary>
    public void setCustomerImage(Sprite image){
        customerImage.GetComponent<SpriteRenderer>().sprite = image;
    }


    /// <summary>
    /// Sets the cook time of the hotbar node.
    /// </summary>
    public void setCookTime(int time){
        cookTime.GetComponent<TMPro.TextMeshProUGUI>().text = time.ToString();
    }


    /// <summary>
    /// Sets the patience time of the hotbar node.
    /// </summary>
    public void setPatienceTime(int time){
        patienceTime.GetComponent<TMPro.TextMeshProUGUI>().text = time.ToString();
    }

    /// <summary>
    /// Sets the customer of the hotbar node.
    /// </summary>
    public void setSelected(Boolean selected){
        isSelected = selected;
    }
}
