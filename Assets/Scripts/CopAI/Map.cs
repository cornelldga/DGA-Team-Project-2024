using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MapTile
{

    public int x;
    public int y;
    public int value;
    // values
    // 0 - road (default)
    // 1 - building
    // 2 - sidewalk
}

/** 
 * Container for the grid representing the game level map. 
 * TODO: add functions to intialize objects onto the map
 */
public class Map : MonoBehaviour
{
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private int CellSize;
    [SerializeField] private Vector3 Origin;

    // TODO: change this initalization read from a json rather than manually change each value. 
    [SerializeField] private MapTile[] MapTiles;

    // Whether the debug grid lines are visible when gizmos are turned on 
    [SerializeField] private bool showDebugInfo;

    public Grid<int> MapGrid;

    public static Map Instance;
    private void Awake() => Instance = this;

    // Start is called before the first frame update
    void Start()
    {
        MapGrid = new Grid<int>(Width, Height, CellSize, Origin);

        // add special tiles
        for (int i = 0; i < MapTiles.Length; i++)
        {
            MapTile mp = MapTiles[i];
            MapGrid.SetValue(mp.x, mp.y, mp.value);

            // color tile in debug view
            if (showDebugInfo)
            {
                Color debugColor = Color.white;

                if (mp.value == 1)
                {
                    // building
                    debugColor = Color.black;
                }
                else if (mp.value == 2)
                {
                    // sidewalk
                    debugColor = Color.yellow;
                }

                Debug.DrawLine(MapGrid.GetWorldPosition(mp.x, mp.y), MapGrid.GetWorldPosition(mp.x, mp.y + 1), debugColor, 100f);
                Debug.DrawLine(MapGrid.GetWorldPosition(mp.x, mp.y), MapGrid.GetWorldPosition(mp.x + 1, mp.y), debugColor, 100f);
                Debug.DrawLine(MapGrid.GetWorldPosition(mp.x + 1, mp.y), MapGrid.GetWorldPosition(mp.x + 1, mp.y + 1), debugColor, 100f);
                Debug.DrawLine(MapGrid.GetWorldPosition(mp.x, mp.y + 1), MapGrid.GetWorldPosition(mp.x + 1, mp.y + 1), debugColor, 100f);
            }
        }

        // draw lines of the grid into the scene for debugging
        if (showDebugInfo)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Debug.DrawLine(MapGrid.GetWorldPosition(x, y), MapGrid.GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(MapGrid.GetWorldPosition(x, y), MapGrid.GetWorldPosition(x + 1, y), Color.white, 100f);

                }
            }

            Debug.DrawLine(MapGrid.GetWorldPosition(0, Height), MapGrid.GetWorldPosition(Width, Height), Color.white, 100f);
            Debug.DrawLine(MapGrid.GetWorldPosition(Width, 0), MapGrid.GetWorldPosition(Width, Height), Color.white, 100f);

        }
    }

    public static float getNavCost(int x, int y, Grid<int> mGrid)
    {
        int TileValue = mGrid.GetValue(x, y);

        if (TileValue == 2)
        {
            // side walk has higher nav cost
            return 3.0f;
        }
        return 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
