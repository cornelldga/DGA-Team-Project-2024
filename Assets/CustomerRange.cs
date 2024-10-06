using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerRange : MonoBehaviour
{
    public bool orderTaken = false;
    public bool playerInRange = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            orderTaken = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered the range");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered range");
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited range");
            playerInRange = false;
        }
    }
}
