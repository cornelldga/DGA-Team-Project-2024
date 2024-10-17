using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 2D grid to represent the map for the AI controller */
public class Grid<TGridObject>
{
    private int Width;
    private int Height;
    private float CellSize;
    private Vector3 Origin;
    private TGridObject[,] GridArray;

    public Grid(int Width, int Height, int CellSize, Vector3 Origin)
    {
        this.Width = Width;
        this.Height = Height;   
        this.CellSize = CellSize;   
        this.Origin = Origin;

        GridArray = new TGridObject[Width, Height];

        // draw lines of the grid into the scene for debugging
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

            }


        }
        Debug.DrawLine(GetWorldPosition(0, Height), GetWorldPosition(Width, Height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(Width, 0), GetWorldPosition(Width, Height), Color.white, 100f);

    }


    /** Convert grid coordinates to world coordinates */
    public Vector3 GetWorldPosition(float x, float y)
    {
        return new Vector3(x, y) * CellSize + Origin;
    }

    /** Convert world coordinates to grid coordinates */
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.x - Origin.x)/ CellSize);
        y = Mathf.FloorToInt((worldPosition.y - Origin.y)/ CellSize);
    }

    /** Set the value stored at the grid position coordinates */
    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height) 
        {
            GridArray[x, y] = value;
        }
        
    }

    /** Sets the value stored at the grid position cooresponding to the given world position */
    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);


    }

    /** Returns the value at the given grid position */
    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            return GridArray[x, y];
        }
        // Invald Value;
        return default(TGridObject);
    }

    /** returns the value at grid position corresponding to the given world position */
    public TGridObject GetValue(Vector3 WorldPosition)
    {
        int x, y;
        GetXY(WorldPosition, out x, out y); 

        return GetValue(x, y);
    }


    public int getHeight()
    {
        return Height;
    }

    public int getWidth()
    {
        return Width;
    }



}
