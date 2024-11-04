using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] private float upwardForce = 20f;     // Vertical boost
    [SerializeField] private float forwardForce = 15f;    // Forward momentum multiplier
    //[SerializeField] private ParticleSystem bounceEffect; // Uncomment if we have bounce animation/effect
    
    private Player myPlayer;
    private Rigidbody playerRb;


    public void Start()
    {
        myPlayer = GameManager.Instance.getPlayer();
        if (myPlayer != null)
        {
            playerRb = myPlayer.GetComponent<Rigidbody>();
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerRb != null)
            {
                Vector3 playerDirection = collision.gameObject.transform.forward;
                Vector3 bounceVector = (Vector3.up * upwardForce) + (playerDirection * forwardForce);
                Vector3 velocity = playerRb.velocity;
                velocity.y = 0;
                playerRb.velocity = velocity;
                playerRb.AddForce(bounceVector, ForceMode.Impulse);

                //if (bounceEffect != null)
                //{
                //    bounceEffect.Play();
                //}
            }
        }
    }
}