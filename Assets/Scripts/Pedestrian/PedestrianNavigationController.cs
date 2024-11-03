using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianNavigationController : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public float rotationSpeed = 120;
    public float stopDistance = 2.0f;
    public Vector3 destination;
    public bool hasReachedDestination = false;

    // Update is called once per frame
    void Update()
    {
        MoveToDestination();
    }

    public void MoveToDestination()
    {
        if (Vector3.Distance(transform.position, destination) > stopDistance)
        {
            hasReachedDestination = false;

            // Rotate towards destination
            Vector3 direction = destination - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

            // Move towards destination
            transform.Translate(0, 0, movementSpeed * Time.deltaTime);
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
}
