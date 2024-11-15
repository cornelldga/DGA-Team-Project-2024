using System;
using System.Collections;
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
    // Internal Constants
    private const float RamRadius = 10; // how close the cop has to be to the cop to start a ram
    private const float VisionRadius = 15; // how close the player has to be to start a pursuit
    private const float MaxPursuitRadius = 35; // The distance where the cop will lose sight of the target
    private const int WanderDistance = 15; // the max distance that the cop will wander to per re-route
    private const int BaseSpeed = 12; // base movement speed while patrolling
    private const int RamSpeed = 20; // revved up speed barreling towards the player. 
    private const float RamCooldown = 1; // the amount of time spend on a ram attack until returning to normal navigation
    private const int WanderRerouteTime = 5; // max time spend on a single wander path to prevent getting stuck
    
    // The behavior that determines a cops pathfinding target
    [SerializeField] private NavState State;
    [SerializeField] private CopType model;

    [SerializeField] private Billboard Sprite;

    // reference to its own rigid body
    private Rigidbody RB;

    // Pathfinding parameters
    private Pathfinding pathfindingLogic;
    // The path the cop is heading towards
    private Vector2[] CurrentPath;
    // Position along the path that cop is at
    private int CurrentIndex;
    
    //Damage associated with the type of vehicle
    private int damage;

    // Movement speed multiplier towards the target
    private int speed = BaseSpeed;

    // Parameters managing cooldown for ramming, in seconds
    private float RamTimer = 0;
    private bool IsRamming = false;

    // parameters for managing wandering rerouting
    private float WanderTime = 0;


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

        // set the damage and speed amount based on cop model type
        // TODO: have cruiser and truck be their own prefabs. 
        switch (model)
        {
            case (CopType.CRUISER):
                damage = 1;
                break;
            case (CopType.TRUCK):
                damage = 2;
                break;
            default:
                damage = 1;
                break;
        }
    }

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

            RB.transform.LookAt(GameManager.Instance.getPlayer().transform.position);

            Vector3 moveDir = (GameManager.Instance.getPlayer().transform.position - this.transform.position).normalized;
            RB.velocity = moveDir * RamSpeed;
            CurrentPath = null;

        } 

        // set navigation state
        else if (distanceFromPlayer < VisionRadius)
        {
            State = NavState.HOTPURSUIT;
        }

        else if (State == NavState.HOTPURSUIT && distanceFromPlayer > MaxPursuitRadius)
        {
            State = NavState.WANDER;
            WanderTime = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {   
        if (other.gameObject.GetComponent<ICrashable>() != null)
        {
            other.gameObject.GetComponent<ICrashable>().Crash(RB.velocity, transform.position);
        } 
    }

    private void FixedUpdate()
    {
        Sprite.UpdateSpriteToRotation(this.transform.localRotation.eulerAngles.y);
    }

    // Update is called once per frame
    void Update()
    {
        StateChanger();

        // resolve the attack before doing anything
        if (IsRamming)
        {
            RamTimer += Time.deltaTime;
            if (RamTimer >= RamCooldown)
            {
                IsRamming = false;
            }

        }



        // calculate path towards current target
        else if (getNavState() == NavState.WANDER)
        {

            WanderTime += Time.deltaTime;

            if ((CurrentPath == null || CurrentIndex >= CurrentPath.Length) || WanderTime >= WanderRerouteTime)
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

                WanderTime = 0;
            }


        }
        else if (State == NavState.HOTPURSUIT)
        {
            SetPathfindingTarget(GameManager.Instance.getPlayer().transform.position);
        }

        // move cop along pathfinding
        HandleMovement();
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

            if (Vector3.Distance(this.transform.position, targetPosition) > 0.5f)
            {
                Vector3 moveDir = (targetPosition - position).normalized;
                RB.velocity = moveDir * speed;
                transform.LookAt(targetPosition);
            }
            else
            {
                CurrentIndex++;
            }
            
        }

    }


}
