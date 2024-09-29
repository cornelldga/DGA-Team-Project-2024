using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public string customerName;
    public float waitTime;   // Time that the customer will wait to place an order
    public float cookTime;   // Time it takes to cook food
    public float returnTime; // Total time to return food t customer
    public bool isOrderCompleted = false;

    private float timer = 0f;
    private enum CustomerState { Waiting, Ordering, Cooking, Returning, Done }
    private CustomerState currentState;
    public void Initialize(string name, float wait, float cook, float returnT)
    {
        customerName = name;
        waitTime = wait;
        cookTime = cook;
        returnTime = returnT;
        currentState = CustomerState.Waiting;
    }
    void Update()
    {
        timer += Time.deltaTime;

        switch (currentState)
        {
            case CustomerState.Waiting:
                if (timer >= waitTime)
                {
                    currentState = CustomerState.Ordering;
                    timer = 0f;
                    Debug.Log(customerName + " has placed an order.");
                }
                break;

            case CustomerState.Ordering:
                currentState = CustomerState.Cooking;
                break;

            case CustomerState.Cooking:
                if (timer >= cookTime)
                {
                    currentState = CustomerState.Returning;
                    timer = 0f;
                    Debug.Log(customerName + " order ready for return.");
                }
                break;

            case CustomerState.Returning:
                if (timer >= returnTime)
                {
                    if (!isOrderCompleted)
                    {
                        Debug.Log(customerName + " not returned in time.");
                    }
                    currentState = CustomerState.Done;
                    timer = 0f;
                }
                break;
        }
    }
    public void CompleteOrder()
    {
        isOrderCompleted = true;
        currentState = CustomerState.Done;
        Debug.Log(customerName + " received their order.");
    }
}
