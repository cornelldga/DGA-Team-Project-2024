using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private float minBreakSpeed = 0f;
    [SerializeField] private int oilAmount = 25;
    [SerializeField] private Material myMaterial;
    [SerializeField] private float respawnTime = 10f;

    private Color myColor;
    private bool isRespawning = false;
    private float respawnTimer = 0f;
    private Vector3 startPosition; // In case we implement physics-based movement/explosions
    private Collider myCollider;
    private Player myPlayer;

    public void Start()
    {
        myColor = myMaterial.color;
        startPosition = transform.position;
        myCollider = GetComponent<Collider>(); // Only called on creation so it's ok to use GetComponent
        myPlayer = GameManager.Instance.getPlayer();
    }

    private void Update()
    {
        if (isRespawning)
        {
            respawnTimer += Time.deltaTime;
            float progress = respawnTimer / respawnTime;

            myColor.a = Mathf.Lerp(0f, 1f, progress);
            myMaterial.color = myColor;

            if (progress >= 1f)
            {
                isRespawning = false;
                myCollider.enabled = true;
            }
        }
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
        if (collision.gameObject.CompareTag("Player"))
        {
            float collisionSpeed = collision.relativeVelocity.magnitude;

            if (minBreakSpeed <= 0f || collisionSpeed >= minBreakSpeed)
            {
                myPlayer.AddOil(oilAmount);
                StartRespawn();
            }
        }
    }

    private void StartRespawn()
    {
        myCollider.enabled = false;
        myColor.a = 0f;
        myMaterial.color = myColor;
        transform.position = startPosition;
        isRespawning = true;
        respawnTimer = 0f;
    }
}