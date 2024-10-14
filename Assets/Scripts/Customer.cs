using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //these are the properties of the customer, but they should probably be provided by the game manager when we merge the code
    [SerializeField] private Sprite customerImage;
    [SerializeField] private string order;
    [SerializeField] private int cookTime;
    [SerializeField] private int patienceTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
