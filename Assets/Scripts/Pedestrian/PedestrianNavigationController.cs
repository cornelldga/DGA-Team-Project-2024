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
public class PedestrianNavigationController : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public float rotationSpeed = 150;
    public float stopDistance = 0.05f;
    public Vector3 destination;
    public bool hasReachedDestination = false;
    public float knockbackScale = 3f;

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
    }

    void Update()
    {
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
            MoveToDestination();

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

    void OnCollisionEnter(Collision collision)
    {
        Player player = GameManager.Instance.getPlayer();
        if (collision.gameObject.CompareTag("Player") && !isInvincible)
        {
            Debug.Log("[PedestrianNavigationController] Collision with player detected!");
            Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();

            float playerSpeed = playerRigidbody.velocity.magnitude;
            float knockbackThreshold = 1.0f;

            if (playerSpeed > knockbackThreshold)
            {
                Vector3 knockbackDirection = (transform.position - player.transform.position).normalized;
                float knockbackForce = playerSpeed * knockbackScale;

                knockbackDirection += Vector3.up * 0.2f;
                knockbackDirection.Normalize();

                rb.isKinematic = false;
                Debug.Log("[PedestrianNavigationController] Player speed: " + playerSpeed + " knockback force: " + knockbackForce * knockbackDirection);
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

                isKnockedBack = true;
                isInvincible = true;
                invincibilityTimer = invincibilityDuration;
                knockbackTimer = knockbackCooldown; // Start knockback cooldown
            }
        }
    }
}