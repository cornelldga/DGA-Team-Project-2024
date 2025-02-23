using UnityEngine;

/// <summary>
/// Controls breakable objects like oil barrels that can be destroyed by player collision and respawn over time.
/// </summary>
public class Breakable : MonoBehaviour
{
    [SerializeField] private float minBreakSpeed = 0f;
    [SerializeField] private int oilAmount = 25;       // Amount of oil to add, 0 if not an oil barrel
    [SerializeField] private Material myMaterial;
    [SerializeField] private float respawnTime = 10f;
    [SerializeField] private float animationDuration = 0.5f; // Add this to set animation length

    private Color objectColor;
    private bool isRespawning = false;
    private float respawnTimer = 0f;
    private Vector3 startPosition;
    private Collider objectCollider;
    private Player myPlayer;
    private Material materialInstance;
    private Animator animator;
    private bool isAnimating = false;
    private float animationTimer = 0f;

    /// <summary>
    /// Initializes the breakable object's material, position, and component references
    /// </summary>
    public void Start()
    {
        animator = GetComponent<Animator>();
        InitializeComponents();
    }

    /// <summary>
    /// Updates the object's transparency during respawn sequence
    /// </summary>
    private void Update()
    {
        if (isAnimating)
        {
            animationTimer += Time.deltaTime;
            if (animationTimer >= animationDuration)
            {
                isAnimating = false;
                StartFade();
            }
        }
        else if (isRespawning)
        {
            UpdateRespawnProgress();
        }
    }

    /// <summary>
    /// Checks for player collision to trigger break/destroy of object
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision);
    }

    /// <summary>
    /// Continuously checks for collision when minimum break speed is zero
    /// </summary>
    private void OnCollisionStay(Collision collision)
    {
        if (minBreakSpeed != 0)
        {
            return;
        }
        CheckCollision(collision);
    }

    /// <summary>
    /// Checks for trigger/collision with player (for when we don't want physics in collisions)
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            myPlayer.AddOil(oilAmount);
            StartRespawn();
            //AudioManager.Instance.Play("sfx_BarrelBreak");
            AudioManager.Instance.Play("sfx_OilReplenish");
        }
    }

    /// <summary>
    /// Processes collision with player and applies appropriate effects
    /// </summary>
    private void CheckCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float collisionSpeed = collision.relativeVelocity.magnitude;
            if (minBreakSpeed <= 0f || collisionSpeed >= minBreakSpeed)
            {
                myPlayer.AddOil(oilAmount);
                StartRespawn();
                //AudioManager.Instance.Play("sfx_BarrelBreak");
                AudioManager.Instance.Play("sfx_OilReplenish");
            }
        }
    }

    /// <summary>
    /// Initiates the respawn sequence for the breakable object
    /// </summary>
    private void StartRespawn()
    {
        // Start animation first
        if (animator != null)
        {
            animator.SetTrigger("Break");
            isAnimating = true;
            animationTimer = 0f;
        }

        // Disable collider immediately
        objectCollider.enabled = false;
    }

    /// <summary>
    /// Initiates the fading once the animation has finished
    /// </summary>
    private void StartFade()
    {
        // Now start the fade after animation
        objectColor.a = 0f;
        materialInstance.color = objectColor;
        transform.position = startPosition;
        isRespawning = true;
        respawnTimer = 0f;
    }


    /// <summary>
    /// Cleans up material instance when object is destroyed
    /// </summary>
    private void OnDestroy()
    {
        if (materialInstance != null)
        {
            Destroy(materialInstance);
        }
    }

    /// <summary>
    /// Sets up initial component references and material state
    /// </summary>
    private void InitializeComponents()
    {
        materialInstance = new Material(myMaterial);
        // Get sprite renderer from child
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.material = materialInstance;
            //renderer.sortingLayerName = "Player";
            //renderer.sortingOrder = 1; 
        }
        objectColor = materialInstance.color;
        objectColor.a = 1f;
        materialInstance.color = objectColor;
        startPosition = transform.position;
        objectCollider = GetComponent<Collider>();
        animator = GetComponentInChildren<Animator>();  // Get animator from child
        myPlayer = GameManager.Instance.getPlayer();
    }

    /// <summary>
    /// Updates the transparency during respawn sequence
    /// </summary>
    private void UpdateRespawnProgress()
    {
        respawnTimer += Time.deltaTime;
        float progress = respawnTimer / respawnTime;
        objectColor.a = Mathf.Lerp(0f, 1f, progress);
        materialInstance.color = objectColor;

        if (progress >= 1f)
        {
            isRespawning = false;
            objectCollider.enabled = true;
        }
    }
}