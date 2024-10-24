using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReplenish : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.getPlayer().AddOil(20);
            // Destroy the health object
            Destroy(gameObject);
        }
    }
}
