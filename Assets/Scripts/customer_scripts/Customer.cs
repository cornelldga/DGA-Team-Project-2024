using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Customer : MonoBehaviour
{
    [Header("Customer Attributes")]
    public string customerName;
    public float waitTime;
    public float cookTime;
    public Transform player;
    public float interactionRange = 2f;

    [Header("Tentative Materials")]
    public Material grayMaterial; // order not taken yet
    public Material greenMaterial; // waiting for order
    public Material redMaterial; // patience ran out
    public Material blueMaterial; // order successfully complete

    [Header("Movement Settings")]
    public float movementAmplitude = 0.5f; // Distance of movement from the starting position
    public float movementFrequency = 1f; // Speed of the oscillation

    private Vector3 startingPosition;

    private float timer = 0f;
    private Renderer customerRenderer;
    private bool orderTaken = false;
    private bool foodReady = false;
    private bool isOrderCompleted = false;

    private enum CustomerState { WaitingForOrder, Cooking, Returning, Done }
    private CustomerState currentState;

    void Start()
    {
        customerRenderer = GetComponent<Renderer>();
        currentState = CustomerState.WaitingForOrder;
        customerRenderer.material = grayMaterial;
        startingPosition = transform.position;
        Debug.Log(customerName + " is waiting for an order.");
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case CustomerState.WaitingForOrder:
                if (distanceToPlayer <= interactionRange && Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log(customerName + " placed an order.");
                    currentState = CustomerState.Cooking;
                    customerRenderer.material = greenMaterial;
                    timer = 0f;
                    orderTaken = true;
                }
                break;

            case CustomerState.Cooking:
                if (timer >= cookTime && !foodReady)
                {
                    Debug.Log(customerName + "'s food is ready for delivery.");
                    foodReady = true;
                }

                if (timer >= waitTime)
                {
                    if (!isOrderCompleted)
                    {
                        Debug.Log(customerName + " is upset! The order was not returned in time.");
                        currentState = CustomerState.Done;
                        customerRenderer.material = redMaterial;
                    }
                }

                if (distanceToPlayer <= interactionRange && Input.GetKeyDown(KeyCode.E) && foodReady)
                {
                    CompleteOrder();
                }
                break;

            case CustomerState.Done:
                break;
        }

        if (currentState != CustomerState.WaitingForOrder && currentState != CustomerState.Done)
        {
            timer += Time.deltaTime;
        }

        MoveCustomer();
    }

    public void CompleteOrder()
    {
        Debug.Log(customerName + " received their order.");
        currentState = CustomerState.Done;
        customerRenderer.material = blueMaterial;
        isOrderCompleted = true;
    }

    void MoveCustomer()
    {
        // Calculate the movement offset using a sine wave
        float movementOffset = Mathf.Sin(Time.time * movementFrequency) * movementAmplitude;

        // Apply the movement along the desired axis (e.g., z-axis)
        Vector3 newPosition = startingPosition + new Vector3(0f, 0f, movementOffset);

        // Update the customer's position
        transform.position = newPosition;
    }

    // ---------------------------- Getters ----------------------------
    public bool IsOrderTaken()
    {
        return orderTaken;
    }

    public bool IsFoodReady()
    {
        return foodReady;
    }

    public bool IsOrderCompleted()
    {
        return isOrderCompleted;
    }
}
