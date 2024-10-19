using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles the inputs and manages the oil and cooking timers for the player
public class Player : MonoBehaviour
{
    public float speed;
    public float oil;

    [SerializeField] private KeyCode nitro = KeyCode.LeftShift;

    private Rigidbody rb;
    private float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
    private int curAngle = 0;
    private bool oilOut = false;
    private float oilTimer = 10f;
    private float timeOilOut;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.eulerAngles = new Vector3(0, angles[curAngle], 0);
    }

    void FixedUpdate()
    {
        Drive();
        Nitro();
    }

    private void Update()
    {
        Turn();
    }

    // While holding shift, the player uses oil to nitro boost.
    void Nitro()
    {
        if (Input.GetKey(nitro) && !oilOut)
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
}
