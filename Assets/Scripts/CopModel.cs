using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopModel : MonoBehaviour
{
    [SerializeField] private Rigidbody2D RB;
    [SerializeField] private Map MapInstance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (!pathfinding)
        //{
        //    int x, y;
        //    MapInstance.MapGrid.GetXY(new Vector3(0, 0, 0), out x, out y);
        //    UnityEngine.Debug.Log("before pathfinding");
        //    Pathfinding p = new Pathfinding(Map.Instance.MapGrid, new Vector2(x, y), new Vector2(4, 4));
        //    UnityEngine.Debug.Log("After pathfinding");
        //    pathfinding = true;
        //}

        if (Input.GetMouseButtonDown(1))
        {
           // UnityEngine.Debug.Log("click");
            int x, y;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MapInstance.MapGrid.GetXY(worldPosition, out x, out y);

            int sx, sy;
            MapInstance.MapGrid.GetXY(RB.transform.position, out sx, out sy);   
            //UnityEngine.Debug.Log("{"+x+","+y+"}");
            Pathfinding p = new Pathfinding(Map.Instance.MapGrid, new Vector2(sx, sy), new Vector2(x, y));

            //Vector2 DirVector = RB.transform.position - MapInstance.MapGrid.GetWorldPosition()

            //RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y * 0.5f);
            RB.transform.position = MapInstance.MapGrid.GetWorldPosition(x + 0.5f, y+0.5f);
        }


    }
}
