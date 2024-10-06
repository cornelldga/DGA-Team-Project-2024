using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TopDownContr : MonoBehaviour
{
    [SerializeField] Transform observe;
    [SerializeField] float aheadSpeed;
    [SerializeField] float followDamping;
    [SerializeField] float cameraHeight;

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
