using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    private int Width;
    private int Height;
    private int CellSize = 1;
    private Vector3 Origin;

    [SerializeField] private Tilemap TileMap;

    // Whether the debug grid lines are visible when gizmos are turned on
    [SerializeField] private bool showDebugInfo;

    public Grid<int> MapGrid;

    public static Map Instance;
    private void Awake() => Instance = this;

    // Start is called before the first frame update
    void Start()
    {
        // TileMap.cellbounds returns the bounds of the tilemap that contain placed sprites/tiles. 
        // Make the containing grid only as big as the used space. 
        Width = TileMap.cellBounds.size.x;
        Height = TileMap.cellBounds.size.y;

        // Origin the is position of the bottom left corner
        Vector3 bottomLeft = TileMap.cellBounds.min;
        Vector3 TileMapOffset = TileMap.transform.position;

        // rotate the axes to XZY
        Origin = new Vector3(bottomLeft.x, 0, bottomLeft.y) + TileMapOffset;
        MapGrid = new Grid<int>(Width, Height, CellSize, Origin);

        if (showDebugInfo)
        {
            float y = this.transform.position.y;
            Debug.Log(y);
            // Draw the perimeter of the grid
            //Debug.DrawLine(MapGrid.GetWorldPosition(0, 0, ), MapGrid.GetWorldPosition(0, Height, y), Color.white, 100f);
            //Debug.DrawLine(MapGrid.GetWorldPosition(0, 0, y), MapGrid.GetWorldPosition(Width, 0, y), Color.white, 100f);
            //Debug.DrawLine(MapGrid.GetWorldPosition(0, Height, y), MapGrid.GetWorldPosition(Width, Height, y), Color.white, 100f);
            //Debug.DrawLine(MapGrid.GetWorldPosition(Width, 0, y), MapGrid.GetWorldPosition(Width, Height, y), Color.white, 100f);

            Debug.DrawLine(MapGrid.GetWorldPosition(0, 0), MapGrid.GetWorldPosition(0, Height), Color.white, 100f);
            Debug.DrawLine(MapGrid.GetWorldPosition(0, 0), MapGrid.GetWorldPosition(Width, 0), Color.white, 100f);
            Debug.DrawLine(MapGrid.GetWorldPosition(0, Height), MapGrid.GetWorldPosition(Width, Height), Color.white, 100f);
            Debug.DrawLine(MapGrid.GetWorldPosition(Width, 0), MapGrid.GetWorldPosition(Width, Height), Color.white, 100f);
        }
       
        foreach (var pos in TileMap.cellBounds.allPositionsWithin)
        {
          
            if (TileMap.HasTile(pos))
            {
                TileBase tile = TileMap.GetTile(pos);

                Vector3 worldPos = TileMap.CellToWorld(pos);
                Debug.Log(tile.name);

                Color debugColor = Color.white;

                switch (tile.name)
                {
                    case "Building":
                        MapGrid.SetValue(worldPos, 1);
                        debugColor = Color.black;
                        break;
                    case "Sidewalk":
                        MapGrid.SetValue(worldPos, 2);
                        debugColor = Color.yellow; 
                        break;
                    default:
                        // street
                        break;
                }

                if (showDebugInfo)
                {
                    float x = worldPos.x;
                    float y = worldPos.y;
                    float z = worldPos.z;

                    //Debug.DrawLine(new Vector3(x, y + 1, z), new Vector3(x, y + 1, z + 1), debugColor, 100f);
                    //Debug.DrawLine(new Vector3(x, y + 1, z), new Vector3(x + 1, y + 1, z), debugColor, 100f);
                    //Debug.DrawLine(new Vector3(x + 1, y + 1, z), new Vector3(x + 1, y + 1, z + 1), debugColor, 100f);
                    //Debug.DrawLine(new Vector3(x, y + 1, z + 1), new Vector3(x + 1, y + 1, z + 1), debugColor, 100f);
                    
                    Debug.DrawLine(new Vector3(x, y, z), new Vector3(x, y, z + 1), debugColor, 100f);
                    Debug.DrawLine(new Vector3(x, y, z), new Vector3(x + 1, y, z), debugColor, 100f);
                    Debug.DrawLine(new Vector3(x + 1, y, z), new Vector3(x + 1, y, z + 1), debugColor, 100f);
                    Debug.DrawLine(new Vector3(x, y, z + 1), new Vector3(x + 1, y, z + 1), debugColor, 100f);

                }





            }
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
