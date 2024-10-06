using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;


public struct PathNode
{
    // positions of parent in the grid
    public int ParentX, ParentY;

    //positions of self in grid
    public int X, Y;

    public double fCost, gCost, hCost;
}


public class Pathfinding
{
    private Grid<int> map;
    private int Height;
    private int Width;
    public Pathfinding(Grid<int> map, Vector2 src, Vector2 dst)
    {
        this.Height = map.getHeight();
        this.Width = map.getWidth();

        bool[,] ClosedList = new bool[Height, Width];
        PathNode[,] PathDetails = new PathNode[Height, Width];

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
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
        int x = (int) src.x;
        int y = (int) src.y;

        PathNode start = PathDetails[x, y];

        start.fCost = 0f;
        start.gCost = 0f;
        start.hCost = 0f;
        start.ParentX = x;
        start.ParentY = y;

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

            for (int i = -1; i <=1; i++) 
            {
                for (int j = -1; j <=1; j++)
                {
                    // self
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    int NewX = p.X + i;
                    int NewY = p.Y + j;

                    if (isValid(NewX, NewY))
                    {
                        // Destination
                        if (NewX == (int)dst.x && NewY == (int)dst.y)
                        {
                            PathNode dstNode = PathDetails[NewX, NewY];
                            dstNode.ParentX = p.X;
                            dstNode.ParentY = p.Y;
                            //trace path
                            foundDest = true;
                            return;
                        }

                        // calculate costs
                        if (!ClosedList[NewX, NewY])
                        {
                            double gNew = PathDetails[p.X, p.Y].gCost + 1.0;
                            double hNew = CalculateHValue(NewX, NewY, (int)dst.x, (int)dst.y);
                            double fNew = gNew + hNew;

                            // update the value of the cell if this path is lesser than
                            if (PathDetails[NewX, NewY].fCost > fNew)
                            {
                                PathNode newNode = PathDetails[NewX, NewY];

                                OpenList.Add(newNode);

                                newNode.fCost = fNew;
                                newNode.gCost = gNew;
                                newNode.hCost = hNew;
                                newNode.ParentX = p.X;
                                newNode.ParentY = p.Y;
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

    }

    public bool isValid(int  x, int y)
    {
        return (x >= 0 && y >= 0 && x <= Width && y <= Height);
    }

    // A Utility Function to calculate the 'h' heuristics.
    public static double CalculateHValue(int row, int col, int dstX, int dstY)
    {
        // Return using the distance formula
        return Math.Sqrt(Math.Pow(row - dstX, 2) + Math.Pow(col - dstY, 2));
    }

    public static void TracePath(PathNode[,] PathDetails, Vector2 dst)
    {
        UnityEngine.Debug.Log("The path is");

        Stack<PathNode> Path =  new Stack<PathNode>();

        int x = (int) dst.x;
        int y = (int) dst.y;

        while (!(PathDetails[x, y].ParentX == x && PathDetails[x, y].ParentY == y))
        {
            Path.Push(PathDetails[x, y]);
            int TempX = PathDetails[x, y].ParentX;
            int TempY = PathDetails[x, y].ParentY;

            x = TempX;
            y = TempY;
        }

        Path.Push(PathDetails[x, y]);

        while (Path.Count > 0)
        {
            PathNode p = Path.Pop();
            UnityEngine.Debug.Log("{" + p.X + ", " +p.Y + "}");
        }
    }
}
