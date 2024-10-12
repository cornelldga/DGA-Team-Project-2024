using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject>
{
    private int Width;
    private int Height;
    private float CellSize;
    private Vector3 Origin;
    private TGridObject[,] GridArray;

    public Grid(int Width, int Height, float CellSize, Vector3 Origin)
    {
        this.Width = Width;
        this.Height = Height;   
        this.CellSize = CellSize;   
        this.Origin = Origin;

        GridArray = new TGridObject[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition((float)(x + 1.5), y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

            }
        }
               for (int x = 0; 3*(Width/10) < Width ; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition((float)(x + 1.5), y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

            }
        }
        Debug.DrawLine(GetWorldPosition(0, Height), GetWorldPosition(Width, Height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(Width, 0), GetWorldPosition(Width, Height), Color.white, 100f);

    }


    public Vector3 GetWorldPosition(float x, float y)
    {
        return new Vector3(x, y) * CellSize + Origin;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition.x - Origin.x)/ CellSize);
        y = Mathf.FloorToInt((worldPosition.y - Origin.y)/ CellSize);
    }

    public void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height) 
        {
            GridArray[x, y] = value;
        }
        
    }

    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);


    }

    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            return GridArray[x, y];
        }
        // Invald Value;
        return default(TGridObject);
    }

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
