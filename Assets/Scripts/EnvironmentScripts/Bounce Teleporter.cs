using UnityEngine;
/// <summary>
/// Controls a teleport pad that launches the player in an arc towards a target position with a bounce effect.
/// </summary>
public class TeleportPad : MonoBehaviour
{
    [SerializeField] private float upwardForce = 20f;     // Initial upward boost
    [SerializeField] private float forwardForce = 15f;    // Initial forward momentum
    [SerializeField] private Transform targetPosition;     // Where to teleport to
    [SerializeField] private float baseSpeed = 10f;       // Base speed of teleport arc
    [SerializeField] private float maxHeight = 30f;       // Maximum height of sine curve
    [SerializeField] private float triggerHeight = 2f;    // Height of the detection area above pad

    private Player myPlayer;
    private Rigidbody playerRb;
    private Collider playerCollider;
    private BoxCollider triggerCollider;
    private bool isTeleporting = false;
    private float teleportTimer = 0f;
    private float teleportDuration;
    private Vector3 startPosition;

    /// <summary>
    /// Initializes the teleport pad by getting references to the player components (rigid body and collider).
    /// </summary>
    public void Start()
    {
        myPlayer = GameManager.Instance.getPlayer();
        if (myPlayer != null)
        {
            playerRb = myPlayer.GetComponent<Rigidbody>();
            playerCollider = myPlayer.GetComponent<Collider>();
        }

        // Setup trigger collider
        triggerCollider = GetComponent<BoxCollider>();
        if (triggerCollider == null)
        {
            triggerCollider = gameObject.AddComponent<BoxCollider>();
        }
        triggerCollider.isTrigger = true;

        // Adjust the trigger collider to be above the pad
        Vector3 size = triggerCollider.size;
        size.y = triggerHeight;
        triggerCollider.size = size;

        Vector3 center = triggerCollider.center;
        center.y = triggerHeight / 2f;
        triggerCollider.center = center;
    }

    /// <summary>
    /// Updates the player's position along a sine curve during teleportation.
    /// </summary>
    private void Update()
    {
        if (isTeleporting)
        {
            teleportTimer += Time.deltaTime;
            float progress = teleportTimer / teleportDuration;
            if (progress <= 1f)
            {
                float heightOffset = Mathf.Sin(progress * Mathf.PI) * maxHeight;
                Vector3 basePosition = Vector3.Lerp(startPosition, targetPosition.position, progress);
                Vector3 newPosition = basePosition + Vector3.up * heightOffset;
                playerRb.MovePosition(newPosition);
            }
            else
            {
                isTeleporting = false;
                teleportTimer = 0f;
                playerRb.position = targetPosition.position;
                playerRb.velocity = Vector3.zero;
                if (playerCollider != null)
                {
                    playerCollider.enabled = true;
                }
            }
        }
    }

    /// <summary>
    /// Initiates the teleportation sequence when player enters the trigger volume.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerRb != null && targetPosition != null && !isTeleporting)
            {
                // Calculate teleport duration based on distance
                float distance = Vector3.Distance(transform.position, targetPosition.position);
                teleportDuration = distance / baseSpeed;

                Vector3 playerDirection = other.gameObject.transform.forward;
                Vector3 bounceVector = (Vector3.up * upwardForce) + (playerDirection * forwardForce);
                playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
                playerRb.AddForce(bounceVector, ForceMode.Impulse);

                startPosition = playerRb.position;
                isTeleporting = true;
                if (playerCollider != null)
                {
                    playerCollider.enabled = false;
                }
            }
        }
    }

    /// <summary>
    /// Visualizes the trigger volume and teleport path in the editor.
    /// </summary>
    private void OnDrawGizmos()
    {
        // Draw trigger volume
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            // Account for object's scale and collider size
            Vector3 scaledSize = new Vector3(
                boxCollider.size.x * transform.lossyScale.x,
                triggerHeight,
                boxCollider.size.z * transform.lossyScale.z
            );

            Vector3 triggerCenter = transform.position + new Vector3(0, triggerHeight / 2f, 0);

            // Draw the trigger volume
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            Gizmos.DrawCube(triggerCenter, scaledSize);
            Gizmos.color = new Color(0, 1, 0, 0.8f);
            Gizmos.DrawWireCube(triggerCenter, scaledSize);
        }

        // Draw path to target
        if (targetPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, targetPosition.position);

            // Draw a few points along the arc
            const int segments = 20;
            Vector3 lastPoint = transform.position;
            float distance = Vector3.Distance(transform.position, targetPosition.position);
            float simulatedDuration = distance / baseSpeed;

            for (int i = 1; i <= segments; i++)
            {
                float progress = i / (float)segments;
                float heightOffset = Mathf.Sin(progress * Mathf.PI) * maxHeight;
                Vector3 basePosition = Vector3.Lerp(transform.position, targetPosition.position, progress);
                Vector3 point = basePosition + Vector3.up * heightOffset;

                Gizmos.DrawLine(lastPoint, point);
                lastPoint = point;
            }
        }
    }
}