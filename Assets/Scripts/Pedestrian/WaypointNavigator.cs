using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(PedestrianNavigationController))]
public class WaypointNavigator : MonoBehaviour
{
    PedestrianNavigationController navigationController;
    public Waypoint currentWaypoint;

    private void Awake()
    {
        navigationController = GetComponent<PedestrianNavigationController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        navigationController.SetDestination(currentWaypoint.GetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (navigationController.hasReachedDestination)
        {
            SetNextDestination();
        }
    }

    private void SetNextDestination()
    {
        if (currentWaypoint.nextWaypoint != null)
        {
            currentWaypoint = currentWaypoint.nextWaypoint;
            navigationController.SetDestination(currentWaypoint.GetPosition());
        }
        else { }
    }
}

