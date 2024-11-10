using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private float minBreakSpeed = 0f;
    [SerializeField] private int oilAmount = 25; // add oil, 0 if not an oil barrel
    [SerializeField] private Material myMaterial;
    [SerializeField] private float respawnTime = 10f;
    [SerializeField] private int damageAmount = 0; // should be 0 for oil 

    private Color myColor;
    private bool isRespawning = false;
    private float respawnTimer = 0f;
    private Vector3 startPosition;
    private Collider myCollider;
    private Player myPlayer;
    private Material materialInstance;

    public void Start()
    {
        materialInstance = new Material(myMaterial);
        GetComponent<Renderer>().material = materialInstance;

        // make completely opaque
        myColor = materialInstance.color;
        myColor.a = 1f;
        materialInstance.color = myColor;

        startPosition = transform.position;
        myCollider = GetComponent<Collider>();
        myPlayer = GameManager.Instance.getPlayer();
    }

    private void Update()
    {
        if (isRespawning)
        {
            respawnTimer += Time.deltaTime;
            float progress = respawnTimer / respawnTime;
            myColor.a = Mathf.Lerp(0f, 1f, progress);
            materialInstance.color = myColor;
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
                myPlayer.TakeDamage(damageAmount);
                StartRespawn();
            }
        }
    }

    private void StartRespawn()
    {
        myCollider.enabled = false;
        myColor.a = 0f;
        materialInstance.color = myColor;
        transform.position = startPosition;
        isRespawning = true;
        respawnTimer = 0f;
    }

    private void OnDestroy()
    {
        if (materialInstance != null)
        {
            Destroy(materialInstance);
        }
    }
}