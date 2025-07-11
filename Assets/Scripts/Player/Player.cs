using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

// This script handles the inputs and manages the oil and cooking timers for the player
public class Player : MonoBehaviour, ICrashable
{
    [Tooltip("Player moves forward by a product of this speed")]
    [SerializeField] float speed = 4f;

    //Player health var
    [Tooltip("Number of health points that player starts with")]
    [SerializeField] float health = 3f;

    [Header("Invincibility Frames")]
    [Tooltip("Number of seconds player is invincible after being hit")]
    [SerializeField] private float invincibilityDuration = 1.5f;
    [Tooltip("Splits invincibility duration into individual frames to inform blinking")]
    [SerializeField] private float invincibilityDeltaTime = 0.15f;

    [Header("Oil Values")]
    [Tooltip("Max amount of oil that player starts with")]
    [SerializeField] float maxOil = 100f;

    [Header("Input Key Codes")]
    [Tooltip("Left button for nitro")]
    [SerializeField] private KeyCode nitro1 = KeyCode.LeftShift;
    [Tooltip("Right button for nitro")]
    [SerializeField] private KeyCode nitro2 = KeyCode.RightShift;
    [Tooltip("Button for drifting")]
    [SerializeField] private KeyCode drift = KeyCode.Space;

    private Rigidbody rb;
    private Vector3 lastVelocity;
    private float oil;
    private Vector3[] directionVector = {
        Vector3.forward,
        Vector3.forward+Vector3.right,
        Vector3.right,
        Vector3.right+Vector3.back,
        Vector3.back,
        Vector3.back+Vector3.left,
        Vector3.left,
        Vector3.forward+Vector3.left};
    // made visible to customize starting direction
    [SerializeField]  private int curDirection = 2;
    int lastDirection = 2;
    private bool isInvincible = false;
    private float turnDelay = 0;
    private float turnRate = 0.25f;

    private float driftTime = 0;
    private float driftLimit = 0;
    private bool driftOut = false;
    private float rightDriftNum = 0;
    private float leftDriftNum = 0;
    private float driftNum = 0;
    private bool drifting = false;
    private bool startDrift = false;
    private int driftAngle = 0;
    private float driftCooldown;
    private bool downDrift = false;
    private bool slowMo = false;
    private bool canDrift = true;

    // Input booleans
    private bool pressForward;
    private bool pressBackward;
    private bool pressRight;
    private bool pressLeft;
    private bool pressNitro;
    private bool pressDrift;

    //New added private variables 

    private List<Customer> customers;

    [SerializeField] float minCrashSpeed;

    [SerializeField] private AnimatorController animController;

    [SerializeField] private GameObject model;

    public ParticleSystem smokeParticle;

    [Tooltip("The maximum amount of force applied when colliding")]
    [SerializeField] float maxCollisionForce;

    //Sound Tracking Variables
    private bool boostSoundPlayed = false;
    private bool lowFuelSoundPlayed = false;
    private int bikeSqueakMax = 200;
    private int bikeSqueakTimer = 200;

    [SerializeField] SpriteRenderer[] arrows;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        oil = maxOil;
        customers = GameManager.Instance.GetCustomers();
        Physics.IgnoreLayerCollision(3, 6, false);
    }

    void FixedUpdate()
    {
        lastVelocity = rb.velocity;
        Drive();
        Nitro();
        Drift();
    }

    private void Update()
    {
        driftCooldown -= Time.deltaTime;
        animController.ResetConditions();
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            pressForward = true;
        }
        else
        {
            pressForward = false;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            pressBackward = true;
        }
        else
        {
            pressBackward = false;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            pressRight = true;
        }
        else
        {
            pressRight = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            pressLeft = true;
        }
        else
        {
            pressLeft = false;
        }
        if (Input.GetKey(nitro1) || Input.GetKey(nitro2))
        {
            pressNitro = true;
        }
        else
        {
            pressNitro = false;
        }
        if (Input.GetKey(drift))
        {
            pressDrift = true;
        }
        else
        {
            pressDrift = false;
        }
        if (!pressRight && !pressLeft)
        {
            turnDelay = 0;
        }
        if ((pressRight || pressLeft) && Time.time > turnDelay)
        {
            turnDelay = Time.time + turnRate;
            Turn();
        }
        if (customers.Count != 0)
        {
            HandleOrders();
        }

        //0, 45, 90 is N
        //90, 135, 180 is E
        //180, 225, 270 is S
        //270, 315, 0 is W
        if(curDirection != lastDirection)
        {
            arrows[lastDirection].enabled = false;
            arrows[curDirection].enabled = true;
        }
        if (curDirection == 0 || curDirection == 1 || curDirection == 2)
        {
            animController.SetFacingNorth(true);
        }
        if (curDirection == 2 || curDirection == 3 || curDirection == 4)
        {
            animController.SetFacingEast(true);
        }
        if (curDirection == 4 || curDirection == 5 || curDirection == 6)
        {
            animController.SetFacingSouth(true);
        }
        if (curDirection == 6 || curDirection == 7 || curDirection == 0)
        {
            animController.SetFacingWest(true);
        }
        if (pressForward ^ pressBackward)
        {
            if (curDirection == 0 || curDirection == 1 || curDirection == 2)
            {
                animController.SetMovingNorth(true);
            }
            if (curDirection == 2 || curDirection == 3 || curDirection == 4)
            {
                animController.SetMovingEast(true);
            }
            if (curDirection == 4 || curDirection == 5 || curDirection == 6)
            {
                animController.SetMovingSouth(true);
            }
            if (curDirection == 6 || curDirection == 7 || curDirection == 0)
            {
                animController.SetMovingWest(true);
            }
        }
        lastDirection = curDirection;

    }


    // While holding shift, the player uses oil to nitro boost.
    void Nitro()
    {
        if (pressNitro && !(oil > 0) && !lowFuelSoundPlayed)
        {
            AudioManager.Instance.Play("sfx_LowFuel");
            lowFuelSoundPlayed = true;
        }
        if (pressNitro && oil > 0)
        {
            rb.AddRelativeForce(directionVector[curDirection] * 50);
            oil--;
            if (!boostSoundPlayed)
            {
                AudioManager.Instance.PlaySound("sfx_Boost");
                boostSoundPlayed = true;
                lowFuelSoundPlayed = true;
            }
        }
        if (!pressNitro)
        {
            boostSoundPlayed = false;
            lowFuelSoundPlayed = false;
        }

    }

    // The player can press W and S to drive forwards and backwards.
    void Drive()
    {
        if (pressForward && pressBackward)
        {
            return;
        }
        if (pressForward)
        {
            rb.AddRelativeForce(directionVector[curDirection].normalized * speed * 10);
            PlayPedalSFX();
        }
        else if (pressBackward)
        {
            rb.AddRelativeForce(-directionVector[curDirection].normalized * speed * 10);
            PlayPedalSFX();
        }
    }

    //Function to handle when bike squeaking sound effects should be played
    void PlayPedalSFX()
    {
        if (bikeSqueakTimer > 0)
        {
            bikeSqueakTimer--;
        }
        else
        {
            int soundToPlay = Random.Range(1, 7);
            //Removed temporarily bc annoying
            //AudioManager.Instance.PlaySound("sfx_BikeSqueak" + soundToPlay);
            bikeSqueakTimer = bikeSqueakMax;
        }
    }

    // Turns the player right for the sake of the tutorial.
    public void TurnRight()
    {
        if (curDirection == 7)
        {
            curDirection = 0;
        }
        else
        {
            curDirection++;
        }
        if (startDrift)
        {
            leftDriftNum = 0;
            rightDriftNum++;
        }
    }

    // The player can use A and D to turn to the next of 8 possible directions.
    void Turn()
    {
        if (pressRight)
        {
            if (curDirection == 7)
            {
                curDirection = 0;
            }
            else
            {
                curDirection++;
            }
            if (startDrift)
            {
                leftDriftNum = 0;
                rightDriftNum++;
            }
        }
        else if (pressLeft)
        {
            if (curDirection == 0)
            {
                curDirection = 7;
            }
            else
            {
                curDirection--;
            }
            if (startDrift)
            {
                rightDriftNum = 0;
                leftDriftNum++;
            }
        }
    }

    // Player can hold the spacebar to brake and turn while braking to drift
    void Drift()
    {
        const float pitchTime = 0.25f;
        if ((pressDrift || slowMo) && canDrift && !driftOut && driftCooldown <= 0)
        {

            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, 3.5f, 7 * Time.deltaTime);

            if (!downDrift)
            {
                driftAngle = curDirection;
                driftLimit = Time.time + 1;
                StartCoroutine(SlowCooldown());
                downDrift = true;
            }
            drifting = true;
            startDrift = true;
            Time.timeScale = 0.5f;
            //Slow down audio
            StartCoroutine(AudioManager.Instance.ChangePitch(0.5f, pitchTime));
            if (Time.time >= driftLimit)
            {
                driftOut = true;
            }
        }
        else if ((!pressDrift || driftOut || !canDrift) && drifting)
        {
            if (startDrift)
            {
                driftTime = Time.time + 0.5f;
                startDrift = false;
                if (rightDriftNum > 0)
                {
                    if (driftAngle + 2 == curDirection || driftAngle - 6 == curDirection)
                    {
                        driftNum = 2;
                    }
                    else if (driftAngle + 3 == curDirection || driftAngle - 5 == curDirection)
                    {
                        driftNum = 3;
                    }
                    else if (driftAngle + 4 == curDirection || driftAngle - 4 == curDirection)
                    {
                        driftNum = 4;
                    }
                }
                if (leftDriftNum > 0)
                {
                    if (driftAngle - 2 == curDirection || driftAngle + 6 == curDirection)
                    {
                        driftNum = 2;
                    }
                    else if (driftAngle - 3 == curDirection || driftAngle + 5 == curDirection)
                    {
                        driftNum = 3;
                    }
                    else if (driftAngle - 4 == curDirection || driftAngle + 4 == curDirection)
                    {
                        driftNum = 4;
                    }
                }
                rightDriftNum = 0;
                leftDriftNum = 0;
            }
            if (driftNum == 2)
            {
                rb.AddRelativeForce(directionVector[curDirection] * 20);
            }
            if (driftNum == 3)
            {
                rb.AddRelativeForce(directionVector[curDirection] * 25);
            }
            if (driftNum == 4)
            {
                rb.AddRelativeForce(directionVector[curDirection] * 30);
            }
            Time.timeScale = 1;
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, 5f, 7 * Time.deltaTime);
            
            if (Time.time >= driftTime)
            {
                drifting = false;
                driftNum = 0;
                driftOut = false;
                Camera.main.orthographicSize = 5f;
                driftCooldown = 2f;
            }

            //TODO Add update to audio manager to speed up audio

            StartCoroutine(AudioManager.Instance.ChangePitch(1f, pitchTime*2));

            downDrift = false;
        }
    }

    private IEnumerator SlowCooldown()
    {
        slowMo = true;

        yield return new WaitForSeconds(0.25f);

        slowMo = false;
    }


    /// <summary>
    /// Called every frame to check through the list of customers and decrease cooking time and oil
    /// </summary>
    void HandleOrders()
    {
        foreach (Customer customer in customers)
        {
            if (customer.cookTime > 0)
            {
                // with the addition of minigames to cook, the timer no longer passively ticks down. 
                //customer.cookTime -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Adds oil to the player's vehichle, never exceeding the maxOil amount
    /// </summary>
    public void AddOil(int oilAmount)
    {
        if (oil <= maxOil)
        {
            oil = oil + oilAmount; //Changed functionality old function was updating oil properly
        }
    }

    private void ScaleModelTo(Vector3 scale)
    {
        model.transform.localScale = scale;
    }

    /// <summary>
    /// Counts amount of time that player should be invincible after taking damage
    /// </summary>
    private IEnumerator BecomeInvincible()
    {
        isInvincible = true;

        Physics.IgnoreLayerCollision(3, 6, true);
        for (float i = 0; i < invincibilityDuration; i += invincibilityDeltaTime)
        {
            if (model.GetComponent<SpriteRenderer>().color == Color.white)
            {
                model.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                model.GetComponent<SpriteRenderer>().color = Color.white;
            }
            yield return new WaitForSeconds(invincibilityDeltaTime);
        }
        Physics.IgnoreLayerCollision(3, 6, false);
        model.GetComponent<SpriteRenderer>().color = Color.white;
        isInvincible = false;
    }

    // Decreases health of player and inititates invulnerability frames
    public void TakeDamage()
    {
        if (isInvincible) return;
        health--;
        int random = Random.Range(0, 2);
        AudioManager.Instance.Play("sfx_Crash1");

        if (health <= 0)
        {
            health = 0;
            GameManager.Instance.LoseGame();
            return;
        }
        StartCoroutine(BecomeInvincible());
    }

    // Public methods to access oil and maxOil
    public float GetOil()
    {
        return oil;

    }

    public float GetMaxOil()
    {
        return maxOil;
    }

    public float GetHealth()
    {
        return health;
    }

    public void Crash(Vector3 speedVector, Vector3 position)
    {
        TakeDamage();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6)
        {
            //Cop Sound already plays, so this is left blank
        }
        if (other.gameObject.layer == 7)
        {
            //If player hits wall
            AudioManager.Instance.Play("sfx_BuildingThud");
        }
        if (other.gameObject.GetComponent<ICrashable>() != null)
        {
            other.gameObject.GetComponent<ICrashable>().Crash(rb.velocity, transform.position);
        }
        float curSpeed = lastVelocity.magnitude;
        if (curSpeed >= 7f)
        {
            Vector3 direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);
            GetComponent<Rigidbody>().velocity = direction * 0.5f * Mathf.Max(curSpeed, maxCollisionForce);
        }
    }


}
