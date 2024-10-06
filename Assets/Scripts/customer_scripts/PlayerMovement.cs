using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 360f;
    public float accelerationMultiplier = 2f;
    public float driftFactor = 0.95f;
    public float lateralDriftFactor = 0.8f;
    public float brakingFactor = 0.95f;

    private Vector3 velocity;
    private float currentSpeed;

    void Update()
    {
        // Get input axes
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Calculate rotation
        float turnAmount = turnInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0, turnAmount, 0);

        // Calculate target speed
        float targetSpeed = moveInput * moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            targetSpeed *= accelerationMultiplier;
        }

        // Apply acceleration or deceleration
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 5f);

        // Calculate movement direction
        Vector3 moveDirection = transform.forward * currentSpeed;

        // Apply drifting
        if (Input.GetKey(KeyCode.Space))
        {
            // Maintain more of the previous velocity for a smoother drift
            velocity = Vector3.Lerp(velocity, moveDirection, (1 - driftFactor) * Time.deltaTime);

            // Apply lateral drift
            Vector3 lateralVelocity = Vector3.Project(velocity, transform.right);
            velocity += lateralVelocity * lateralDriftFactor;
        }
        else
        {
            // Normal movement with smoother transitions
            velocity = Vector3.Lerp(velocity, moveDirection, Time.deltaTime * 3f);
        }

        // Apply braking when no input is given
        if (moveInput == 0 && !Input.GetKey(KeyCode.Space))
        {
            velocity *= brakingFactor;
        }

        // Move the player
        transform.position += velocity * Time.deltaTime;
    }
}