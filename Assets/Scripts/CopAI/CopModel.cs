using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

// Navigation type
public enum NavState
{
    HOTPURSUIT,     // Travel directly towards the target position via the most direct path. 
    WANDER,         // Drive through the streets to cover the most distance 
    IDLE            //  Cop is not moving and staying still in current location. 
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
    [SerializeField] private Rigidbody RB;
    [SerializeField] private NavState State;
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
    public const int WANDERTIME = 2;
    private float wanderTimer;


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
        if (GetCopType() == CopType.CRUISER)
        {
            damage = 1;
        }
        else if (GetCopType() == CopType.TRUCK)
        {
            damage = 2;
        }
        State = NavState.WANDER;
    }

    private void StateChanger()
    {
        float distance = Vector3.Distance(this.transform.position, GameManager.Instance.getPlayer().transform.position);
        if (distance < 10)
        {
            State = NavState.HOTPURSUIT;
        }
        else if (distance > 30)
        {
            State = NavState.IDLE;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject damagedObject = other.gameObject;
        if (damagedObject.tag == "Player")
        {
            if (RB.velocity.magnitude > 5)
            {
                GameManager.Instance.getPlayer().TakeDamage(DamageAmount(damagedObject));
            }

        }
        //  else if(damagedObject.tag)
    }
    private int DamageAmount(GameObject hitObject)
    {
        return damage;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO add accelerant.
        if (getNavState() == NavState.WANDER && (CurrentPath == null || CurrentIndex >= CurrentPath.Length))
        {
          //  int copX, copY;

          //  int sx, sy;
         //   Map.Instance.MapGrid.GetXY(this.transform.position, out sx, out sy);

         //   SetTarget(sx + Random.Range(-5, 5), sy + Random.Range(-5, 5));
         if (wanderTimer <= 0){
         //|| RB.velocity.magnitude == 0){ //what if we get there early?
            wanderTimer = WANDERTIME;
            Vector3 PlayerCenter = GameManager.Instance.getPlayer().transform.position;
                float radius = 10;
                float angle = UnityEngine.Random.Range(0,Mathf.PI);
                float x = radius * Mathf.Cos(angle);
                float y = radius * Mathf.Sin(angle);
                Vector3 target = new Vector3(PlayerCenter.x + x,PlayerCenter.y+y);
                SetTarget(target);

        
         }
         else{
            wanderTimer -= Time.deltaTime;
         }
        }
        else if (State == NavState.HOTPURSUIT)
        {

            SetTarget(GameManager.Instance.getPlayer().transform.position);
            transform.LookAt(GameManager.Instance.getPlayer().transform.position);
        }

        HandleMovement();
        StateChanger();

    }



    public void SetTarget(Vector3 WorldPosition)
    {
        int x, y;
        Map.Instance.MapGrid.GetXY(WorldPosition, out x, out y);

        SetTarget(x, y);

    }

    private void SetTarget(int dx, int dy)
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
            if (State == NavState.HOTPURSUIT)
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
            else if (getNavState() == NavState.WANDER)
            {
                
                Vector3 targetPosition = Map.Instance.MapGrid.GetWorldPosition(CurrentPath[CurrentIndex].x + 0.5f, CurrentPath[CurrentIndex].y + 0.5f);
                // remove y to ignore it in the calculation
                Vector3 position = transform.position;
                targetPosition = new Vector3(targetPosition.x, position.y, targetPosition.z);

                if (Vector3.Distance(this.transform.position, targetPosition) > 0.5f)
                {

                    Vector3 moveDir = (targetPosition - position).normalized;
                    RB.velocity = moveDir * speed;
               //     transform.LookAt(transform.forward);

                }
                else
                {
                    CurrentIndex++;
                }



                //Vector3 leftcorner = transform.position
                // Vector3 targetPosition = Map.Instance.MapGrid.GetWorldPosition(CurrentPath[CurrentIndex].x + 0.5f, CurrentPath[CurrentIndex].y + 0.5f);
                // Vector3 position = RB.transform.position;
                // Vector3 moveDir = (targetPosition - position).normalized;
                // RB.velocity = moveDir * speed;

            }
        }

    }


}
