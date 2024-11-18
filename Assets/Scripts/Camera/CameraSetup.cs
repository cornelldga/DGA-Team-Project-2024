using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This Script handles the Camera Movement and Setup*/

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

    Transform player; // The player or object to follow
    [Tooltip("The offset from the player to the camera")]
    [SerializeField] Vector3 offset = new Vector3(-20, 20, -20); 

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial position of the camera based on the offset
        player = GameManager.Instance.getPlayer().transform;
        transform.position = player.position + offset;

    }

    //LateUpdate is called every frame, if the Behaviour is enabled (after all Update functions have been called)
    void LateUpdate()
    {

        //calculating  target position based on the player's position and offset
        Vector3 targetPosition = player.position + offset;

        // move the camera to the target position 
        transform.position = targetPosition;


    }

}


