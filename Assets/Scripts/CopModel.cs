using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CopModel : MonoBehaviour
{
    [SerializeField] private Rigidbody2D RB;
    [SerializeField] private Map MapInstance;

    private Pathfinding pathfindingLogic;
    private Vector2[] CurrentPath;
    private int CurrentIndex;

    private int speed = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            UnityEngine.Debug.Log("click");
            int x, y;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MapInstance.MapGrid.GetXY(worldPosition, out x, out y);

            int sx, sy;
            MapInstance.MapGrid.GetXY(RB.transform.position, out sx, out sy);   
            UnityEngine.Debug.Log("{"+x+","+y+"}");

            if (pathfindingLogic == null)
            {
                pathfindingLogic = new Pathfinding(MapInstance.MapGrid, new Vector2(0, 0));
            }
            if (pathfindingLogic.isValid(x, y))
            {
                bool pathFound = pathfindingLogic.FindShortestPath(new Vector2(sx, sy), new Vector2(x, y));
                if (pathFound)
                {
                    CurrentPath = pathfindingLogic.VectorPath;
                    CurrentIndex = 0;
                    UnityEngine.Debug.Log(CurrentPath != null);
                    UnityEngine.Debug.Log(CurrentPath.Length);


                    //RB.transform.position = MapInstance.MapGrid.GetWorldPosition(x + 0.5f, y + 0.5f);
                }
            }
            
        }

        HandleMovement();


    }

    private void HandleMovement()
    {
        UnityEngine.Debug.Log(CurrentPath != null);
        if (CurrentPath != null && CurrentIndex < CurrentPath.Length)
        {
            UnityEngine.Debug.Log("enter");
            Vector2 targetPosition = MapInstance.MapGrid.GetWorldPosition(CurrentPath[CurrentIndex].x + 0.5f, CurrentPath[CurrentIndex].y + 0.5f);
           
            if (Vector3.Distance(RB.transform.position, targetPosition) > 0.1f)
            {
                Vector2 position = new Vector2(RB.transform.position.x, RB.transform.position.y);
                Vector2 moveDir = (targetPosition - position).normalized;

                RB.transform.position = position + moveDir * speed * Time.deltaTime;

            }
            else
            {
                CurrentIndex++;
            }
        }
    }


}
