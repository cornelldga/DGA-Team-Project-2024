using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

// https://www.geeksforgeeks.org/a-search-algorithm/

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
    private Vector2 start;
    public Pathfinding(Grid<int> map, Vector2 src, Vector2 dst)
    {
        this.Height = map.getHeight();
        this.Width = map.getWidth();
        this.start = src;
        this.map = map;

        UnityEngine.Debug.Log("start: {" + src.x + ", " + src.y + "}");
        UnityEngine.Debug.Log("end: {" + dst.x + ", " + dst.y + "}");

        bool[,] ClosedList = new bool[Width, Height];
        PathNode[,] PathDetails = new PathNode[Width, Height];

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
        int x = (int) src.x;
        int y = (int) src.y;

        PathNode start = PathDetails[x, y];

        start.fCost = 0f;
        start.gCost = 0f;
        start.hCost = 0f;
        start.ParentX = x;
        start.ParentY = y;

        PathDetails[x, y] = start;

        SortedSet<PathNode> OpenList = new SortedSet<PathNode>(
            Comparer<PathNode>.Create((a, b) => a.fCost.CompareTo(b.fCost)));

        OpenList.Add(start);

        bool foundDest = false;

        while (OpenList.Count > 0)
        {
            PathNode p = OpenList.Min;
            OpenList.Remove(p);

            UnityEngine.Debug.Log("{" + p.X+", "+p.Y+"}");


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
                        UnityEngine.Debug.Log("New {" + NewX + ", " + NewY + "}");
                        // Destination
                        if (NewX == (int)dst.x && NewY == (int)dst.y)
                        {
                            UnityEngine.Debug.Log("Destination found");
                            UnityEngine.Debug.Log("dst {" +dst.x + ", " + dst.y + "}");
                            
                            PathDetails[NewX, NewY].ParentX = p.X;
                            PathDetails[NewX, NewY].ParentY = p.Y;

                            TracePath(PathDetails, dst);
                            foundDest = true;
                            return;
                        }

                        // calculate costs
                        if (!ClosedList[NewX, NewY])
                        {
                            UnityEngine.Debug.Log("calculate costs");
                            double gNew = PathDetails[p.X, p.Y].gCost + 1.0; // 1 is the path cost
                            double hNew = CalculateHValue(NewX, NewY, (int)dst.x, (int)dst.y);
                            double fNew = gNew + hNew;
                            UnityEngine.Debug.Log("h: " + hNew + ", g: " + gNew +", f: " + fNew);

                            // update the value of the cell if this path is lesser than
                            if (PathDetails[NewX, NewY].fCost == double.MaxValue || PathDetails[NewX, NewY].fCost > fNew)
                            {
                                UnityEngine.Debug.Log("add new node costs");


                                PathDetails[NewX, NewY].fCost = fNew;
                                PathDetails[NewX, NewY].gCost = gNew;
                                PathDetails[NewX, NewY].hCost = hNew;
                                PathDetails[NewX, NewY].ParentX = p.X;
                                PathDetails[NewX, NewY].ParentY = p.Y;

                                OpenList.Add(PathDetails[NewX, NewY]);

                                UnityEngine.Debug.Log("New Parent {" + PathDetails[NewX, NewY].ParentX + ", " + PathDetails[NewX, NewY].ParentY + "}");
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
        return (x >= 0 && y >= 0 && x < Width && y < Height);
    }

    // A Utility Function to calculate the 'h' heuristics.
    public static double CalculateHValue(int x, int y, int dstX, int dstY)
    {
        // Return using the distance formula
        return Math.Sqrt(Math.Pow((x - dstX), 2) + Math.Pow(y - dstY, 2));
        //return Math.Pow((x - dstX), 2) + Math.Pow(y - dstY, 2);
    }

    public void TracePath(PathNode[,] PathDetails, Vector2 dst)
    {
        UnityEngine.Debug.Log("The path is");

        Stack<PathNode> Path =  new Stack<PathNode>();

        int x = (int) dst.x;
        int y = (int) dst.y;
        UnityEngine.Debug.Log("Dst Parent {" + PathDetails[x, y].ParentX + ", " + PathDetails[x, y].ParentY + "}");

        UnityEngine.Debug.Log(PathDetails.Length);
        
        while (!(PathDetails[x, y].ParentX == (int)this.start.x && PathDetails[x, y].ParentY == (int)this.start.y))
        {
            UnityEngine.Debug.Log("{" + x + ", " + y + "}");

            PathNode CurrentNode = PathDetails[x, y];

            Path.Push(CurrentNode);
            int TempX = CurrentNode.ParentX;
            UnityEngine.Debug.Log("tempX: " + TempX);
            int TempY = CurrentNode.ParentY;
            UnityEngine.Debug.Log("tempY: " + TempY);


            x = TempX;
            y = TempY;

        }

        Path.Push(PathDetails[x, y]);

        while (Path.Count > 0)
        {
            PathNode p = Path.Pop();
            Debug.DrawLine(map.GetWorldPosition(p.ParentX + 0.5f, p.ParentY + 0.5f), map.GetWorldPosition(p.X + 0.5f, p.Y + 0.5f), Color.green, 100f);
            UnityEngine.Debug.Log("{" + p.X + ", " +p.Y + "}");
        }
    }
}
