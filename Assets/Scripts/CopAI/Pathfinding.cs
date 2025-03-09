using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

// A* algorithm adapted from: https://www.geeksforgeeks.org/a-search-algorithm/

/** Represents a node on a grid for use in an A* pathfinding algorithm */
public struct PathNode
{
    // positions of parent in the grid
    public int ParentX, ParentY;

    //positions of self in grid
    public int X, Y;

    public double fCost, gCost, hCost;
}


/** A* Pathfinding algorithm on a rectangular grid. */
public class Pathfinding
{
    private Grid<TileType> map;
    private int Height;
    private int Width;
    private Vector2 start;

    public Vector2[] VectorPath;

    public Pathfinding()
    {
        map = Map.Instance.MapGrid;
        Height = map.getHeight();
        Width = map.getWidth();
    }

    /** Returns true if a path has been found between the points */
    public bool FindShortestPath(Vector2 src, Vector2 dst)
    {

        this.start = src;

        //TODO: preintialize these so they are not reconstructed each call
        bool[,] ClosedList = new bool[Width, Height];
        PathNode[,] PathDetails = new PathNode[Width, Height];

        // reset costs for each path node on the grid. 
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                PathDetails[i, j].fCost = double.MaxValue;
                PathDetails[i, j].gCost = double.MaxValue;
                PathDetails[i, j].hCost = double.MaxValue;
                PathDetails[i, j].ParentX = -1;
                PathDetails[i, j].ParentY = -1;
                PathDetails[i, j].X = i;
                PathDetails[i, j].Y = j;
            }
        }

        // Initalize Parameters of startNode
        int x = (int)src.x;
        int y = (int)src.y;

        PathNode start = PathDetails[x, y];

        start.fCost = 0f;
        start.gCost = 0f;
        start.hCost = 0f;
        start.ParentX = x;
        start.ParentY = y;

        PathDetails[x, y] = start;

        // list of nodes we are traversing, prioritizing lower f costs. 
        SortedSet<PathNode> OpenList = new SortedSet<PathNode>(
            Comparer<PathNode>.Create((a, b) => a.fCost.CompareTo(b.fCost)));

        OpenList.Add(start);

        bool foundDest = false;

        while (OpenList.Count > 0)
        {
            PathNode p = OpenList.Min;
            OpenList.Remove(p);

            // Add vertex to closed list
            ClosedList[p.X, p.Y] = true;

            // Find 8 successors of Node
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // self
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    int NewX = p.X + i;
                    int NewY = p.Y + j;

                    // check if position is within the grid
                    if (isValid(NewX, NewY))
                    {
                        // if the new position is our target destination
                        if (NewX == (int)dst.x && NewY == (int)dst.y)
                        {
                            PathDetails[NewX, NewY].ParentX = p.X;
                            PathDetails[NewX, NewY].ParentY = p.Y;

                            TracePath(PathDetails, dst);
                            foundDest = true;
                            return true;
                        }

                        // calculate costs
                        if (!ClosedList[NewX, NewY])
                        {
                            double gNew = PathDetails[p.X, p.Y].gCost + Map.Instance.getNavCost(p.X, p.Y); // 1 is the default path cost, variable based on tile type
                            double hNew = CalculateHValue(NewX, NewY, (int)dst.x, (int)dst.y);
                            double fNew = gNew + 1.5 * hNew;

                            // update the value of the cell the current path to this node is cheaper
                            if (PathDetails[NewX, NewY].fCost == double.MaxValue || PathDetails[NewX, NewY].fCost > fNew)
                            {
                                PathDetails[NewX, NewY].fCost = fNew;
                                PathDetails[NewX, NewY].gCost = gNew;
                                PathDetails[NewX, NewY].hCost = hNew;
                                PathDetails[NewX, NewY].ParentX = p.X;
                                PathDetails[NewX, NewY].ParentY = p.Y;

                                OpenList.Add(PathDetails[NewX, NewY]);
                            }

                        }
                    }

                }
            }

        }

        if (!foundDest)
        {
            UnityEngine.Debug.Log("path not found");
        }

        return foundDest;
    }

    /** Returns true if a straight line from the src and dst does not hit any obstacles (buildings).*/
    public bool IsPathClear(Vector2 src, Vector2 dst) {

        // Initalize Parameters of startNode
        int x = (int)src.x;
        int y = (int)src.y;

        float dist = Vector2.Distance(src, dst);

        for (int i = 0; i < dist; i++)
        {
            float a = i / dist;
            int newX = (int)((src.x * (1 - a)) + (dst.x * a));
            int newY = (int)((src.y * (1 - a)) + (dst.y * a));

            if (!isValid(newX, newY)) return false;
        }
        
        return true;
    }

    /** Returns true of the given coordinates are within the bounds of the map and valid to navgate through */
    public bool isValid(int  x, int y)
    {
        return (x >= 0 && y >= 0 && x < Width && y < Height) && Map.Instance.MapGrid.GetValue(x, y) != TileType.BUILDING;
    }

    /** A Utility Function to calculate the 'h' heuristics. */
    private static double CalculateHValue(int x, int y, int dstX, int dstY)
    {
        // Return using the distance formula
        return Math.Sqrt(Math.Pow((x - dstX), 2) + Math.Pow(y - dstY, 2));
        //return Math.Pow((x - dstX), 2) + Math.Pow(y - dstY, 2);
    }

    /** Trace through the found shortest path and convert it into a vector represention 
     * starting from the start and ending at the destination*/
    private void TracePath(PathNode[,] PathDetails, Vector2 dst)
    {
        Stack<PathNode> Path =  new Stack<PathNode>();

        int x = (int) dst.x;
        int y = (int) dst.y;
        
        // find the path by traversing through each nodes parent
        while (!(PathDetails[x, y].ParentX == (int)this.start.x && PathDetails[x, y].ParentY == (int)this.start.y))
        {
            PathNode CurrentNode = PathDetails[x, y];

            Path.Push(CurrentNode);
            int TempX = CurrentNode.ParentX;
            int TempY = CurrentNode.ParentY;

            x = TempX;
            y = TempY;

        }
        // add start node to the stack
        Path.Push(PathDetails[x, y]);


        // read stack to convert to start to destination order
        VectorPath = new Vector2[Path.Count];

        int i = 0;
        
        while (Path.Count > 0)
        {
            PathNode p = Path.Pop();
            // draw the debug lines of the path onto the map
            Debug.DrawLine(map.GetWorldPosition(p.ParentX + 0.5f, p.ParentY + 0.5f), map.GetWorldPosition(p.X + 0.5f, p.Y + 0.5f), Color.green, 0.5f);
            VectorPath[i] = new Vector2(p.X, p.Y);
            i++;
        }

        
    }
}
