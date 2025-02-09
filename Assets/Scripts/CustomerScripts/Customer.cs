using System;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

/// <summary>
/// The Customer class is responsible for managing the customer's attributes, state, and behavior. 
/// <para> NOTE: For now, the customer is not updating its timer. </para>
/// <para> NOTE: For now, the customer is moving back and forth. </para>
/// </summary>
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(PedSoundManager))]
public class Customer : MonoBehaviour
{
    [Header("Customer Attributes")]
    public string customerName;
    public string orderName;
    /// <summary>
    /// The image that will be displayed in UI.
    /// </summary>
    [Tooltip("The image that will be displayed in UI.")]
    public Sprite customerImage;
    /// <summary>
    /// The sprite that will be displayed in the game world.
    /// </summary>
    [Tooltip("The sprite that will be displayed in the game world.")]
    public GameObject customerSprite;
    /// <summary>
    /// The time the customer will wait for their order, after which the order will fail. 
    /// </summary>
    public float waitTime;
    /// <summary>
    /// The time it takes for the customer's order to be cooked.
    /// </summary>
    [Tooltip("The time it takes for the customer's order to be cooked.")]
    public float cookTime;
    /// <summary>
    /// The range within which the player can interact with the customer.
    /// </summary>
    [Tooltip("The range within which the player can interact with the customer.")]
    public float interactionRange = 2f;
    public GameObject detectionRange;

    [Header("Tentative Materials")]
    public Material grayMaterial; // order not taken yet
    public Material greenMaterial; // waiting for order
    public Material redMaterial; // patience ran out
    public Material blueMaterial; // order successfully complete

    [Header("Movement Settings")]
    /// <summary>
    /// if the customer is moving back and forth along the Z-axis. If unchecked, the customer will move along the X-axis (West and East).
    /// </summary>
    public bool isMovingNorthSouth = true;

    /// <summary>
    /// The amplitude of the oscillation.
    /// </summary>
    public float movementAmplitude = .25f;
    /// <summary>
    /// The speed of the oscillation.
    /// </summary>
    public float movementFrequency = 1f;

    private Vector3 startingPosition;
    private LineRenderer lineRenderer;
    private float timer = 0f;
    private Renderer customerRenderer;
    private bool orderTaken = false;
    private bool foodReady = false;
    private bool isOrderCompleted = false;
    private enum CustomerState { WaitingForOrder, Cooking, Returning, Done }
    private CustomerState currentState;
    private Vector3 previousPosition;
    private AnimatorController animController;
    private PedSoundManager pedSoundManager;

    void Start()
    {
        customerRenderer = GetComponent<Renderer>();
        currentState = CustomerState.WaitingForOrder;
        customerRenderer.material = grayMaterial;
        startingPosition = transform.position;
        // set the radius of the customer's detection range
        detectionRange.GetComponent<SphereCollider>().radius = interactionRange / 2;

        // Initialize and configure the LineRenderer
        SetupInteractionRangeIndicator();

        // Get the Billboard component
        animController = customerSprite.GetComponent<AnimatorController>();
        if (isMovingNorthSouth)
        {
            SpriteRenderer spriteRenderer = customerSprite.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = true;
        }

        pedSoundManager = GetComponent<PedSoundManager>();
    }

    void Update()
    {
        // check if player is in range
        if (detectionRange.GetComponent<CustomerRange>().playerInRange)
        {
            // change the color of the line renderer to green
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        }
        else
        {
            // change the color of the line renderer to blue
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
        }

        // update the state of the customer
        switch (currentState)
        {
            case CustomerState.WaitingForOrder:
                float oil = GameManager.Instance.getPlayer().GetOil();
                if (detectionRange.GetComponent<CustomerRange>().playerInRange && Input.GetKeyDown(KeyCode.E) && oil >= 20)
                {
                    // TAKING ORDER
                    currentState = CustomerState.Cooking;
                    customerRenderer.material = greenMaterial;
                    timer = 0f;
                    orderTaken = true;
                    GameManager.Instance.getPlayer().AddOil(-20);
                    GameManager.Instance.TakeOrder(this);
                    pedSoundManager.PlayTakeOrderSound();
                }
                break;

            case CustomerState.Cooking:
                // NOTE: I removed the function of the timer for now.
                if (cookTime <= 0 && !foodReady)
                {
                    foodReady = true;
                    AudioManager.Instance.Play("sfx_TimerDing");
                }

                if (waitTime <= 0)
                {
                    if (!isOrderCompleted)
                    {
                        // FAIL ORDER
                        currentState = CustomerState.Done;
                        customerRenderer.material = redMaterial;
                        GameManager.Instance.RemoveOrder(this);
                        pedSoundManager.PlayOrderFailedSound();
                    }
                    break;
                }

                if (detectionRange.GetComponent<CustomerRange>().playerInRange && Input.GetKeyDown(KeyCode.E) && foodReady)
                {
                    ReceiveOrder();
                }

                // NOTE: nobody is updating the cookTime now. Supposedly, Game manager or Player should do this. Par requirement, I am updating the waitTime now. This will render the customer to go straight into the red state.
                waitTime -= Time.deltaTime;
                break;

            case CustomerState.Done:
                break;
        }

        if (currentState != CustomerState.WaitingForOrder && currentState != CustomerState.Done)
        {
            timer += Time.deltaTime;
        }

        // move the customer back and forth
        MoveCustomer();

        // line up the rotation angle with the camera
        animController.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, 0);

        // Check facing direction and set the animator
        Vector3 movingDirection = transform.position - previousPosition;
        SetAnimationDirection(movingDirection);
        previousPosition = transform.position;
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
        pedSoundManager.PlayOrderCompleteSound();
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

    /// <returns> If this customer is active. Becomes inactive once order is completed or failed. </returns>
    public bool IsInactive()
    {
        if (currentState == CustomerState.Done)
        {
            return true;
        }
        return false;
    }


    // PRIVATE METHODS ----------------------------

    private void MoveCustomer()
    {
        // Calculate the movement offset using a sine wave
        float movementOffset = Mathf.Sin(Time.time * movementFrequency) * movementAmplitude;

        // Apply the movement along the SELF's Z-axis
        Vector3 direction = isMovingNorthSouth ? Vector3.forward : Vector3.right;
        Vector3 newPosition = startingPosition + direction * movementOffset;

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

    /// <summary>
    /// Set the animation direction based on the distance between the current position and the destination.
    /// </summary>
    private void SetAnimationDirection(Vector3 distance)
    {
        bool isWalkingWest = distance.x < 0 && Mathf.Abs(distance.x) > Mathf.Abs(distance.z);
        bool isWalkingEast = distance.x > 0 && Mathf.Abs(distance.x) > Mathf.Abs(distance.z);
        bool isWalkingSouth = distance.z < 0 && Mathf.Abs(distance.z) > Mathf.Abs(distance.x);
        bool isWalkingNorth = distance.z > 0 && Mathf.Abs(distance.z) > Mathf.Abs(distance.x);
        // set all to false
        animController.SetMovingWest(false);
        animController.SetMovingEast(false);
        animController.SetMovingNorth(false);
        animController.SetMovingSouth(false);
        if (isWalkingWest)
        {
            animController.SetMovingWest(true);
        }
        else if (isWalkingEast)
        {
            animController.SetMovingEast(true);
        }
        else if (isWalkingNorth)
        {
            animController.SetMovingNorth(true);
        }
        else if (isWalkingSouth)
        {
            animController.SetMovingSouth(true);
        }
    }
}
