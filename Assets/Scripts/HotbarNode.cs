using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The HotbarNode class is essentially one hotbar item. This class is allows for easier getting and setting of the hotbar item's properties.
/// </summary>
public class HotbarNode : MonoBehaviour
{
    [SerializeField] private GameObject orderTextField;
    [SerializeField] private GameObject customerImage;
    [SerializeField] private GameObject cookTime;
    [SerializeField] private GameObject patienceTime;

    [SerializeField] private GameObject selectedBorder;

    // Start is called before the first frame update
    void Start()
    {

    }


    /// <summary>
    /// Sets the order of the hotbar node.
    /// </summary>
    public void setOrder(String order)
    {
        orderTextField.GetComponent<TMPro.TextMeshProUGUI>().text = order;
    }


    /// <summary>
    /// Sets the customer image of the hotbar node.
    /// </summary>
    public void setCustomerImage(Sprite image)
    {
        customerImage.GetComponent<UnityEngine.UI.Image>().sprite = image;
    }


    /// <summary>
    /// Sets the cook time of the hotbar node.
    /// </summary>
    public void setCookTime(float time)
    {
        cookTime.GetComponent<TMPro.TextMeshProUGUI>().text = time.ToString();
    }


    /// <summary>
    /// Sets the patience time of the hotbar node.
    /// </summary>
    public void setPatienceTime(float time)
    {
        patienceTime.GetComponent<TMPro.TextMeshProUGUI>().text = time.ToString();
    }

    public GameObject getSelectedBorder()
    {
        return selectedBorder;
    }
}
