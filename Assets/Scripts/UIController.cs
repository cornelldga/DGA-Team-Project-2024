using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for controlling the UI
/// This includes creation of new UI elements dynamically and updating them
/// </summary>
public class UIController : MonoBehaviour
{
    [SerializeField] GameObject hotbarPanel;
    [SerializeField] GameObject hotbarNodePrefab;
    private ArrayList hotbar;
    void Start()
    {
        // Create a new hotbar
        hotbar = new ArrayList();
        // Add some nodes to the hotbar
        hotbar.Add(new HotbarNode(5, Color.red, "Burger", 5, hotbarNodePrefab, hotbarPanel));
        hotbar.Add(new HotbarNode(10, Color.blue, "Fries", 5, hotbarNodePrefab, hotbarPanel));
        hotbar.Add(new HotbarNode(12, Color.green, "Drink", 5, hotbarNodePrefab, hotbarPanel));

        // Display the hotbar
        for(int i = 0; i < hotbar.Count; i++)
        {
            HotbarNode node = (HotbarNode)hotbar[i];
            node.displayNode(hotbarPanel, i * 100, 0);
        }

        // Update the hotbar every second
        InvokeRepeating("updateHotbar", 0.0f, 1.0f);
    }


    void Update(){
        float width = hotbarNodePrefab.GetComponent<RectTransform>().rect.width;
        this.transform.position = new Vector3((Screen.width/2) - (hotbar.Count * (width/2)) + width/2, transform.position.y, 0);
    }

    /// <summary>
    /// This function will be called every second to update the hotbar
    /// </summary>
    void updateHotbar(){
        //get the width of a hotbar node
        float width = hotbarNodePrefab.GetComponent<RectTransform>().rect.width;
        for(int i = 0; i < hotbar.Count; i++)
        {
            HotbarNode node = (HotbarNode)hotbar[i];
            bool done = node.updateNodeTimers();
            if(done){
                hotbar.RemoveAt(i);
                i--;
            }
            float x = i * width;
            node.displayNode(hotbarPanel, x, 0);
        }
    }
}
