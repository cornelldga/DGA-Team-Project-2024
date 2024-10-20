using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A work in progress isometric controller (just for testing cop following.)
public class Movement : MonoBehaviour
{
    private float speed = 10;
    private float direction;
    private Rigidbody player;

    // Start is called before the first frame update
    void Start()
    {
        player  = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = Input.GetAxis("Vertical");

        player.velocity = new Vector3(direction*speed,player.velocity.x);

    }
}
