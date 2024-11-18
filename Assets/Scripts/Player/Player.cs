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

    [Header("Input Key Codes")]
    [Tooltip("Left button for nitro")]
    [SerializeField] private KeyCode nitro1 = KeyCode.LeftShift;
    [Tooltip("Right button for nitro")]
    [SerializeField] private KeyCode nitro2 = KeyCode.RightShift;
    [Tooltip("Button for drifting")]
    [SerializeField] private KeyCode drift = KeyCode.Space;


    private Rigidbody rb;
    private Vector3 lastVelocity;
    private float oil;
    private float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
    private int curAngle = 0;
    private bool movingForward = false;
    private bool isDead = false;
    private bool isInvincible = false;
    private float turnDelay = 0;
    private float turnRate = 0.25f;
    private float driftTime = 0;

    // Input booleans
    private bool pressForward;
    private bool pressBackward;
    private bool pressRight;
    private bool pressLeft;
    private bool pressNitro;
    private bool pressDrift;

    //New added private variables 

    private List<Customer> customers;

    [SerializeField] float minCrashSpeed;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.eulerAngles = new Vector3(0, angles[curAngle], 0);
        oil = maxOil;
        customers = GameManager.Instance.GetCustomers();
    }

    void FixedUpdate()
    {
        lastVelocity = rb.velocity;
        Drive();
        Nitro();
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
        if (Input.GetKey(nitro1) || Input.GetKey(nitro2))
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
        if (!pressRight && !pressLeft)
        {
            turnDelay = 0;
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
            AudioManager.Instance.Play("sfx_Boost");
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
    // Player can hold the spacebar to brake and turn while braking to drift
    void Drift()
    {
        if (pressDrift && (pressLeft || pressRight))
        {
            driftTime = Time.time + 0.5f;
        }
        if (!pressDrift && driftTime > Time.time && movingForward)
        {
            rb.AddRelativeForce(Vector3.forward * 50);
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
    public void TakeDamage()
    {
        if (isInvincible) return;
        health --;
        int random = Random.Range(0, 2);
        switch(random)
        {
            case 0:
                AudioManager.Instance.Play("sfx_Crash1");
                break;
            case 1:
                AudioManager.Instance.Play("sfx_Crash2");
                break;
        }

        if (health <= 0)
        {
            health = 0;
            isDead = true;
            GameManager.Instance.LoseGame();
            return;
        }
        StartCoroutine(BecomeInvincible());
    }

    // Public methods to access oil and maxOil
    public float GetOil()
    {
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

    public void Crash(Vector3 speedVector, Vector3 position)
    {
        if(speedVector.magnitude >= minCrashSpeed){
            Debug.Log("crashed into player");
            TakeDamage();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<ICrashable>() != null)
        {
            other.gameObject.GetComponent<ICrashable>().Crash(rb.velocity, transform.position);
        }
        float curSpeed = lastVelocity.magnitude;
        Vector3 direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);
        GetComponent<Rigidbody>().velocity = direction * Mathf.Max(curSpeed, 2f);
    }
}
