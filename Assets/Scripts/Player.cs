using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.UIElements;
using UnityEngine;

// This script handles the inputs and manages the oil and cooking timers for the player
public class Player : MonoBehaviour, ICrashable
{
    [Tooltip("Player moves forward by a product of this speed")]
    [SerializeField] float speed = 4f;

    //Player health var
    [Tooltip("Number of health points that player starts with")]
    [SerializeField] float health = 3f;

    [Header("Invincibility Frames")]
    [Tooltip("Number of seconds player is invincible after being hit")]
    [SerializeField] private float invincibilityDuration = 1.5f;
    [Tooltip("Splits invincibility duration into individual frames to inform blinking")]
    [SerializeField] private float invincibilityDeltaTime = 0.15f;

    [Header("Oil Values")]
    [Tooltip("Max amount of oil that player starts with")]
    [SerializeField] float maxOil = 100f;
    [Tooltip("Oil consumption rate per second")]
    [SerializeField] float oilConsumptionRate = 1f;
    [Tooltip("Total cooking time in seconds")]
    [SerializeField] float cookingTime = 60f;

    [Header("Input Key Codes")]
    [Tooltip("Button for nitro")]
    [SerializeField] private KeyCode nitro = KeyCode.LeftShift;
    [Tooltip("Button for drifting")]
    [SerializeField] private KeyCode drift = KeyCode.Space;

    private Rigidbody rb;
    private float oil;
    private float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
    private int curAngle = 0;
    private bool movingForward = false;
    private bool isDead = false;
    private bool isInvincible = false;
    private float turnDelay = 0;
    private float turnRate = 0.25f;

    // Input booleans
    private bool pressForward;
    private bool pressBackward;
    private bool pressRight;
    private bool pressLeft;
    private bool pressNitro;
    private bool pressDrift;

    //New added private variables 

    private float cookingTimer;

    private List<Customer> customers;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.eulerAngles = new Vector3(0, angles[curAngle], 0);
        cookingTimer = cookingTime;
        oil = maxOil;
        customers = GameManager.Instance.GetCustomers();
    }

    void FixedUpdate()
    {
        Drive();
        Nitro();
        Cook();
        Drift();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            pressForward = true;
        }
        else
        {
            pressForward = false;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            pressBackward = true;
        }
        else
        {
            pressBackward = false;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            pressRight = true;
        }
        else
        {
            pressRight = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            pressLeft = true;
        }
        else
        {
            pressLeft = false;
        }
        if (Input.GetKey(nitro))
        {
            pressNitro = true;
        }
        else
        {
            pressNitro = false;
        }
        if (Input.GetKey(drift))
        {
            pressDrift = true;
        }
        else
        {
            pressDrift = false;
        }
        if ((pressRight || pressLeft) && Time.time > turnDelay)
        { 
            turnDelay = Time.time + turnRate;
            Turn(); 
        }
        if (customers.Count != 0)
        {
            HandleOrders();
        }
    }

    // While holding shift, the player uses oil to nitro boost.
    void Nitro()
    {
        if (pressNitro && oil > 0)
        {
            rb.AddRelativeForce(Vector3.forward * 50);
            oil--;
        }
    }

    // The player can press W and S to drive forwards and backwards.
    void Drive()
    {
        if (pressForward)
        {
            rb.AddRelativeForce(Vector3.forward * speed * 10);
            movingForward = true;
        }
        else if (pressBackward)
        {
            rb.AddRelativeForce(-Vector3.forward * speed * 10);
            movingForward = false;
        }
        else
        {
            movingForward = false;
        }
    }

    // The player can use A and D to turn to the next of 8 possible directions.
    void Turn()
    {
        if (!pressDrift)
        {
            if (pressRight)
            {
                if (curAngle == 7)
                {
                    curAngle = 0;
                }
                else
                {
                    curAngle++;
                }
                transform.eulerAngles = new Vector3(0, angles[curAngle], 0);
            }
            else if (pressLeft)
            {
                if (curAngle == 0)
                {
                    curAngle = 7;
                }
                else
                {
                    curAngle--;
                }
                transform.eulerAngles = new Vector3(0, angles[curAngle], 0);
            }
        }
        else if (pressDrift)
        {
            if (pressRight)
            {
                if (curAngle == 7)
                {
                    curAngle = 1;
                }
                else if (curAngle == 6)
                {
                    curAngle = 0;
                }
                else
                {
                    curAngle += 2;
                }
                transform.eulerAngles = new Vector3(0, angles[curAngle], 0);
            }
            else if (pressLeft)
            {
                if (curAngle == 0)
                {
                    curAngle = 6;
                }
                else if (curAngle == 1)
                {
                    curAngle = 7;
                }
                else
                {
                    curAngle -= 2;
                }
                transform.eulerAngles = new Vector3(0, angles[curAngle], 0);
            }
        }
    }

    //Cook method continuously decreases the cookingTimer and oil (uncommented in order to operate oilbar)
    void Cook()
    {
        if (cookingTimer > 0)
        {
            cookingTimer -= Time.deltaTime;
            oil -= oilConsumptionRate * Time.deltaTime;

            if (oil <= 0)
            {
                oil = 0;
            }

            if (cookingTimer <= 0)
            {
                cookingTimer = 0;
            }
        }
    }

    // Player can hold the spacebar to brake and turn while braking to drift
    void Drift()
    {
        if (pressDrift)
        {
            if (movingForward && rb.velocity.magnitude > 0.01f)
            {
                rb.AddRelativeForce(-Vector3.forward * speed * 10);
            }
        }
    }

    /// <summary>
    /// Called every frame to check through the list of customers and decrease cooking time and oil
    /// </summary>
    void HandleOrders()
    {
        foreach (Customer customer in customers)
        {
            if (customer.cookTime > 0 && oil > 0)
            {
                customer.cookTime -= Time.deltaTime;
                oil -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Adds oil to the player's vehichle, never exceeding the maxOil amount
    /// </summary>
    public void AddOil(int oilAmount)
    {
        if (oil <= maxOil)
        {
            oil = oil + oilAmount; //Changed functionality old function was updating oil properly
            Debug.Log("Oil replenished: " + oil);
        }
    }

    /// <summary>
    /// Counts amount of time that player should be invincible after taking damage
    /// </summary>
    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;

        for (float i = 0; i < invincibilityDuration; i += invincibilityDeltaTime)
        {
            yield return new WaitForSeconds(invincibilityDeltaTime);
        }

        isInvincible = false;
    }

    // Decreases health of player and inititates invulnerability frames
    public void TakeDamage(int amount)
    {
        if (isInvincible) return;
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            isDead = true;
            GameManager.Instance.LoseGame();
            Debug.Log("player is dead, 0 health remaining");
            return;
        }
        StartCoroutine(BecomeInvincible());
    }

    // Public methods to access oil and maxOil
    public float GetOil()
    {
        //Debug.Log("Oil count " + oil);
        return oil;

    }

    public float GetMaxOil()
    {
        return maxOil;
    }

    public float GetHealth()
    {
        return health;
    }

    public void Crash(Vector3 speedVector)
    {
    }
}
