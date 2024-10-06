using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float speed;
    public float turnSpeed;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddRelativeForce(Vector3.forward * speed * 10);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddRelativeForce(-Vector3.forward * speed * 10);
        }
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        localVelocity.x = 0;
        rb.velocity = transform.TransformDirection(localVelocity);
        print(rb.velocity);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddTorque(Vector3.up * turnSpeed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddTorque(-Vector3.up * turnSpeed);
        }
    }
}
