using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The UIController class is responsible for managing the UI elements in the game. This class is responsible for holding UI elements and providing the methods to update them.
/// </summary>
public class UIController : MonoBehaviour
{


    //these are the customers that the UIController is going to display in the hotbar
    //these will be provided by the player when we merge the code, right now it's being serialzed so that we can see it in the editor
    [SerializeField] private GameObject[] customers;

    //this is the hotbar that the UIController is going to display the customers in
    [SerializeField] private GameObject hotbar;

    //this is the list of hotbar nodes that the UIController is going to display the customers in
    private List<GameObject> hotbarNodes = new List<GameObject>();

    //this is the prefab for the hotbar nodes
    [SerializeField] private GameObject hotbarNodePrefab;


    //I don't have any buttons set up to change this index, once we decide what keys to use to change the selected node, we can add that functionality
    private int hotbarIndex = 1;

    /// <summary>
    ///Start method.
    /// </summary>
    void Start(){
        updateOrdersUI();
    }
    
    /// <summary>
    /// Update is called once per frame. This function updates the orders UI.
    /// </summary>
    void Update()
    {
        //updateOrdersUI is going to be called by the Player script every second (according to Jacob)
        //so we don't need to call it here, but for now, I'm going to call it here so that we can see the orders UI update in the editor
        updateOrdersUI();
    }


    /// <summary>
    ///updates the orders UI by clearing the current hotbar nodes and populating it with new nodes based on the current customers.
    /// </summary>
    public void updateOrdersUI(){
        //clear the current hotbar nodes
        foreach(GameObject node in hotbarNodes){
            Destroy(node);
        }
        hotbarNodes.Clear();

        //populate the hotbar with the current customers
        foreach(GameObject customer in customers){
            if(customer == null){
                continue;
            }
            GameObject node = Instantiate(hotbarNodePrefab, hotbar.transform);
            HotbarNode hotbarNode = node.GetComponent<HotbarNode>();

            hotbarNode.setOrder(customer.GetComponent<Customer>().order);
            hotbarNode.setCustomerImage(customer.GetComponent<Customer>().customerImage);
            hotbarNode.setCookTime(customer.GetComponent<Customer>().cookTime);
            hotbarNode.setPatienceTime(customer.GetComponent<Customer>().patienceTime);

            hotbarNodes.Add(node);
        }


        //set the selected border of the hotbar node at the hotbarIndex
        //this is really hacky, but it's just to show off selection for now
        for(int i = 0; i < hotbarNodes.Count; i++){
            if(i == hotbarIndex){
                hotbarNodes[i].GetComponent<HotbarNode>().getSelectedBorder().SetActive(true);
            } else {
                hotbarNodes[i].GetComponent<HotbarNode>().getSelectedBorder().SetActive(false);
            }
        }
    }
}