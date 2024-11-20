using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// What each tile can represent. Each type has a different navigation cost. 
public enum TileType
{
    ROAD = 0, // default. Road has the cheapest cost to travel. 
    BUILDING, // cannot move be navigated through. (under normal circumstances)
    SIDEWALK  // Placed around buildings. Less preferred path of travel but will be taken if necessary
}

/** 
 * Container for the grid representing the game level map. 
 * TODO: add functions to intialize objects onto the map
 */
public class Map : MonoBehaviour
{
    public static Map Instance;
    private void Awake() => Instance = this;

    // Tilemap used to mark navigatable areas of the pathfinding.
    [SerializeField] private Tilemap TileMap;
    [SerializeField] private TilemapRenderer TilemapRenderer;

    // Whether the debug grid lines are visible when gizmos are turned on
    [SerializeField] private bool showDebugInfo;

    // Map Grid initalization paramters
    private int Width;
    private int Height;
    private int CellSize = 1;
    private Vector3 Origin;

    // Abstraction of the tilemap to be used for pathfinding a* calculations
    public Grid<TileType> MapGrid;

    // Start is called before the first frame update
    void Start()
    {
        TilemapRenderer.enabled = false;

        // TileMap.cellbounds returns the bounds of the tilemap that contain placed sprites/tiles. 
        // Make the containing grid only as big as the used space. 
        Width = TileMap.cellBounds.size.x;
        Height = TileMap.cellBounds.size.y;

        // Origin the is position of the bottom left corner
        Vector3 bottomLeft = TileMap.cellBounds.min;
        Vector3 TileMapOffset = TileMap.transform.position;

        // rotate the axes to XZY
        Origin = new Vector3(bottomLeft.x, 0, bottomLeft.y) + TileMapOffset;
        MapGrid = new Grid<TileType>(Width, Height, CellSize, Origin);


        if (showDebugInfo)
        {
            // Draw the perimeter of the grid
 
            Debug.DrawLine(MapGrid.GetWorldPosition(0, 0), MapGrid.GetWorldPosition(0, Height), Color.white, 100f);
            Debug.DrawLine(MapGrid.GetWorldPosition(0, 0), MapGrid.GetWorldPosition(Width, 0), Color.white, 100f);
            Debug.DrawLine(MapGrid.GetWorldPosition(0, Height), MapGrid.GetWorldPosition(Width, Height), Color.white, 100f);
            Debug.DrawLine(MapGrid.GetWorldPosition(Width, 0), MapGrid.GetWorldPosition(Width, Height), Color.white, 100f);
        }
       

        foreach (var pos in TileMap.cellBounds.allPositionsWithin)
        {
            // add placed tiles to the map representation as their correct type. 
            if (TileMap.HasTile(pos))
            {
                TileBase tile = TileMap.GetTile(pos);
                Vector3 worldPos = TileMap.CellToWorld(pos);
                Color debugColor = Color.white;

                switch (tile.name)
                {
                    case "Building":
                        MapGrid.SetValue(worldPos, TileType.BUILDING);
                        debugColor = Color.black;
                        break;
                    case "Sidewalk":
                        MapGrid.SetValue(worldPos, TileType.SIDEWALK);
                        debugColor = Color.yellow; 
                        break;
                    default:
                        // street
                        break;
                }

                // draw outline around tile square in scene 
                if (showDebugInfo)
                {
                    float x = worldPos.x;
                    float y = worldPos.y;
                    float z = worldPos.z;
                    
                    Debug.DrawLine(new Vector3(x, y, z), new Vector3(x, y, z + 1), debugColor, 100f);
                    Debug.DrawLine(new Vector3(x, y, z), new Vector3(x + 1, y, z), debugColor, 100f);
                    Debug.DrawLine(new Vector3(x + 1, y, z), new Vector3(x + 1, y, z + 1), debugColor, 100f);
                    Debug.DrawLine(new Vector3(x, y, z + 1), new Vector3(x + 1, y, z + 1), debugColor, 100f);

                }

            }
        }


       
    }

    // Returns the cost of navigation on the current tile represented by the given grid coordinates. 
    public float getNavCost(int x, int y)
    {
        switch (MapGrid.GetValue(x, y))
        {
            case TileType.BUILDING:
                // cost is high enough that it will not be traversed unless absolutely necessary.
                // ie. the cop is caught and needs to free themselves
                return 100f; 
            
            case TileType.SIDEWALK:
                // sidewalk has a higher nav cost than the road.
                return 5.0f; 
            
            default :
                // road
                return 1.0f;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
