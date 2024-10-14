using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The UIController class is responsible for managing the UI elements in the game. This class is responsible for updating the orders UI.
/// </summary>
public class UIController : MonoBehaviour
{

    private GameObject[] customers;
    [SerializeField] private GameObject hotbar;
    private List<GameObject> hotbarNodes = new List<GameObject>();
    [SerializeField] private GameObject hotbarNodePrefab;

    private int hotbarIndex = 0;

    /// <summary>
    ///Constructor for the UIController class. This constructor initializes the UIController with the customers that are currently in the scene.
    /// </summary>
    public UIController(GameObject[] customers){
        this.customers = customers;
        updateOrdersUI();
    }
    
    /// <summary>
    /// Update is called once per frame. This function updates the orders UI.
    /// </summary>
    void Update()
    {
        updateOrdersUI();
    }


    /// <summary>
    ///updates the orders UI by clearing the current hotbar nodes and populating it with new nodes based on the current customers.
    /// </summary>
    void updateOrdersUI(){
        //clear the hotbar
        foreach (GameObject hotbarNode in hotbarNodes){
            Destroy(hotbarNode);
        }
        foreach (GameObject customer in customers){
            //this assumes that the customer script is called Customer
            GameObject hotbarNode = Instantiate(hotbarNodePrefab, hotbar.transform);
            hotbarNode.GetComponent<HotbarNode>().setOrder(customer.GetComponent<Customer>().getOrder());
            hotbarNode.GetComponent<HotbarNode>().setCustomerImage(customer.GetComponent<Customer>().getCustomerImage());
            hotbarNode.GetComponent<HotbarNode>().setCookTime(customer.GetComponent<Customer>().getCookTime());
            hotbarNode.GetComponent<HotbarNode>().setPatienceTime(customer.GetComponent<Customer>().getPatienceTime());
            hotbarNodes.Add(hotbarNode);
            //if the node we just added is the selected node, set it to selected
            if(hotbarIndex == hotbarNodes.Count){
                hotbarNode.GetComponent<HotbarNode>().setSelected(true);
            }
        }
    }
}