using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the movement of a pedestrian.
/// </summary>
/// <remarks>
/// Without getting crashed, the pedestrian will move towards the destination. When it reaches the destination, it selects a new one based on 1) its walking direction and 2) if there is a branch from the current waypoint. 
/// If the pedestrian collides with the player, it will be knocked back based on the player's speed and ped-play direction.
/// </remarks>
public class PedestrianNavigationController : MonoBehaviour, ICrashable
{
    public float movementSpeed = 1.0f;
    public float rotationSpeed = 150;
    public float stopDistance = 0.05f;
    public Vector3 destination;
    public bool hasReachedDestination = false;
    public float knockbackScale = 3f;
    [Tooltip("The maximum force that can be applied to a pedestrian")]
    [SerializeField] float maxKnockback;

    bool isKnockedBack = false;
    bool isInvincible = false;
    float invincibilityDuration = 3.0f; // Duration of invincibility in seconds
    float invincibilityTimer = 0f;
    float knockbackCooldown = 0.5f; // Delay before rechecking for kinematic state
    float knockbackTimer = 0f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Ensure the Rigidbody is kinematic initially
        Debug.Log("PedestrianNavigationController Start, position: " + transform.position);
    }

    void Update()
    {
        Debug.Log("PedestrianNavigationController Update, position: " + transform.position + " name: " + gameObject.name);
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f && rb.velocity.magnitude < 0.1f)
            {
                rb.isKinematic = true; // Re-enable kinematic Rigidbody after delay
                isKnockedBack = false;
            }
        }
        else
        {
            if (destination != null)
            {
                MoveToDestination();
            }
            else
            {
                Debug.Log("Destination is null for pedestrian: " + gameObject.name);
            }

            if (isInvincible)
            {
                invincibilityTimer -= Time.deltaTime;
                if (invincibilityTimer <= 0f)
                {
                    isInvincible = false;
                }
            }
        }
    }

    public void MoveToDestination()
    {
        if (Vector3.Distance(transform.position, destination) > stopDistance)
        {
            hasReachedDestination = false;

            // Rotate towards destination
            Vector3 direction = destination - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Move towards destination
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
        else
        {
            hasReachedDestination = true;
        }
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        hasReachedDestination = false;
    }

    public void Crash(Vector3 speedVector, Vector3 position)
    {
        if (!isInvincible)
        {
            float magnitude = speedVector.magnitude;
            float knockbackThreshold = 1.0f;

            if (magnitude > knockbackThreshold)
            {
                Vector3 knockbackDirection = (transform.position - position).normalized;
                float knockbackForce = Mathf.Min(magnitude * knockbackScale, maxKnockback);

                knockbackDirection += Vector3.up * 0.2f;
                knockbackDirection.Normalize();

                rb.isKinematic = false;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

                isKnockedBack = true;
                isInvincible = true;
                invincibilityTimer = invincibilityDuration;
                knockbackTimer = knockbackCooldown; // Start knockback cooldown
            }
        }
    }
}