using UnityEngine;

/// <summary>
/// The Customer class is responsible for managing the customer's attributes, state, and behavior. 
/// <para> NOTE: For now, the customer is not updating its timer. </para>
/// <para> NOTE: For now, the customer is moving back and forth. </para>
/// </summary>
public class Customer : MonoBehaviour, ICrashable
{
    [Header("Customer Attributes")]
    public string customerName;
    public string orderName;
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
    public SpriteRenderer rangeIndicatorSprite;
    public GameObject hungrySign;

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

    [Header("Fade Settings")]
    /// <summary>
    /// How long it takes for customer to disappear
    /// </summary>
    public float fadeOutDuration = 7f;
    /// <summary>
    /// Difference between customer disappearing and ring indicator disappearing
    /// </summary>
    public float destroyDelay = 0f;
    private float fadeTimer = 0f;
    private bool isFading = false;
    private float originalAlpha;
    private SpriteRenderer spriteRenderer;
    [Header("Knockback Settings")]
    [SerializeField] float invincibilityDuration = 3.0f; // Duration of invincibility in seconds
    [SerializeField] float knockbackCooldown = 0.5f; // Delay before rechecking for kinematic state
    [SerializeField] float knockbackThreshold = 1f; // Minimum speed required to knock back a pedestrian
    float knockbackTimer = 0f;
    bool isKnockedBack = false;
    bool isInvincible = false;
    float invincibilityTimer = 0f;
    Rigidbody rb;
    [SerializeField] float knockbackScale = 3f;
    [Tooltip("The maximum force that can be applied to a pedestrian")]
    [SerializeField] float maxKnockback = 25f;

    private Vector3 startingPosition;
    private float timer = 0f;
    private Renderer customerRenderer;
    private bool orderTaken = false;
    private bool foodReady = false;
    private bool isOrderCompleted = false;
    private enum CustomerState { WaitingForOrder, Cooking, Returning, Fading, Done }
    private CustomerState currentState;
    private Vector3 previousPosition;
    private AnimatorController animController;
    private PedSoundManager pedSoundManager;
    private CustomerSoundManager customerSoundManager;
    private bool isPedSound = false;
    private Vector3 destination; // the destination of the customer if knocked back

    void Start()
    {
        customerRenderer = GetComponent<Renderer>();
        currentState = CustomerState.WaitingForOrder;
        startingPosition = transform.position;
        // set the radius of the customer's detection range
        detectionRange.GetComponent<SphereCollider>().radius = interactionRange;

        // Initialize and configure the LineRenderer
        // SetupInteractionRangeIndicator();

        // immediately size + tint the ring:
        UpdateRangeIndicator();

        // Get the Billboard componen
        animController = customerSprite.GetComponent<AnimatorController>();

        // Used for changing the alpha for the renderer/material
        spriteRenderer = customerSprite.GetComponent<SpriteRenderer>();
        originalAlpha = spriteRenderer.color.a;
        // set animation speed to 0.5
        animController.changeAnimSpeed(0.4f);
        if (isMovingNorthSouth)
        {
            SpriteRenderer spriteRenderer = customerSprite.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = true;
        }

        pedSoundManager = GetComponent<PedSoundManager>();
        customerSoundManager = GetComponent<CustomerSoundManager>();
        if (pedSoundManager == null)
        {
            isPedSound = false;
        }
        else
        {
            isPedSound = true;
        }
        rb = GetComponent<Rigidbody>();

        hungrySign.SetActive(false);
    }

    void Update()
    {
        if (currentState != CustomerState.Fading || currentState != CustomerState.Fading)
        {
            // range color is R = 80 G = 140 B = 80 A = 0.7
            Color rangeColor = new Color(0.31f, 0.55f, 0.31f, 0.7f);

            bool inRange = detectionRange.GetComponent<CustomerRange>().playerInRange;
            rangeIndicatorSprite.color = inRange
                ? rangeColor
                : Color.white;
        }

        // update the state of the customer
        switch (currentState)
        {
            case CustomerState.WaitingForOrder:
                float oil = GameManager.Instance.getPlayer().GetOil();
                if (detectionRange.GetComponent<CustomerRange>().playerInRange && Input.GetKeyDown(KeyCode.E) && oil >= 20)
                {
                    GameManager.Instance.TakeOrder(this);
                    if (isPedSound)
                    {
                        pedSoundManager.PlayTakeOrderSound(transform.position);
                    }
                    else
                    {
                        customerSoundManager.PlayTakeOrderSound(transform.position);
                    }
                }
                break;

            case CustomerState.Cooking:
                if (cookTime <= 0 && !foodReady)
                {
                    foodReady = true;
                    hungrySign.SetActive(true);
                    AudioManager.Instance.Play("sfx_TimerDing");
                }

                if (waitTime <= 0)
                {
                    if (!isOrderCompleted)
                    {
                        // Start the fading
                        currentState = CustomerState.Fading;
                        StartFading();
                        GameManager.Instance.RemoveOrder(this);
                        AudioManager.Instance.PlaySound("sfx_Anger");
                        if (isPedSound)
                        {
                            pedSoundManager.PlayOrderFailedSound(transform.position);
                        }
                        else
                        {
                            customerSoundManager.PlayOrderFailedSound(transform.position);
                        }
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
            case CustomerState.Fading:
                HandleFading();
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
    }
    /// <summary>
    /// Called when the player accepts the order
    /// </summary>
    public void TookOrder()
    {
        currentState = CustomerState.Cooking;
        timer = 0f;
        orderTaken = true;
        GameManager.Instance.getPlayer().AddOil(-20);
        if (isPedSound)
        {
            pedSoundManager.PlayTakeOrderSound(transform.position);
        }
        else
        {
            customerSoundManager.PlayTakeOrderSound(transform.position);
        }
    }

    /// <summary>
    /// Completes the order for the customer. 
    /// For now, it will change the customer's material to blue.
    /// It will call the GameManager to update the game status.
    /// </summary>
    public void ReceiveOrder()
    {
        currentState = CustomerState.Fading;
        StartFading();
        GameManager.Instance.CompleteOrder(this);
        isOrderCompleted = true;
        if (isPedSound)
        {
            pedSoundManager.PlayOrderCompleteSound(transform.position);
        }
        else
        {
            customerSoundManager.PlayOrderCompleteSound(transform.position);
        }
    }

    public void Crash(Vector3 speedVector, Vector3 position)
    {
        if (!isInvincible)
        {
            float magnitude = speedVector.magnitude;
            Debug.Log("Crash! Pedestrian hit with speed: " + magnitude + " speed vector: " + speedVector);
            if (magnitude > knockbackThreshold)
            {
                Vector3 knockbackDirection = (transform.position - position).normalized;
                knockbackDirection.y = 0; // Ignore vertical direction
                float knockbackForce = Mathf.Min(magnitude * knockbackScale, maxKnockback);

                knockbackDirection += Vector3.up * 0.2f;
                knockbackDirection.Normalize();

                rb.isKinematic = false;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

                isKnockedBack = true;
                isInvincible = true;
                invincibilityTimer = invincibilityDuration;
                knockbackTimer = knockbackCooldown; // Start knockback cooldown

                // play hurt sound
                if (isPedSound)
                {
                    pedSoundManager.PlayHurtSound(transform.position);
                }
                else
                {
                    customerSoundManager.PlayHurtSound(transform.position);
                }

                Debug.Log("Crash! Pedestrian knocked back with force: " + knockbackForce);
            }
        }
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


    private void StartFading()
    {
        isFading = true;
        fadeTimer = 0f;
    }

    private void HandleFading()
    {
        if (!isFading) return;

        fadeTimer += Time.deltaTime;
        float normalizedTime = fadeTimer / fadeOutDuration;

        if (normalizedTime <= 1f)
        {
            float alpha = Mathf.Lerp(originalAlpha, 0f, normalizedTime);
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;

            Color rangeColor = rangeIndicatorSprite.color;
            rangeColor.a = alpha;
            rangeIndicatorSprite.color = rangeColor;

            Color hungryColor = hungrySign.GetComponent<SpriteRenderer>().color;
            hungryColor.a = alpha;
            hungrySign.GetComponent<SpriteRenderer>().color = hungryColor;
        }
        else
        {
            isFading = false;
            currentState = CustomerState.Done;
            Destroy(gameObject, destroyDelay);
        }
    }
    private Vector3 targetPosition;
    private bool movingToEnd = true; // Determines direction

    private void MoveCustomer()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;

            animController.SetMovingNorth(false);
            animController.SetMovingSouth(false);
            animController.SetMovingEast(false);
            animController.SetMovingWest(false);

            // Determine crash direction
            Vector3 deltaPosition = transform.position - previousPosition;
            if (deltaPosition.x > 0) animController.SetCrashRight(true);
            else if (deltaPosition.x < 0) animController.SetCrashLeft(true);

            if (knockbackTimer <= 0f && rb.velocity.magnitude < 0.1f)
            {
                rb.isKinematic = true;
                isKnockedBack = false;
                animController.SetCrashRight(false);
                animController.SetCrashLeft(false);
                SetAnimationDirection(destination - transform.position);
            }
        }
        else
        {
            // Define movement axis
            Vector3 moveDirection = isMovingNorthSouth ? Vector3.forward : Vector3.right;

            // Define two endpoints
            Vector3 start = startingPosition - moveDirection * movementAmplitude;
            Vector3 end = startingPosition + moveDirection * movementAmplitude;

            // Set the target based on movement direction
            targetPosition = movingToEnd ? end : start;

            // Move towards the target
            float step = movementFrequency * Time.deltaTime;
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, step));

            // Check if reached the target
            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                movingToEnd = !movingToEnd; // Toggle direction
            }

            // Update animation direction
            SetAnimationDirection(targetPosition - transform.position);
            previousPosition = transform.position;
        }
    }

    // private void SetupInteractionRangeIndicator()
    // {
    //     // Get or Add a LineRenderer component
    //     lineRenderer = GetComponent<LineRenderer>();
    //     if (lineRenderer == null)
    //     {
    //         lineRenderer = gameObject.AddComponent<LineRenderer>();
    //     }

    //     // Configure LineRenderer properties
    //     lineRenderer.positionCount = 0; // Will be set later
    //     lineRenderer.loop = true; // Close the circle
    //     lineRenderer.useWorldSpace = false; // Relative to the GameObject
    //     lineRenderer.startWidth = 0.05f;
    //     lineRenderer.endWidth = 0.05f;
    //     lineRenderer.startColor = Color.blue;
    //     lineRenderer.endColor = Color.blue;
    //     lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Use a simple material

    //     // Create the circle
    //     CreateCirclePoints();
    // }

    // private void CreateCirclePoints()
    // {
    //     int segments = 100; // Number of segments to make the circle smooth
    //     float angle = 0f;

    //     lineRenderer.positionCount = segments + 1; // +1 to close the circle

    //     for (int i = 0; i <= segments; i++)
    //     {
    //         float x = Mathf.Cos(Mathf.Deg2Rad * angle) * interactionRange;
    //         float z = Mathf.Sin(Mathf.Deg2Rad * angle) * interactionRange;

    //         lineRenderer.SetPosition(i, new Vector3(x, 0f, z));

    //         angle += (360f / segments);
    //     }
    // }

    private void UpdateRangeIndicator()
    {
        // Make the ring’s world‑space diameter = interactionRange * 2
        // (since we imported the sprite so that scale=1 → diameter=1 unit)
        float diameter = interactionRange * 2f;
        rangeIndicatorSprite.transform.localScale
            = new Vector3(diameter, diameter, diameter);
    }

    // if you ever change interactionRange at runtime:
    public void SetInteractionRange(float r)
    {
        interactionRange = r;
        detectionRange.GetComponent<SphereCollider>().radius = r;
        UpdateRangeIndicator();
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
