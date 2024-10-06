using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A simple enemy controler for what will be a police car.
/// </summary>

public class EnemyController : MonoBehaviour
{
public NavMeshAgent car;

    // Update is called once per frame
    /// <summary>
    /// Every second, the position of the mouse on screen is captured as a Ray. If you click space, the car's new destination is that point."
    /// It then starts moving.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Ray position = Camera.main.ScreenPointToRay(Input.mousePosition); // Ray relative to the camera.
            if (Physics.Raycast(position,out var posInfo)){ //.RayCast takes a Ray and gives out posInfo as a variable
                car.SetDestination(posInfo.point); // Sets the destination of this agent.
            }
        }
        
    }
}
