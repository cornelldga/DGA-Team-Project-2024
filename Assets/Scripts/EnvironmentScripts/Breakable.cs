using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private float minBreakSpeed = 0f;
    [SerializeField] private int oilAmount = 25; // Oil amount should be 0 if not oil box/barrel

    private void Update()
    {
        
    }



    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (minBreakSpeed != 0)
        {
            return;
        }
        CheckCollision(collision);
    }

    private void CheckCollision(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            //print("here2");
            float collisionSpeed = collision.relativeVelocity.magnitude;
            //print("collisionSpeed:" + (collisionSpeed));
            // Break if either:
            // 1. minBreakSpeed is 0 (break on any collision)
            // 2. collision speed is above the minimum threshold
            if (minBreakSpeed <= 0f || collisionSpeed >= minBreakSpeed)
            {
                //print("here3");
                GameManager.Instance.getPlayer().AddOil(oilAmount);

                gameObject.SetActive(false);

            }
        }
    }
}