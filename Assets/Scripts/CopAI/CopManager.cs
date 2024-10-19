using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopManager : MonoBehaviour
{

    [SerializeField] private CopModel[] cops;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Navigate to the position of the right click
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
           
            for (int i = 0; i < cops.Length; i++)
            {
                cops[i].SetTarget(worldPosition);
            }

        }
    }
}
