using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class CopController: MonoBehaviour
{
    public NavMeshAgent agent;
   

    private void Start()
    {
        agent.SetDestination(new Vector3(0,0,0));
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
