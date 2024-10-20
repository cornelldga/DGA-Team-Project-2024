using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A work in progress isometric controller (just for testing cop following.)
public class Movement : MonoBehaviour


{
    private Rigidbody player;
    private float speed = 10;
    private Vector3 input;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GatherInput();
        Direction();
    }

    void FixedUpdate()
    {
        Move();
    }
    void GatherInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

    }
    void Move()
    {
        player.MovePosition(transform.position + (transform.forward * input.magnitude) * speed * Time.deltaTime);

    }
    void Direction()
    {
        if (input != Vector3.zero)
        {
            var rel = ((transform.position + input) - transform.position);
            var rotation = Quaternion.LookRotation(rel, Vector3.up);
            transform.rotation = rotation;
        }
    }

}