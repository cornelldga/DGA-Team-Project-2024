using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Camera Following Script

/*
Camera Settings :) 

Transform 
Position: 0 0 0 
Rotation: 30 45 0 
Scale: 1 1 1 

Camera 
Projection: Orthographic 
Size: 15 
Clipping Planes: 
Near: -100
Far: 1000

*/
public class IsometricCameraSetup : MonoBehaviour
{

    public Transform player; // The player or object to follow
    public Vector3 offset = new Vector3(0, 0, 0); // The offset from the player to the camera


    void Start()
    {
        // Set the initial position of the camera based on the offset
        transform.position = player.position + offset;

    }

    void LateUpdate()
    {

        // calculating  target position based on the player's position and offset
        Vector3 targetPosition = player.position + offset;

        //  move the camera to the target position 
        transform.position = targetPosition;


    }

}


