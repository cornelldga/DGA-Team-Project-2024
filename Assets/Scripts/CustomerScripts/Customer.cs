using UnityEngine;

/// <summary>
/// The Customer class is responsible for managing the customer's attributes, state, and behavior. 
/// <para> NOTE: For now, the customer is not updating its timer. </para>
/// <para> NOTE: For now, the customer is moving back and forth. </para>
/// </summary>
[RequireComponent(typeof(Renderer))]
public class Customer : MonoBehaviour
{
    [Header("Customer Attributes")]
    public string customerName;
    public string orderName;
    public Sprite customerImage;
    /// <summary>
    /// The time the customer will wait for their order, after which the order will fail. 
    /// </summary>
    public float waitTime;

    /// <summary>
    /// The time it takes for the customer's order to be cooked.
    /// </summary>
    public float cookTime;
    public float interactionRange = 2f;
    public GameObject detectionRange;

    [Header("Tentative Materials")]
    public Material grayMaterial; // order not taken yet
    public Material greenMaterial; // waiting for order
    public Material redMaterial; // patience ran out
    public Material blueMaterial; // order successfully complete

    [Header("Movement Settings")]
    public float movementAmplitude = 0.5f; // Distance of movement from the starting position
    public float movementFrequency = 1f; // Speed of the oscillation

    private Vector3 startingPosition;
    private LineRenderer lineRenderer;
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
        // set the radius of the customer's detection range
        detectionRange.GetComponent<SphereCollider>().radius = interactionRange / 2;
        // Initialize and configure the LineRenderer
        SetupInteractionRangeIndicator();
    }

    void Update()
    {
        // float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case CustomerState.WaitingForOrder:
                if (detectionRange.GetComponent<CustomerRange>().playerInRange && Input.GetKeyDown(KeyCode.E))
                {
                    currentState = CustomerState.Cooking;
                    customerRenderer.material = greenMaterial;
                    timer = 0f;
                    orderTaken = true;

                    // TODO: Pass self to Player 
                    // NOTE: I used GameManager.Instance.AddCustomer() instead
                    // This is not going to work since I am not passing myself. 
                    // We should change this.
                    //GameManager.Instance.addCustomer();
                    GameManager.Instance.TakeOrder(this);


                }
                break;

            case CustomerState.Cooking:
                // NOTE: I removed the function of the timer for now.
                if (cookTime <= 0 && !foodReady)
                {
                    foodReady = true;
                }

                if (waitTime <= 0)
                {
                    if (!isOrderCompleted)
                    {
                        currentState = CustomerState.Done;
                        customerRenderer.material = redMaterial;
                        GameManager.Instance.RemoveOrder(this);
                    }
                    break;
                }

                if (detectionRange.GetComponent<CustomerRange>().playerInRange && Input.GetKeyDown(KeyCode.E) && foodReady)
                {
                    ReceiveOrder();
                }

                waitTime -= Time.deltaTime;

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

    /// <summary>
    /// Completes the order for the customer. 
    /// For now, it will change the customer's material to blue.
    /// It will call the GameManager to update the game status.
    /// </summary>
    public void ReceiveOrder()
    {
        currentState = CustomerState.Done;
        customerRenderer.material = blueMaterial;
        GameManager.Instance.CompleteOrder(this);
        isOrderCompleted = true;
        hotbarManager.RemoveFromHotBar(this);
    }

    // GETTERS ----------------------------

    /// <returns> If this customer's order is already taken. </returns>
    public bool IsOrderTaken()
    {
        return orderTaken;
    }

    /// <returns> If this customer's food is ready for delivery. </returns>
    public bool IsFoodReady()
    {
        return foodReady;
    }

    /// <returns> If this customer's order is completed. </returns>
    public bool IsOrderCompleted()
    {
        return isOrderCompleted;
    }


    // PRIVATE METHODS ----------------------------

    private void MoveCustomer()
    {
        // Calculate the movement offset using a sine wave
        float movementOffset = Mathf.Sin(Time.time * movementFrequency) * movementAmplitude;

        // Apply the movement along the desired axis (e.g., z-axis)
        Vector3 newPosition = startingPosition + new Vector3(0f, 0f, movementOffset);

        // Update the customer's position
        transform.position = newPosition;
    }

    private void SetupInteractionRangeIndicator()
    {
        // Get or Add a LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Configure LineRenderer properties
        lineRenderer.positionCount = 0; // Will be set later
        lineRenderer.loop = true; // Close the circle
        lineRenderer.useWorldSpace = false; // Relative to the GameObject
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Use a simple material

        // Create the circle
        CreateCirclePoints();
    }

    private void CreateCirclePoints()
    {
        int segments = 100; // Number of segments to make the circle smooth
        float angle = 0f;

        lineRenderer.positionCount = segments + 1; // +1 to close the circle

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * interactionRange;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * interactionRange;

            lineRenderer.SetPosition(i, new Vector3(x, 0f, z));

            angle += (360f / segments);
        }
    }
}
