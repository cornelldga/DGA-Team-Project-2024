using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Unity.Properties;

/// <summary>
/// This class is the representation of a node in the hotbar
/// This includes the patience, face color, order, and time to make the order
/// It's an actual object that will be displayed on the screen
/// </summary>
public class HotbarNode
{
    private int maxPatience{get; set;}
    private Color face{get; set;}
    private string order{get; set;}
    private int timeToMake{get; set;}

    private GameObject node;
    private GameObject[] children;
    

    /// <summary>
    /// Constructor for HotbarNode
    /// </summary>
    /// <param name="maxPatience">The maximum patience of the customer</param>
    /// <param name="face">The color of the customer</param>
    /// <param name="order">The order of the customer</param>
    /// <param name="timeToMake">The time to make the order</param>
    public HotbarNode(int maxPatience, Color face, string order, int timeToMake, GameObject hotbarNodePrefab, GameObject hotbarPanel)
    {
        this.maxPatience = maxPatience;
        this.face = face;
        this.order = order;
        this.timeToMake = timeToMake;

        this.node = GameObject.Instantiate(hotbarNodePrefab);
        this.node.transform.SetParent(hotbarPanel.transform);
        this.node.SetActive(true);
        this.node.GetComponent<Image>().color = face;
        this.node.GetComponentInChildren<TextMeshProUGUI>().text = order;

        this.children = new GameObject[3];
        this.children[0] = this.node.transform.GetChild(0).gameObject;
        this.children[1] = this.node.transform.GetChild(1).gameObject;
        this.children[2] = this.node.transform.GetChild(2).gameObject;
    }


    /// <summary>
    /// This function will be called every frame to update the timer
    /// Decrements the maxPatience and timeToMake by 1
    /// </summary>
    /// <returns>True if the maxPatience is less than or equal to 0, false otherwise</returns>
    public bool updateNodeTimers()
    {
        if(this.maxPatience <= 0)
        {
            //destroy self (just set active to false for now)
            this.node.SetActive(false);
            return true;
        }
        //this function will be called every frame to update the timer
        this.maxPatience -= 1;
        if(this.timeToMake > 0)
        { 
            this.timeToMake -= 1;
        }
        return false;
    }

    /// <summary>
    /// This function will be called every frame to update the timer
    /// Decrements the maxPatience and timeToMake by 1
    /// </summary>
    /// <param name="hotbarPanel">The panel that the hotbar is on</param>
    /// <param name="xOffset">The x offset from the hotbarPanel</param>
    /// <param name="yOffset">The y offset from the hotbarPanel</param>
    public void displayNode(GameObject hotbarPanel, float xOffset, float yOffset){
        //make sure the color, order, and timer are updated
        //position has to be relative to the hotbarPanel
        float x = hotbarPanel.transform.position.x;
        float y = hotbarPanel.transform.position.y;
        this.node.transform.position = new Vector3(x + xOffset, y + yOffset, 0);
        this.node.transform.SetParent(hotbarPanel.transform);

        this.node.GetComponent<Image>().color = this.face;
        
        this.children[0].GetComponent<TextMeshProUGUI>().text = this.maxPatience.ToString();
        this.children[1].GetComponent<TextMeshProUGUI>().text = this.order;
        this.children[2].GetComponent<TextMeshProUGUI>().text = this.timeToMake.ToString();
    }
}
