using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Navigation type
public enum NavState
{
    HOTPURSUIT,     // Travel directly towards the target position via the most direct path. 
    WANDER,         // Drive through the streets to cover the most distance 
    IDLE            //  Cop is not moving and staying still in current location. 
}


/** 
 * Represents a cop that navs towards the given postion 
 * TODO: add different cop behaviors rather than following a point 
 */
public class CopModel : MonoBehaviour
{
    [SerializeField] private Rigidbody RB;
    [SerializeField] private NavState State;

    // Pathfinding parameters
    private Pathfinding pathfindingLogic;
    // The path the cop is heading towards
    private Vector2[] CurrentPath;
    // Position along the path that cop is at
    private int CurrentIndex;

    // Movement speed multiplier towards the target
    private int speed = 8;

    public NavState getNavState()
    {
        return State;
    }

    // Update is called once per frame
    void Update()
    {
        if (State == NavState.WANDER && (CurrentPath == null || CurrentIndex >= CurrentPath.Length))
        {
            int sx, sy;
            Map.Instance.MapGrid.GetXY(RB.transform.position, out sx, out sy);
            SetTarget(sx + Random.Range(-5, 5), sy + Random.Range(-5, 5));
        } 
        else if (State == NavState.HOTPURSUIT)
        {
            SetTarget(GameManager.Instance.getPlayer().transform.position);
            transform.LookAt(GameManager.Instance.getPlayer().transform.position);
        }

        HandleMovement();

    }
  
    



    public void SetTarget(Vector3 WorldPosition)
    {
        int x, y;
        Map.Instance.MapGrid.GetXY(WorldPosition, out x, out y);

        SetTarget (x, y);

    }

    public void SetTarget(int dx, int dy)
    {
        int sx, sy;
        Map.Instance.MapGrid.GetXY(RB.transform.position, out sx, out sy);
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
            if (State == NavState.HOTPURSUIT){
            Vector3 targetPosition = Map.Instance.MapGrid.GetWorldPosition(CurrentPath[CurrentIndex].x + 0.5f, CurrentPath[CurrentIndex].y + 0.5f);
           
            if (Vector3.Distance(RB.transform.position, targetPosition) > 0.5f)
            {
                Vector3 position = RB.transform.position;
                Vector3 moveDir = (targetPosition - position).normalized;

                // TODO change to vel once able to prevent cops from being knocked off the map. 
                //RB.transform.position = position + moveDir * speed * Time.deltaTime;
                RB.velocity = moveDir * speed;
            }
            else
            {
                CurrentIndex++;
                //RB.velocity = Vector2.zero;
            }
        }
        else if (State == NavState.WANDER){
                Vector3 targetPosition = Map.Instance.MapGrid.GetWorldPosition(CurrentPath[CurrentIndex].x + 0.5f, CurrentPath[CurrentIndex].y + 0.5f);
                Vector3 position = RB.transform.position;
                Vector3 moveDir = (targetPosition - position).normalized;
                RB.velocity = moveDir * speed;
            
        }
    }
    
    }

IEnumerator wanderWait(){
        yield return new WaitForSeconds(2);
}
}
