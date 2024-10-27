using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

// This script handles the inputs and manages the oil and cooking timers for the player
public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    float oil;
    [SerializeField] float maxOil;

    //Player health var
    [SerializeField] float health;
    public float oilConsumptionRate = 1f; // Oil consumption rate per second
    public float cookingTime = 60f; // Total cooking time in seconds

    [SerializeField] private KeyCode nitro = KeyCode.LeftShift;

    private Rigidbody rb;
    private float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
    private int curAngle = 0;

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

    // The player can press up and down arrows to drive forwards and backwards.
    void Drive()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddRelativeForce(Vector3.forward * speed * 10);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddRelativeForce(-Vector3.forward * speed * 10);
        }
    }

    // The player can use left or right arrow to turn to the next of 8 possible directions.
    void Turn()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
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
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
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
                Debug.Log("Oil depleted!");
            }

            if (cookingTimer <= 0)
            {
                cookingTimer = 0;
                Debug.Log("Cooking complete!");
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


    // Public methods to access oil and maxOil
    public float GetOil()
    {
        Debug.Log("Oil count " + oil);
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
