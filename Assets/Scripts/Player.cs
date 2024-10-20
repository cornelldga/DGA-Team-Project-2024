using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float oil;
    public float oilConsumptionRate = 1f; // Oil consumption rate per second
    public float cookingTime = 60f; // Total cooking time in seconds

    private Rigidbody rb;
    private float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
    private int curAngle = 0;
    private bool turnPressed = false;
    private bool oilOut = false;
    private float oilTimer = 10f;

    //New added private variables 
    private float timeOilOut;

    private float cookingTimer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.eulerAngles = new Vector3(0, angles[curAngle], 0);
        cookingTimer = cookingTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Drive();
        Turn();
        Nitro();
        Cook();
    }

    void Nitro()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !oilOut)
        {
            rb.AddRelativeForce(Vector3.forward * 50);
            oil--;
            if (oil == 0)
            {
                oilOut = true;
                timeOilOut = Time.time;
            }
        }
        if (oilOut && Time.time >= timeOilOut + oilTimer)
        {
            oil = 100;
            oilOut = false;
        }
    }

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

    void Turn()
    {
        if (Input.GetKey(KeyCode.RightArrow) && !turnPressed)
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
            turnPressed = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !turnPressed)
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
            turnPressed = true;
        }
        else if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && turnPressed)
        {
            turnPressed = false;
        }
    }

    //Cook method continuously decreases the cookingTimer and oil 
    void Cook()
    {
        if (cookingTimer > 0)
        {
            cookingTimer -= Time.deltaTime;
            oil -= oilConsumptionRate * Time.deltaTime;

            if (oil <= 0)
            {
                oil = 0;
                oilOut = true;
                Debug.Log("Oil depleted!");
            }

            if (cookingTimer <= 0)
            {
                cookingTimer = 0;
                Debug.Log("Cooking complete!");
            }
        }
    }
}
