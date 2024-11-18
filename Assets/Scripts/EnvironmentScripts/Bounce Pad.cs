using UnityEngine;
/// <summary>
/// Controls bounce pad behavior to provide upward and forward force to the player upon trigger enter.
/// </summary>
public class BouncePad : MonoBehaviour
{
    [SerializeField] private float upwardForce = 20f;     // Vertical boost
    [SerializeField] private float forwardForce = 15f;    // Forward momentum multiplier
    [SerializeField] private float triggerHeight = 2f;    // Height of the detection area above the pad
                                                          //[SerializeField] private ParticleSystem bounceEffect; // Uncomment if we have bounce animation/effect

    private Player myPlayer;
    private Rigidbody playerRb;
    private BoxCollider triggerCollider;

    /// <summary>
    /// Initializes the bounce pad and sets up the trigger volume.
    /// </summary>
    public void Start()
    {
        myPlayer = GameManager.Instance.getPlayer();
        if (myPlayer != null)
        {
            playerRb = myPlayer.GetComponent<Rigidbody>();
        }

        triggerCollider = GetComponent<BoxCollider>();
        if (triggerCollider == null)
        {
            triggerCollider = gameObject.AddComponent<BoxCollider>();
        }
        triggerCollider.isTrigger = true;

        Vector3 size = triggerCollider.size;
        size.y = triggerHeight;
        triggerCollider.size = size;

        Vector3 center = triggerCollider.center;
        center.y = triggerHeight / 2f;
        triggerCollider.center = center;
    }

    /// <summary>
    /// Applies bounce force to the player when they enter the trigger volume.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRb != null)
            {
                Vector3 playerDirection = other.gameObject.transform.forward;
                Vector3 bounceVector = (Vector3.up * upwardForce) + (playerDirection * forwardForce);
                playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
                playerRb.AddForce(bounceVector, ForceMode.Impulse);
                //if (bounceEffect != null)
                //{
                //    bounceEffect.Play();
                //}
            }
        }
    }

    /// <summary>
    /// Visualizes the trigger volume in the editor.
    /// </summary>
    private void OnDrawGizmos()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            // Account for object's scale and collider size
            Vector3 scaledSize = new Vector3(
                boxCollider.size.x * transform.lossyScale.x,
                triggerHeight,
                boxCollider.size.z * transform.lossyScale.z
            );

            // Calculate center position accounting for scale
            Vector3 triggerCenter = transform.position + new Vector3(0, triggerHeight / 2f, 0);

            // Draw the trigger volume
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            Gizmos.DrawCube(triggerCenter, scaledSize);

            // Draw wireframe for better visibility
            Gizmos.color = new Color(0, 1, 0, 0.8f);
            Gizmos.DrawWireCube(triggerCenter, scaledSize);
        }
    }
}