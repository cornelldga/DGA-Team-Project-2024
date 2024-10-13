using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float speed;
    public float oil;

    private Rigidbody rb;
    private float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315};
    private int curAngle = 0;
    private bool turnPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.eulerAngles = new Vector3(0, angles[curAngle], 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Drive();
        Turn();
        Nitro();
    }

    void Nitro()
    {
        if (Input.GetKey(KeyCode.LeftShift) && oil > 0)
        {
            rb.AddRelativeForce(Vector3.forward * 100);
            oil--;
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
        else if (!Input.GetKey(KeyCode.RightArrow)  && !Input.GetKey(KeyCode.LeftArrow) && turnPressed) 
        {
            turnPressed = false;
        }
    }
}
