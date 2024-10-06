using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopModel : MonoBehaviour
{
    [SerializeField] private Rigidbody2D RB;
    [SerializeField] private Map MapInstance;

    private bool pathfinding;

    // Start is called before the first frame update
    void Start()
    {



        pathfinding = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!pathfinding)
        {
            int x, y;
            MapInstance.MapGrid.GetXY(new Vector3(0, 0, 0), out x, out y);
            UnityEngine.Debug.Log("before pathfinding");
            Pathfinding p = new Pathfinding(Map.Instance.MapGrid, new Vector2(x, y), new Vector2(4, 4));
            UnityEngine.Debug.Log("After pathfinding");
            pathfinding = true;
        }
        
    }
}
