using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RadarDot : MonoBehaviour
{
    private Customer customer;

    [SerializeField] private Animator blipAnim;

    // Customer has not been interacted with = white dot
    // Customer has order taken = green
    // Color of customer tracked
    // When order is failed or completed, should no longer appear on radar

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        blipAnim.SetBool("OrderTaken", customer.IsOrderTaken());
        Debug.Log("OrderTaken: " + customer.IsOrderTaken()); // Verify it changed

        if (customer.IsInactive())
        {
            this.GetComponent<Image>().enabled = false;
        }
    }

    public void SetCustomer(Customer c)
    {
        customer = c;
    }

    public Customer GetCustomer()
    {
        return customer;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        ////Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        //blipAnim.Play("RadarDotAnim", -1, 0.0f);
        ////spriteMove = -0.1f;
        
        // Get the current animation state info
        AnimatorStateInfo currentState = blipAnim.GetCurrentAnimatorStateInfo(0);
        // Play the current state from the beginning
        blipAnim.Play(currentState.fullPathHash, -1, 0.0f);
    }
}
