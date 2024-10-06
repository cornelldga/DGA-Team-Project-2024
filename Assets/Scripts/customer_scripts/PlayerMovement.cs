using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 360f;
    public float accelerationMultiplier = 2f; 
    public float driftFactor = 0.9f; 

    private Vector3 moveDirection;

    void Update()
    {
        float currentMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * accelerationMultiplier : moveSpeed;

        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        float moveAmount = moveInput * currentMoveSpeed * Time.deltaTime;
        float turnAmount = turnInput * turnSpeed * Time.deltaTime;

        transform.Rotate(0, turnAmount, 0);

        Vector3 forward = transform.forward;

        if (Input.GetKey(KeyCode.Space))
        {
            moveDirection = Vector3.Lerp(moveDirection, forward * moveAmount, (1 - driftFactor) * Time.deltaTime);
        }
        else
        {
            moveDirection = forward * moveAmount;
        }

        transform.Translate(moveDirection, Space.World);
    }
}
