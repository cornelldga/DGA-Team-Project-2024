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

    [SerializeField] private MapTile[] MapTiles;

    public Grid<int> MapGrid;

    public static Map Instance;
    private void Awake() => Instance = this;

    // Start is called before the first frame update
    void Start()
    {
        MapGrid = new Grid<int>(Width, Height, CellSize, Origin);

        UnityEngine.Debug.Log(MapTiles.Length);
        for (int i = 0; i < MapTiles.Length; i++)
        {
            MapTile mp = MapTiles[i];
            MapGrid.SetValue(mp.x, mp.y, mp.value);

            if (mp.value == 1)
            {
                // building
                Debug.DrawLine(MapGrid.GetWorldPosition(mp.x, mp.y), MapGrid.GetWorldPosition(mp.x, mp.y + 1), Color.black, 100f);
                Debug.DrawLine(MapGrid.GetWorldPosition(mp.x, mp.y), MapGrid.GetWorldPosition(mp.x + 1, mp.y), Color.black, 100f);
                Debug.DrawLine(MapGrid.GetWorldPosition(mp.x + 1, mp.y), MapGrid.GetWorldPosition(mp.x + 1, mp.y + 1), Color.black, 100f);
                Debug.DrawLine(MapGrid.GetWorldPosition(mp.x, mp.y + 1), MapGrid.GetWorldPosition(mp.x + 1, mp.y + 1), Color.black, 100f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
