using UnityEngine;

/// <summary>
/// The Customer class is responsible for managing the customer's attributes, state, and behavior. 
/// <para> NOTE: For now, the customer is not updating its timer. </para>
/// <para> NOTE: For now, the customer is moving back and forth. </para>
/// </summary>
public class Customer : MonoBehaviour, ICrashable
{
    [Header("Customer Attributes")]
    public CustomerType customerType;
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

    [Header("Moving Settings")]
    [Tooltip("Drag these in Scene to set the back-and-forth patrol endpoints.")]
    public Vector3 patrolPointA = Vector3.zero;
    public Vector3 patrolPointB = Vector3.zero;
    [SerializeField] float moveSpeed;

    [Header("Patrol Obstacle Detection")]
    [Tooltip("Layers considered obstacles for patrol direction.")]
    public LayerMask obstacleMask;

    [Header("Fade Settings")]
    /// <summary>
    /// How long it takes for customer to disappear
    /// </summary>
    public float fadeOutDuration = 7f;
    /// <summary>
    /// Difference between customer disappearing and ring indicator disappearing
    /// </summary>
    private float fadeTimer = 0f;
    private SpriteRenderer spriteRenderer;
    [Header("Knockback Settings")]
    [SerializeField] float invincibilityDuration = 3.0f; // Duration of invincibility in seconds
    [SerializeField] float knockbackCooldown = 0.5f; // Delay before rechecking for kinematic state
    [SerializeField] float knockbackThreshold = 1f; // Minimum speed required to knock back a pedestrian
    float knockbackTimer = 0f;
    bool isKnockedBack = false;
    bool isInvincible = false;
    Rigidbody rb;
    [Tooltip("The maximum force that can be applied to a pedestrian")]
    [SerializeField] float maxKnockback = 25f;

    private Vector3 startingPosition;
    // track which patrol point we're moving toward
    private bool movingToB = true;
    private bool orderTaken = false;
    private bool foodReady = false;
    private bool isOrderCompleted = false;
    private enum CustomerState { WaitingForOrder, Cooking, Returning, Fading, Done }
    private CustomerState currentState;
    private Vector3 previousPosition;
    private AnimatorController animController;
    private Vector3 destination; // the destination of the customer if knocked back

    [SerializeField] Color rangeColor;
    [SerializeField] Color inRangeColor;
    [SerializeField] Color cookingColor;
    [SerializeField] Color completeColor;


    void Awake()
    {
        customerType = (CustomerType)(Random.Range(0, 4));
    }
    void Start()
    {
        fadeTimer = fadeOutDuration;
        currentState = CustomerState.WaitingForOrder;
        startingPosition = transform.position;
        // if the default east-west path is blocked, switch to north-south
        DetectAndSwitchPatrolAxis();
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
        // set animation speed to 0.5
        animController.changeAnimSpeed(0.4f);
        rb = GetComponent<Rigidbody>();

        hungrySign.SetActive(false);
    }

    void Update()
    {
        if (currentState != CustomerState.Fading && currentState == CustomerState.Cooking && foodReady)
        {
            bool inRange = detectionRange.GetComponent<CustomerRange>().playerInRange;
            rangeIndicatorSprite.color = inRange
                ? inRangeColor
                : rangeColor;
        }

        // update the state of the customer
        switch (currentState)
        {
            case CustomerState.WaitingForOrder:
                if (detectionRange.GetComponent<CustomerRange>().playerInRange && Input.GetKeyDown(KeyCode.E) && GameManager.Instance.getPlayer().GetOil() >= 20)
                {
                    GameManager.Instance.TakeOrder(this);
                    currentState = CustomerState.Cooking;
                    orderTaken = true;
                    switch (customerType)
                    {
                        case CustomerType.Beetle:
                            AudioManager.Instance.PlaySound("sfx_order_Beetle" + Random.Range(1, 3));
                            break;
                        case CustomerType.Grasshopper:
                            AudioManager.Instance.PlaySound("sfx_order_Grasshopper" + Random.Range(1, 3));
                            break;
                        case CustomerType.Ladybug:
                            AudioManager.Instance.PlaySound("sfx_order_Ladybug" + Random.Range(1, 3));
                            break;
                        case CustomerType.Rolypoly:
                            AudioManager.Instance.PlaySound("sfx_order_Rolypoly" + Random.Range(1, 3));
                            break;
                    }
                    rangeIndicatorSprite.color = cookingColor;
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
                        GameManager.Instance.FailOrder(this);
                        AudioManager.Instance.PlaySound("sfx_Anger");
                        rangeIndicatorSprite.color = Color.clear;
                        hungrySign.SetActive(false);
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
    }

    void FixedUpdate()
    {
        MoveCustomer();
    }

    /// <summary>
    /// Completes the order for the customer. 
    /// For now, it will change the customer's material to blue.
    /// It will call the GameManager to update the game status.
    /// </summary>
    public void ReceiveOrder()
    {
        currentState = CustomerState.Fading;
        GameManager.Instance.CompleteOrder(this);
        isOrderCompleted = true;
        rangeIndicatorSprite.color = Color.clear;
        hungrySign.SetActive(false);
        switch (customerType)
        {
            case CustomerType.Beetle:
                AudioManager.Instance.PlaySound("sfx_complete_Beetle" + Random.Range(1, 3));
                break;
            case CustomerType.Grasshopper:
                AudioManager.Instance.PlaySound("sfx_complete_Grasshopper" + Random.Range(1, 3));
                break;
            case CustomerType.Ladybug:
                AudioManager.Instance.PlaySound("sfx_complete_Ladybug" + Random.Range(1, 3));
                break;
            case CustomerType.Rolypoly:
                AudioManager.Instance.PlaySound("sfx_complete_Rolypoly" + Random.Range(1, 3));
                break;
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
                float knockbackForce = Mathf.Abs(magnitude) < maxKnockback ? magnitude :
                    magnitude < 0 ? -maxKnockback : maxKnockback;

                knockbackDirection += Vector3.up * 0.2f;
                knockbackDirection.Normalize();

                rb.isKinematic = false;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

                isKnockedBack = true;
                isInvincible = true;
                knockbackTimer = knockbackCooldown; // Start knockback cooldown

                // play hurt sound
                switch (customerType)
                {
                    case CustomerType.Beetle:
                        AudioManager.Instance.PlaySound("sfx_hurt_Beetle" + Random.Range(1, 3));
                        break;
                    case CustomerType.Grasshopper:
                        AudioManager.Instance.PlaySound("sfx_hurt_Grasshopper" + Random.Range(1, 3));
                        break;
                    case CustomerType.Ladybug:
                        AudioManager.Instance.PlaySound("sfx_hurt_Ladybug" + Random.Range(1, 3));
                        break;
                    case CustomerType.Rolypoly:
                        AudioManager.Instance.PlaySound("sfx_hurt_Rolypoly" + Random.Range(1, 3));
                        break;
                }

                Debug.Log("Crash! Pedestrian knocked back with force: " + knockbackForce);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // draw lines between points
        Gizmos.color = Color.cyan;
        Vector3 worldA = transform.position + patrolPointA;
        Vector3 worldB = transform.position + patrolPointB;
        Gizmos.DrawLine(worldA, worldB);
        Gizmos.DrawSphere(worldA, 0.1f);
        Gizmos.DrawSphere(worldB, 0.1f);

        // make the points draggable
        UnityEditor.EditorGUI.BeginChangeCheck();
        Vector3 newWorldA = UnityEditor.Handles.PositionHandle(worldA, Quaternion.identity);
        Vector3 newWorldB = UnityEditor.Handles.PositionHandle(worldB, Quaternion.identity);
        if (UnityEditor.EditorGUI.EndChangeCheck())
        {
            // record undo so changes can be undone
            UnityEditor.Undo.RecordObject(this, "Move Patrol Point");
            patrolPointA = newWorldA - transform.position;
            patrolPointB = newWorldB - transform.position;
        }
    }
#endif

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

    private void HandleFading()
    {
        fadeTimer -= Time.deltaTime;
        float normalizedTime = fadeTimer / fadeOutDuration;
        if (normalizedTime <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            spriteRenderer.color = new Color(255, 255, 255, normalizedTime);
            Color hungryColor = hungrySign.GetComponent<SpriteRenderer>().color;
        }
    }
    private Vector3 targetPosition;

    private void MoveCustomer()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.fixedDeltaTime;

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
            // compute world-space patrol endpoints
            Vector3 worldA = startingPosition + patrolPointA;
            Vector3 worldB = startingPosition + patrolPointB;
            // choose current target
            Vector3 target = movingToB ? worldB : worldA;

            // if far from target, push toward it
            if (Vector3.Distance(transform.position, target) > 0.1f)
            {
                // move toward target
                rb.MovePosition(Vector3.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime));
                SetAnimationDirection(target - transform.position);
                previousPosition = transform.position;
            }
            else
            {
                // reached endpoint: flip direction
                movingToB = !movingToB;
                // (Optional) Instantly flip animation direction at endpoint
                Vector3 newTarget = movingToB ? worldB : worldA;
                SetAnimationDirection(newTarget - transform.position);
            }
        }
    }


    /// <summary>
    /// Check the current patrol axis for obstacles; if any are detected, switch to the perpendicular axis.
    /// </summary>
    private void DetectAndSwitchPatrolAxis()
    {
        // compute world-space endpoints
        Vector3 worldA = startingPosition + patrolPointA;
        Vector3 worldB = startingPosition + patrolPointB;
        Vector3 dir = (worldB - worldA).normalized;
        float dist = Vector3.Distance(worldA, worldB);

        // if there's any obstacle between A and B
        if (Physics.Raycast(worldA, dir, dist, obstacleMask))
        {
            // swap to north-south: use same distance but along Z instead of X
            float half = dist / 2f;
            patrolPointA = new Vector3(0f, 0f, half);
            patrolPointB = new Vector3(0f, 0f, -half);
            // reset moving direction
            movingToB = true;
        }
    }

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