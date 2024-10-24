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

        Debug.Log(TileMap.cellBounds.size);
        Debug.Log(TileMap.cellBounds.max);

        Vector3 TileMapOffset = TileMap.CellToWorld(Vector3Int.FloorToInt(TileMap.cellBounds.center));
        Debug.Log(TileMap.CellToWorld(Vector3Int.FloorToInt(TileMap.cellBounds.center)));

        Width = TileMap.cellBounds.size.x;
        Height = TileMap.cellBounds.size.y;

        // Origin the is position of the bottom left corner. 
        Origin = new Vector3(-Mathf.Ceil((float)Width / 2), 0, -Mathf.Ceil((float) Height / 2)) + TileMapOffset;

        MapGrid = new Grid<int>(Width, Height, CellSize, Origin);
        //MapGrid = new Grid<int>(, TileMap.cellBounds.size.y, CellSize, Origin);

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



        foreach (var pos in TileMap.cellBounds.allPositionsWithin)
        {
            
     
            if (TileMap.HasTile(pos))
            {
                TileBase tile = TileMap.GetTile(pos);

                Vector3 worldPos = TileMap.CellToWorld(pos);
                Debug.Log(tile.name);

                Color debugColor;

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
                    int x = (int)worldPos.x;
                    int y = (int)worldPos.y;
                    int z = (int)worldPos.z;

                    Debug.DrawLine(worldPos, new Vector3(x, y, z + 1), Color.yellow, 100f);
                    Debug.DrawLine(worldPos, new Vector3(x + 1, y, z), Color.yellow, 100f);
                    Debug.DrawLine(new Vector3(x + 1, y, z), new Vector3(x + 1, y, z + 1), Color.yellow, 100f);
                    Debug.DrawLine(new Vector3(x, y, z + 1), new Vector3(x + 1, y, z + 1), Color.yellow, 100f);

                    //Debug.DrawLine(MapGrid.GetWorldPosition(mp.x, mp.y), MapGrid.GetWorldPosition(mp.x, mp.y + 1), debugColor, 100f);
                    //Debug.DrawLine(MapGrid.GetWorldPosition(mp.x, mp.y), MapGrid.GetWorldPosition(mp.x + 1, mp.y), debugColor, 100f);
                    //Debug.DrawLine(MapGrid.GetWorldPosition(mp.x + 1, mp.y), MapGrid.GetWorldPosition(mp.x + 1, mp.y + 1), debugColor, 100f);
                    //Debug.DrawLine(MapGrid.GetWorldPosition(mp.x, mp.y + 1), MapGrid.GetWorldPosition(mp.x + 1, mp.y + 1), debugColor, 100f);
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
