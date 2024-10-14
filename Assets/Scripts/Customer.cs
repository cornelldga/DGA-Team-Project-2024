using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The customer class as provided by me is a placeholder. This is only to be used for testing of the UIController and HotbarNode classes.
/// This class is responsible for decrementing it's own timer, but that will be done by the player when we merge the code.
/// </summary>
public class Customer : MonoBehaviour
{
    //these are the properties of the customer, but they should probably be provided by the game manager when we merge the code
    [SerializeField] public Sprite customerImage;
    [SerializeField] public string order;
    [SerializeField] public int cookTime;
    [SerializeField] public int patienceTime;
    // Start is called before the first frame update

    //make a timer that decreases patienceTime and cookTime every second
    void Start(){
        //decrement patienceTime and cookTime every second
        InvokeRepeating("decrementTimes", 1.0f, 1.0f);
    }

    private void decrementTimes(){
        //decrement patienceTime and cookTime
        //if patienceTime is 0, destroy the customer
        if(cookTime > 0){
            cookTime--;
        }
        patienceTime--;
        if (patienceTime <= 0){
            //destroy the customer
            Destroy(gameObject);
        }
    }
}
