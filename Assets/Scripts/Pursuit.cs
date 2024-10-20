using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script gives the cop the ability to "pursue" the player. It will take the shortest path until hitting the player.
public class Pursuit : MonoBehaviour
{
    private GameObject player;
    private float speed = 8F;

    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("PlayerModel");
    }

    // Update is called once per frame
    void Update()
    {
        //In the update method, the cop will check where the player is, and orient itself accordingly. It wil then move towards the player.
        distance = Vector3.Distance(this.transform.position,player.transform.position);
        Vector3 direction = player.transform.position -transform.position;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed* Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle );

        
    }
}
