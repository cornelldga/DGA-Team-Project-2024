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
    void Start()
    {
        observeRigidBody = observe.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (observe == null)
            return;

        Vector3 targetPosition = observe.position + Vector3.up * cameraHeight + observeRigidBody.velocity * aheadSpeed;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followDamping * Time.deltaTime);

    }
}
