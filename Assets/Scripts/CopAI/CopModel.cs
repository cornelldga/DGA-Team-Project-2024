using System;
using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

// Navigation type
public enum NavState
{
    HOTPURSUIT,     // Travel towards the target position via the most direct path. Ram attack the player when in range.
    WANDER,         // Drive through the streets to cover the most distance, looking for the player
    IDLE            // Cop is not moving and staying still in current location. 
}
public enum CopType
{
    CRUISER,
    TRUCK
}

/// <summary>
/// Represents a cop that navs towards the given postion, with the goal of damaging the player. 
/// </summary>
public class CopModel : MonoBehaviour
{
    // The behavior that determines a cops pathfinding target
    [SerializeField] private NavState State;
    [SerializeField] private CopType model;

    [SerializeField] private AnimatorController animController;

    // Internal Constants
    [SerializeField] private float RamRadius = 3; // how close the cop has to be to the cop to start a ram
    [SerializeField] private float VisionRadius = 10; // how close the player has to be to start a pursuit
    [SerializeField] private float MaxPursuitRadius = 15; // The distance where the cop will lose sight of the target
    
    [SerializeField] private float BaseSpeed = 6; // base movement speed while patrolling
    [SerializeField] private int RamSpeed = 7; // revved up speed barreling towards the player. 
    [SerializeField] private float RamCooldown = 0.2f; // the amount of time spend on a ram attack until returning to normal navigation
    [SerializeField] float ramInnacuracy; // adds inaccuracy rotation to ram position
    
    private const int WanderDistance = 15; // the max distance that the cop will wander to per re-route
    private const int WanderRerouteTime = 5; // max time spend on a single wander path to prevent getting stuck
    private const int PursuitRerouteTime = 1; // max time spend on a single hot pursuit path to prevent getting stuck


    // reference to its own rigid body
    private Rigidbody RB;

    // Pathfinding parameters
    private Pathfinding pathfindingLogic;
    // The path the cop is heading towards
    private Vector2[] CurrentPath;
    // Position along the path that cop is at
    private int CurrentIndex;

    // the rotation angle of the cop car to determine drawn animation sprite sheet
    private float angle;

    // Movement speed multiplier towards the target
    private float speed;

    // variables managing cooldown for ramming, in seconds
    private float RamTimer = 0;
    private bool IsRamming = false;

    // variables for managing reroutings
    private float NavTime = 0;


    public NavState getNavState()
    {
        return State;
    }
    public CopType GetCopType()
    {
        return model;
    }

    void Start()
    {
        RB = GetComponent<Rigidbody>();
        speed = BaseSpeed;
    }

    // returns true if cop is pending recieving a path towards a target 
    private bool PendingRoute() { return (CurrentPath == null || CurrentIndex >= CurrentPath.Length); }

    /// <summary>
    /// Change the navigation state of the cop based on proximity to player
    /// </summary>
    private void StateChanger()
    {
        // distance is given as a magnitude
        float distanceFromPlayer = Vector3.Distance(this.transform.position, GameManager.Instance.getPlayer().transform.position);
        
        // set attacking state
        if (!IsRamming && distanceFromPlayer < RamRadius)
        {
            
            IsRamming = true;
            RamTimer = 0;
            Vector3 moveDir = Quaternion.AngleAxis(UnityEngine.Random.Range(-ramInnacuracy, ramInnacuracy), Vector3.up) *
                (GameManager.Instance.getPlayer().transform.position - this.transform.position).normalized;
            RB.velocity = moveDir * RamSpeed;

            angle = (float)((Mathf.Atan2(moveDir.x, moveDir.z)) * (180 / Math.PI)) - 45; // subtract 45 to account for orthographic rotation.
            
            CurrentPath = null;

        }

        // set navigation state
        else if (distanceFromPlayer < VisionRadius)
        {
            State = NavState.HOTPURSUIT;
            if (!FindObjectOfType<AudioManager>().IsSoundPlaying("sfx_SirenLong"))
            {
                //FindObjectOfType<AudioManager>().StopSound("sfx_SirenShort");
                FindObjectOfType<AudioManager>().PlaySound("sfx_SirenLong");
            }
        }

        else if (State == NavState.HOTPURSUIT && distanceFromPlayer > MaxPursuitRadius)
        {
            State = NavState.WANDER;
            /*if (!FindObjectOfType<AudioManager>().IsSoundPlaying("sfx_SirenLong"))
            {
                FindObjectOfType<AudioManager>().StopSound("sfx_SirenShort");
                FindObjectOfType<AudioManager>().PlaySound("sfx_SirenLong");
            }*/
        }
    }

    private void OnCollisionEnter(Collision other)
    {   
        if (other.gameObject.GetComponent<ICrashable>() != null)
        {
            other.gameObject.GetComponent<ICrashable>().Crash(RB.velocity, transform.position);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        StateChanger();


        // resolve the attack before doing anything
        if (IsRamming)
        {
            Debug.Log("Ramming");
            RamTimer += Time.deltaTime;
            if (RamTimer >= RamCooldown)
            {
                IsRamming = false;
            }

        }

        // calculate path towards current target
        else if (getNavState() == NavState.WANDER)
        {
            Debug.Log("Wander");
            NavTime += Time.deltaTime;

            if (PendingRoute() || NavTime >= WanderRerouteTime)
            {
                // Choose a random position to wander to
                // ----
                int sx, sy;

                // random position in radius of self
                //Map.Instance.MapGrid.GetXY(this.transform.position, out sx, out sy);

                // random position in radius of player
                Map.Instance.MapGrid.GetXY(GameManager.Instance.getPlayer().transform.position, out sx, out sy);

                SetPathfindingTarget(sx + UnityEngine.Random.Range(-WanderDistance, WanderDistance), sy + UnityEngine.Random.Range(-WanderDistance, WanderDistance));
                // -----

                NavTime = 0;
            }


        }
        else if (State == NavState.HOTPURSUIT)
        {
            Debug.Log("Pursuit");
            NavTime += Time.deltaTime;

            if (PendingRoute() || NavTime >= PursuitRerouteTime)
            {
                SetPathfindingTarget(GameManager.Instance.getPlayer().transform.position);
                NavTime = 0;
            }
            
        }

        // move cop along pathfinding
        HandleMovement();

        // Update to the correct animation to rotation angle
        animController.SetAnimFromRotationAngle(angle);

    }


    /// <summary>
    /// Set the pathfinding target to the given world position
    /// </summary>
    /// <param name="WorldPosition"></param>
    public void SetPathfindingTarget(Vector3 WorldPosition)
    {
        int x, y;
        Map.Instance.MapGrid.GetXY(WorldPosition, out x, out y);

        SetPathfindingTarget(x, y);

    }


    /// <summary>
    /// Set the pathfinding target to the given world position in grid coordinates
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    public void SetPathfindingTarget(int dx, int dy)
    {
        int sx, sy;
        Map.Instance.MapGrid.GetXY(this.transform.position, out sx, out sy);
        //UnityEngine.Debug.Log("{" + x + "," + y + "}");

        if (pathfindingLogic == null)
        {
            // initalize this here to ensure that map grid in the map instance has be initalized. 
            pathfindingLogic = new Pathfinding();
        }
        if (pathfindingLogic.isValid(dx, dy))
        {
            bool pathFound = pathfindingLogic.FindShortestPath(new Vector2(sx, sy), new Vector2(dx, dy));
            if (pathFound)
            {
                CurrentPath = pathfindingLogic.VectorPath;
                CurrentIndex = 0;
            }
        }

    }


    /// <summary>
    /// Transform the cops position along their pathing finding path towards their current target
    /// </summary>
    private void HandleMovement()
    {
        //UnityEngine.Debug.Log(CurrentPath != null);
        if (CurrentPath != null && CurrentIndex < CurrentPath.Length)
        {
           
            Vector3 targetPosition = Map.Instance.MapGrid.GetWorldPosition(CurrentPath[CurrentIndex].x + 0.5f, CurrentPath[CurrentIndex].y + 0.5f);
            // remove y to ignore it in the calculation
            Vector3 position = this.transform.position;
            targetPosition = new Vector3(targetPosition.x, position.y, targetPosition.z);

            Vector3 targetDirection = targetPosition - position;
            targetDirection.Normalize();
            angle = (float) ((Mathf.Atan2(targetDirection.x, targetDirection.z)) * (180 / Math.PI)) - 45; // subtract 45 to account for orthographic rotation.

            // if not at the target yet, move towards it
            if (Vector3.Distance(this.transform.position, targetPosition) > 0.5f)
            {
                Vector3 moveDir = (targetPosition - position).normalized;
                RB.velocity = moveDir * speed;
            }
            else
            {
                // else increment path index
                CurrentIndex++;
                RB.velocity = new Vector3();
            }
            
        }

    }


}
