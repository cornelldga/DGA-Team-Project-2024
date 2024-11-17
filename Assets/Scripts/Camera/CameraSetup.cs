using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    Transform player; // The player or object to follow
    [Tooltip("The offset from the player to the camera")]
    [SerializeField] Vector3 offset = new Vector3(-20, 20, -20);

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.getPlayer().transform;
        transform.position = player.position + offset;

    }

    // Update is called once per frame
    void Update()
    {
        //calculating  target position based on the player's position and offset
        Vector3 targetPosition = player.position + offset;

        // move the camera to the target position 
        transform.position = targetPosition;


    }
}
