using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;





// Navigation type
public enum NavState
{
    HOTPURSUIT,     // Travel directly towards the target position via the most direct path. 
    WANDER,         // Drive through the streets to cover the most distance, looking for the player
    IDLE            // Cop is not moving and staying still in current location. 
}
public enum CopType
{
    CRUISER,
    TRUCK
}


/** 
 * Represents a cop that navs towards the given postion 
 * TODO: add different cop behaviors rather than following a point 
 */
public class CopModel : MonoBehaviour
{
    // Internal Constants
    private const float VisionRadius = 10; // how close the player has to be to start a pursuit
    private const float MaxPursuitRadius = 30; // The distance where the cop will lose sight of the target
    private const int WanderDistance = 5; // the max distance that the cop will wander to per re-route

    // The behavior that determines a cops pathfinding target
    [SerializeField] private NavState State;
    [SerializeField] private Rigidbody RB;
    [SerializeField] private CopType model;
    
    // Pathfinding parameters
    private Pathfinding pathfindingLogic;
    // The path the cop is heading towards
    private Vector2[] CurrentPath;
    // Position along the path that cop is at
    private int CurrentIndex;
    

    // Movement speed multiplier towards the target
    private int speed = 8;

    //Damage associated with the type of vehicle
    private int damage;

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

    /** Change the navigation state of the cop based on proximity to player **/
    public void StateChanger()
    {
        // distance is given as a magnitude
        float distanceFromPlayer = Vector3.Distance(this.transform.position, GameManager.Instance.getPlayer().transform.position);
        if (distanceFromPlayer < VisionRadius)
        {
            State = NavState.HOTPURSUIT;
        }
        else if (State == NavState.HOTPURSUIT && distanceFromPlayer > MaxPursuitRadius)
        {
            State = NavState.WANDER;
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        GameObject damagedObject = other.gameObject;
        if (damagedObject.tag == "Player")
        {
            if (RB.velocity.magnitude > 5)
            {
                doDamage(damagedObject);
            }

        }
        //  else if(damagedObject.tag)
    }
    public void doDamage(GameObject hitObject)
    {
    }

    // Update is called once per frame
    void Update()
    {
        StateChanger();

        // calculate path towards current target
        if (getNavState() == NavState.WANDER && (CurrentPath == null || CurrentIndex >= CurrentPath.Length))
        {
            int sx, sy;
            Map.Instance.MapGrid.GetXY(this.transform.position, out sx, out sy);

            SetTarget(sx + Random.Range(-WanderDistance, WanderDistance), sy + Random.Range(-WanderDistance, WanderDistance));
        }
        else if (State == NavState.HOTPURSUIT)
        {
            SetTarget(GameManager.Instance.getPlayer().transform.position);
            transform.LookAt(GameManager.Instance.getPlayer().transform.position);
        }

        HandleMovement();
    }


    /** Set the pathfinding target to the given world position */
    public void SetTarget(Vector3 WorldPosition)
    {
        int x, y;
        Map.Instance.MapGrid.GetXY(WorldPosition, out x, out y);

        SetTarget(x, y);

    }

    /** Set the pathfinding target to the given world position in grid coordinates */
    public void SetTarget(int dx, int dy)
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



    /** Transform the cops position along their pathing finding path towards their current target */
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
            }
            else
            {
                CurrentIndex++;
            }
            
        }

    }


    IEnumerator wanderWait()
    {
        yield return new WaitForSeconds(2);
    }
}
