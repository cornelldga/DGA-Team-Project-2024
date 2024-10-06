using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopModel : MonoBehaviour
{
    [SerializeField] private Rigidbody2D RB;
    [SerializeField] private Map MapInstance;

    private Pathfinding pathfindingLogic;

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
                    Vector2[] Path = pathfindingLogic.Path;
                    RB.transform.position = MapInstance.MapGrid.GetWorldPosition(x + 0.5f, y + 0.5f);
                }
            }
            
        }


    }
}
