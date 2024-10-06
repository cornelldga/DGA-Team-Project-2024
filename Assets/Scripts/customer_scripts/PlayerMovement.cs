using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 360f;
    public float accelerationMultiplier = 2f; // Acceleration multiplier when Shift is pressed
    public float driftFactor = 0.9f; // Controls the intensity of the drift

    private Vector3 moveDirection; // Current movement direction

    void Update()
    {
        // Check if Shift is held down to apply acceleration
        float currentMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * accelerationMultiplier : moveSpeed;

        // Get input axes
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Calculate movement and rotation amounts
        float moveAmount = moveInput * currentMoveSpeed * Time.deltaTime;
        float turnAmount = turnInput * turnSpeed * Time.deltaTime;

        // Apply rotation
        transform.Rotate(0, turnAmount, 0);

        // Determine the forward direction
        Vector3 forward = transform.forward;

        if (Input.GetKey(KeyCode.Space))
        {
            // Apply drifting effect
            moveDirection = Vector3.Lerp(moveDirection, forward * moveAmount, (1 - driftFactor) * Time.deltaTime);
        }
        else
        {
            // Normal movement
            moveDirection = forward * moveAmount;
        }

        // Move the player
        transform.Translate(moveDirection, Space.World);
    }
}
