using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.UIElements;
using UnityEngine;

// This script handles the inputs and manages the oil and cooking timers for the player
public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    float oil;
    [SerializeField] float maxOil;

    //Player health var
    [SerializeField] float health;
    [SerializeField] private float invincibilityDuration;
    [SerializeField] private float invincibilityDeltaTime;
    public float oilConsumptionRate = 1f; // Oil consumption rate per second
    public float cookingTime = 60f; // Total cooking time in seconds

    [SerializeField] private KeyCode nitro = KeyCode.LeftShift;
    [SerializeField] private KeyCode drift = KeyCode.Space;

    private Rigidbody rb;
    private float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
    private int curAngle = 0;
    private bool movingForward = false;
    private bool movingBackward = false;
    private bool isDead = false;
    private bool isInvincible = false;

    //New added private variables 

    private float cookingTimer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.eulerAngles = new Vector3(0, angles[curAngle], 0);
        cookingTimer = cookingTime;
        InvokeRepeating(nameof(HandleOrders), 1, 1);
        oil = maxOil;
    }

    void FixedUpdate()
    {
        Drive();
        Nitro();
        Cook();
        Steer();
        Drift();
    }

    private void Update()
    {
        Turn();
    }

    // While holding shift, the player uses oil to nitro boost.
    void Nitro()
    {
        if (Input.GetKey(nitro) && oil > 0)
        {
            rb.AddRelativeForce(Vector3.forward * 50);
            oil--;
        }
    }

    // The player can press W and S to drive forwards and backwards.
    void Drive()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(Vector3.forward * speed * 10);
            movingForward = true;
            movingBackward = false;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddRelativeForce(-Vector3.forward * speed * 10);
            movingForward = false;
            movingBackward = true;
        }
        else
        {
            movingForward = false;
            movingBackward = false;
        }
    }

    // The player can use A and D to turn to the next of 8 possible directions.
    void Turn()
    {
        if (!Input.GetKey(drift))
        {
            if (Input.GetKeyDown(KeyCode.D))
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
            else if (Input.GetKeyDown(KeyCode.A))
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
        else if (Input.GetKey(drift))
        {
            if (Input.GetKeyDown(KeyCode.D))
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
            else if (Input.GetKeyDown(KeyCode.A))
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
    // Player can hold right and left arrows to steer left and right
    void Steer()
    {
        if (rb.velocity.magnitude > 0.01f && (movingForward || movingBackward))
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddRelativeForce(Vector3.right * 10);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddRelativeForce(Vector3.left * 10);
            }
        }
    }

    // Player can hold the spacebar to brake and turn while braking to drift
    void Drift()
    {
        if (Input.GetKey(drift))
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
    /// <param name="customers"></param>
    public void HandleOrders(List<Customer> customers)
    {
        Debug.Log("HandleOrders called. Number of customers: " + customers.Count);
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
}
