using UnityEngine;

public class TeleportPad : MonoBehaviour
{
    [SerializeField] private float upwardForce = 20f;
    [SerializeField] private float forwardForce = 15f;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float teleportDuration = 3f;
    [SerializeField] private float maxHeight = 30f; // Maximum height of sine curve

    private Player myPlayer;
    private Rigidbody playerRb;
    private Collider playerCollider;
    private bool isTeleporting = false;
    private float teleportTimer = 0f;
    private Vector3 startPosition;

    public void Start()
    {
        myPlayer = GameManager.Instance.getPlayer();
        if (myPlayer != null)
        {
            playerRb = myPlayer.GetComponent<Rigidbody>();
            playerCollider = myPlayer.GetComponent<Collider>();
        }
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerRb != null && targetPosition != null && !isTeleporting)
            {
                Vector3 playerDirection = collision.gameObject.transform.forward;
                Vector3 bounceVector = (Vector3.up * upwardForce) + (playerDirection * forwardForce);

                Vector3 velocity = playerRb.velocity;
                velocity.y = 0;
                playerRb.velocity = velocity;

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
}