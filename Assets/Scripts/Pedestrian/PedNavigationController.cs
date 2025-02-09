using System;
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
[RequireComponent(typeof(Rigidbody))]
public class PedNavigationController : MonoBehaviour, ICrashable
{
    public float movementSpeed = 1.0f;
    [SerializeField] float stopDistance = 0.05f;
    [SerializeField] Vector3 destination;
    public bool hasReachedDestination = false;
    [SerializeField] float knockbackScale = 3f;
    [Tooltip("The maximum force that can be applied to a pedestrian")]
    [SerializeField] float maxKnockback;

    bool isKnockedBack = false;
    bool isInvincible = false;
    float invincibilityDuration = 3.0f; // Duration of invincibility in seconds
    float invincibilityTimer = 0f;
    float knockbackCooldown = 0.5f; // Delay before rechecking for kinematic state
    float knockbackTimer = 0f;
    Rigidbody rb;

    [SerializeField] AnimatorController animController;
    private Vector3 previousPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Ensure the Rigidbody is kinematic initially
        previousPosition = transform.position;
    }

    void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            // set animator
            animController.SetMovingNorth(false);
            animController.SetMovingSouth(false);
            animController.SetMovingEast(false);
            animController.SetMovingWest(false);
            // check self direction
            Vector3 direction = transform.position - previousPosition;
            // check if facing right or left
            if (direction.x > 0)
            {
                animController.SetCrashRight(true);
            }
            else if (direction.x < 0)
            {
                animController.SetCrashLeft(true);
            }

            if (knockbackTimer <= 0f && rb.velocity.magnitude < 0.1f)
            {
                rb.isKinematic = true; // Re-enable kinematic Rigidbody after delay
                isKnockedBack = false;
                // reset animator
                animController.SetCrashRight(false);
                animController.SetCrashLeft(false);
                Vector3 distance = destination - transform.position;
                SetAnimationDirection(distance);
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

        // line up the rotation angle with the camera
        animController.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, 0);

        previousPosition = transform.position;
    }

    public void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, destination) > stopDistance)
        {
            hasReachedDestination = false;
        }
        else
        {
            hasReachedDestination = true;
        }
    }

    public void SetDestination(Vector3 destination)
    {
        Vector3 oldDestination = this.destination;
        this.destination = destination;
        hasReachedDestination = false;

        if (!animController && !animController.anim) return;
        Vector3 distance = destination - oldDestination;
        SetAnimationDirection(distance);
        // Debug.Log("Pedestrian setting destination to: " + destination + " and moving directions (WENS): " + isWalkingWest + " " + isWalkingEast + " " + isWalkingNorth + " " + isWalkingSouth);
    }

    // check if the pedestrian is going to move north, south, east or west, and set the corresponding boolean in the animator.
    // note that there might be a slight shift in other directions due to randomization.
    private void SetAnimationDirection(Vector3 distance)
    {
        bool isWalkingWest = distance.x < 0 && Mathf.Abs(distance.x) > Mathf.Abs(distance.z);
        bool isWalkingEast = distance.x > 0 && Mathf.Abs(distance.x) > Mathf.Abs(distance.z);
        bool isWalkingSouth = distance.z < 0 && Mathf.Abs(distance.z) > Mathf.Abs(distance.x);
        bool isWalkingNorth = distance.z > 0 && Mathf.Abs(distance.z) > Mathf.Abs(distance.x);
        // set all to false
        animController.SetMovingWest(false);
        animController.SetMovingEast(false);
        animController.SetMovingNorth(false);
        animController.SetMovingSouth(false);
        if (isWalkingWest)
        {
            animController.SetMovingWest(true);
        }
        else if (isWalkingEast)
        {
            animController.SetMovingEast(true);
        }
        else if (isWalkingNorth)
        {
            animController.SetMovingNorth(true);
        }
        else if (isWalkingSouth)
        {
            animController.SetMovingSouth(true);
        }
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