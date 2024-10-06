using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CopController: MonoBehaviour
{
    public NavMeshAgent agent;
   

    private void Start()
    {

    } 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
      
            if (Physics.Raycast(movePosition, out var hitinfo)){
                agent.SetDestination(hitinfo.point);
           
            }
        }
    }
}
