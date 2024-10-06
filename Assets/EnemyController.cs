using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
public NavMeshAgent car;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Ray position = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(position,out var hitInfo)){
                car.SetDestination(hitInfo.point);
            }
        }
        
    }
}
