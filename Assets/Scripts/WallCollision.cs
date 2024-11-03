using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles the collision between a wall and the player
public class WallCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.relativeVelocity.magnitude);
        if (collision.gameObject.tag == "Player")
        {
            if (collision.relativeVelocity.magnitude >= 15.0f)
            { 
                GameManager.Instance.getPlayer().TakeDamage(1); 
            }
        }
    }
}
