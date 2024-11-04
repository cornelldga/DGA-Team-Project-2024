using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IsometricCameraSetup : MonoBehaviour
{

    public Transform player; // The player or object to follow
    public Vector3 offset = new Vector3(0, 10, -10); // The offset from the player to the camera
    public float smoothTime = 0.5f; // The smooth time for the camera movement
    public float maxSpeed = 20.0f; // Maximum speed for the camera movement

    private Vector3 _currentVelocity = Vector3.zero;
    //private Rigidbody _playerRigidbody;

    void Start()
    {
        //_playerRigidbody = player.GetComponent<Rigidbody>();

        // Set the initial position of the camera based on the offset
        transform.position = player.position + offset;

    }

    void LateUpdate()
    {
        if (player == null)
            return;

        // calculating  target position based on the player's position and offset
        Vector3 targetPosition = player.position + offset;

        //  move the camera to the target position (using SmoothDamp)
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime, maxSpeed);


    }

}


