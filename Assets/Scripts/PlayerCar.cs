using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    //Speed that car turns 
    [SerializeField] float turnSpeed = 5;
    //Target Rotation car should aim for 
    Quaternion targetRotation;
    Rigidbody _rigidbody;

    //Allows for script to interact with Rigidbody component
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    //Acceleration for car 
    [SerializeField] float acceleration = 8;

    // Update the target rotation based on the mouse position (for now?)
    void Update()
    {
        setRotationPoint();

    }

    //Calculates the target direction for car to rotate too 
    private void setRotationPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(0, rotationAngle, 0);
        }
    }
    // Calculating the acceleration input based on mouse button presses
    // If the left mouse button (button 0) is pressed, accelerationInput is positive
    // If the right mouse button (button 1) is pressed, accelerationInput is negative
    private void FixedUpdate()
    {
        float accelerationInput = acceleration * (Input.GetMouseButton(0) ? 1 : Input.GetMouseButton(1) ? -1 : 0) * Time.fixedDeltaTime;
        _rigidbody.AddRelativeForce(Vector3.forward * accelerationInput);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

    }
}
