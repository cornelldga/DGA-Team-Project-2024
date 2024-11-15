using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// This class navigates the pedestrian through the waypoints.
/// Upon arriving at a waypoint, the pedestrian selects the next waypoint based on its walking direction and the branch ratio. 
/// </summary>
[RequireComponent(typeof(PedestrianNavigationController))]
public class WaypointNavigator : MonoBehaviour
{
    PedestrianNavigationController navigationController;
    public Waypoint currentWaypoint;
    int direction;

    private void Awake()
    {
        navigationController = GetComponent<PedestrianNavigationController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        int draw = Random.Range(0, 2);
        direction = Mathf.RoundToInt(draw);
        navigationController.SetDestination(currentWaypoint.GetPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (navigationController.hasReachedDestination)
        {
            SelectNextDestination();
            navigationController.SetDestination(currentWaypoint.GetPosition());
        }
    }

    private void SelectNextDestination()
    {
        if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
        {
            if (Random.Range(0f, 1f) <= currentWaypoint.branchRatio)
            {
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count)];
                return;
            }
        }
        if (currentWaypoint.nextWaypoint == null && currentWaypoint.previousWaypoint == null)
        {
            return;
        }
        if (direction == 0)
        {
            if (currentWaypoint.nextWaypoint == null)
            {
                currentWaypoint = currentWaypoint.previousWaypoint;
                direction = 1;
                return;
            }
            currentWaypoint = currentWaypoint.nextWaypoint;
        }
        else if (direction == 1)
        {
            if (currentWaypoint.previousWaypoint == null)
            {
                currentWaypoint = currentWaypoint.nextWaypoint;
                direction = 0;
                return;
            }
            currentWaypoint = currentWaypoint.previousWaypoint;
        }
    }
}

