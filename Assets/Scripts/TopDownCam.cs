using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//This script controls the PlayerCamera(Main Camera) and follows the movement of the player 
public class TopDownContr : MonoBehaviour
{
    //Target object camera follows 
    [SerializeField] Transform observe;
    //How far ahead  of target's velocity camera should be positioned
    [SerializeField] float aheadSpeed;
    //Smoothness of camera movement
    [SerializeField] float followDamping;

    [SerializeField] float cameraHeight;
    //Used to get target's velocity
    Rigidbody observeRigidBody;
    // Start is called before the first frame update

    // Camera pivot transform
    [SerializeField] Transform cameraPivot;

    void Start()
    {
        if (observe != null)
        {
            observeRigidBody = observe.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("Observe target is not assigned.");
        }

        if (cameraPivot == null)
        {
            Debug.LogError("Camera pivot is not assigned.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (observe == null)
            return;

        // Calculate the target position for the camera pivot
        Vector3 targetPosition = observe.position + Vector3.up * cameraHeight + observeRigidBody.velocity * aheadSpeed;
        cameraPivot.position = Vector3.Lerp(cameraPivot.position, targetPosition, followDamping * Time.deltaTime);

    }
}
