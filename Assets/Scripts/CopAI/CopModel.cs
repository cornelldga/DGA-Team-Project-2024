using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/** 
 * Represents a cop that navs towards the given postion 
 * TODO: add different cop behaviors rather than following a point 
 */
public class CopModel : MonoBehaviour
{
    [SerializeField] private Rigidbody2D RB;

    // Pathfinding parameters
    private Pathfinding pathfindingLogic;
    // The path the cop is heading towards
    private Vector2[] CurrentPath;
    // Position along the path that cop is at
    private int CurrentIndex;

    // Movement speed multiplier towards the target
    private int speed = 5;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }


    public void SetTarget(Vector3 WorldPosition)
    {
        int x, y;
        Map.Instance.MapGrid.GetXY(WorldPosition, out x, out y);

        int sx, sy;
        Map.Instance.MapGrid.GetXY(RB.transform.position, out sx, out sy);
        //UnityEngine.Debug.Log("{" + x + "," + y + "}");

        if (pathfindingLogic == null)
        {
            // initalize this here to ensure that map grid in the map instance has be initalized. 
            pathfindingLogic = new Pathfinding();
        }
        if (pathfindingLogic.isValid(x, y))
        {
            bool pathFound = pathfindingLogic.FindShortestPath(new Vector2(sx, sy), new Vector2(x, y));
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
            Vector2 targetPosition = Map.Instance.MapGrid.GetWorldPosition(CurrentPath[CurrentIndex].x + 0.5f, CurrentPath[CurrentIndex].y + 0.5f);
           
            if (Vector3.Distance(RB.transform.position, targetPosition) > 0.5f)
            {
                Vector2 position = new Vector2(RB.transform.position.x, RB.transform.position.y);
                Vector2 moveDir = (targetPosition - position).normalized;

                // TODO change to vel once able to prevent cops from being knocked off the map. 
                RB.transform.position = position + moveDir * speed * Time.deltaTime;
                //RB.velocity = moveDir * speed;
            }
            else
            {
                CurrentIndex++;

                //RB.velocity = Vector2.zero;
            }
        }
    }


}
