using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Node
{
    public enum State
    {
        None,
        Open,
        Closed
    }
    public int F = 0;   // Total estimated path length. F = G + h
    public int G = 0;   // Distance travelled so far.
    public int H = 0;   // Estimated distance remaining to target.
    public int C = 1;   // Cost of walking over this node.
    public bool Wall = false;   // Walls block movement.
    public Vector2Int Parent = new Vector2Int(-1,-1);   // The node before this one.
    public State state = State.None;    // Current node state. Could be none (not reached yet), Open (possible next node) and Closed (reached the node).
}
[Serializable]
public class Layer
{
    public string type;
    public int[] data;
}

[Serializable]
public class Level
{
    public int width;
    public int height;
    public Layer[] layers;

}

public class Pathfind : MonoBehaviour
{
    public static int gridWidth = 16;
    public static int gridHeight = 16;
    public static float cellSize = 1.0f;

    public static Node[,] Nodes;
    public Material groundMat;
    //   static Texture2D tex;

    public GameObject preefab;
    public GameObject preFabTree;

    public Level level;

    public string FilePath;

    public bool startDetroying = false;
    // Start is called before the first frame update
    void Start()
    {
        startDetroying = false;
        string[] mapData = {
            "****************",
            "*..............*",
            "*..............*",
            "*..............*",
            "*..............*",
            "*..............*",
            "*..............*",
            "*..............*",
            "*..............*",
            "*..............*",
            "*..............*",
            "*..............*",
            "*..............*",
            "*..............*",
            "*..............*",
            "****************",
        };
        //  tex = new Texture2D(GridWidth, GridHeight);
        //  tex.filterMode = FilterMode.Point;
        //  groundMat.SetTexture("_MainTex", tex);
       /* for (int z = 0; z < gridHeight; ++z)
        {
            for (int x = 0; x < gridWidth; ++x)
            {
                Nodes[gridHeight - z - 1, x] = new Node();
                if (mapData[z][x] == '*')
                {
                    Nodes[gridHeight - z - 1, x].Wall = true;


                    // Instantiate(preefab, new Vector3(x + 0.5f, 1, (gridHeight - z - 1 + 0.5f)), Quaternion.identity);

                    //      tex.SetPixel(x, GridHeight-y-1, Color.red);
                }
                else if (mapData[z][x] == '.')
                {
                    Nodes[gridHeight - z - 1, x].Wall = false;


                    // Instantiate(preFabTree, new Vector3(x + 0.5f, 1, (gridHeight - z - 1 + 0.5f)), Quaternion.identity);
                }
            }
        }*/

    }

    public void ClearMap()
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            for (int x = 0; x < gridWidth; ++x)
            {
                Nodes[y, x].Wall = false;
            }
        }
    }
    public void LoadMap()
    {

        string file = File.ReadAllText(FilePath);
        Debug.Log(file);

        level = JsonUtility.FromJson<Level>(file);
        int tilelayer = -1;

        gridHeight = level.height;
        gridWidth = level.width;


        Nodes = new Node[gridHeight, gridWidth];
        for (int i = 0; i < level.layers.Length; ++i)
        {
            if (level.layers[i].type == "tilelayer")
            {
                tilelayer = i;
                break;
            }
        }
        if (tilelayer >= 0)
        {
            for (int z = 0; z < gridHeight; ++z)
            {
                for (int x = 0; x < gridWidth; ++x)
                {
                    Nodes[gridHeight - z - 1, x] = new Node();
                    int index = x + z * gridHeight;
                    int tile = level.layers[tilelayer].data[index];
                    if (tile == 34)
                    {
                        Instantiate(preFabTree, new Vector3(x + 0.5f, 1, (gridHeight - z - 1 + 0.5f)), Quaternion.identity);
                        Nodes[gridHeight - z - 1, x].Wall = true;
                    }
                    else if (tile == 53)
                    {
                        Instantiate(preefab, new Vector3(x + 0.5f, 1, (gridHeight - z - 1 + 0.5f)), Quaternion.identity);
                        Nodes[gridHeight - z - 1, x].Wall = true;
                    }
                    else
                    {
                        Nodes[gridHeight - z - 1, x].Wall = false;
                    }

                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        for (int y = 0; y <= gridHeight; ++y)
        {
            Debug.DrawLine(new Vector3(0, 0.1f, y), new Vector3(gridWidth, 0.1f, y));
        }
        for (int x = 0; x <= gridWidth; ++x)
        {
            Debug.DrawLine(new Vector3(x, 0.1f, 0), new Vector3(x, 0.1f, gridHeight));
        }

    }

    public static Node GetNode(Vector2Int pos)
    {
        return Nodes[pos.y, pos.x];
    }

    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        Vector2Int[] directions = new Vector2Int[] {
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
            //new Vector2Int(-1, -1),
            //new Vector2Int(1, 1),
            //new Vector2Int(-1, 1),
            //new Vector2Int(1, -1)
        };

        for (int y = 0; y < gridHeight; ++y)
        {
            for (int x = 0; x < gridWidth; ++x)
            {
                Nodes[y, x].state = Node.State.None;
                Nodes[y, x].Parent = new Vector2Int(-1, -1);
                Nodes[y, x].G = 0;
            }
        }
        
        // A* goes here
        List<Vector2Int> openList = new List<Vector2Int>();

        Vector2Int currentCoord;

        openList.Add(start);

        GetNode(start).state = Node.State.Open;
        GetNode(start).G = 0;

        while (openList.Count > 0)
        {
            Vector2Int lowestFCoord = openList[0];
            int lowestF = GetNode(lowestFCoord).F;
            int lowestFIndex = 0;
            for(int i=1; i<openList.Count; ++i)
            {
                if (GetNode(openList[i]).F < lowestF)
                {
                    lowestFIndex = i;
                    lowestFCoord = openList[i];
                    lowestF = GetNode(lowestFCoord).F;
                }
            }
            currentCoord = lowestFCoord;
            Node currentNode = GetNode(currentCoord);
            currentNode.state = Node.State.Closed;
            openList.RemoveAt(lowestFIndex);
            for(int i=0;i<directions.Length; ++i)
            {
                Vector2Int adjCoord = currentCoord + directions[i];
                Node adjCoordNode = GetNode(adjCoord);
                int cost = adjCoordNode.C;
                if (adjCoordNode.Wall)
                {
                    // do nothing
                }
                else if(adjCoordNode.state == Node.State.Closed) 
                { 
                    // do nothing
                }
                else if(adjCoordNode.state == Node.State.Open)
                {
                    if(adjCoordNode.G > currentNode.G + cost)
                    {
                        adjCoordNode.G = currentNode.G + cost;
                        adjCoordNode.H = Math.Abs(adjCoord.x - end.x)+ Math.Abs(adjCoord.y - end.y);
                        adjCoordNode.F = adjCoordNode.G + adjCoordNode.H;
                        adjCoordNode.Parent = currentCoord;
                    }
                }
                else if(adjCoordNode.state == Node.State.None)
                {

                    adjCoordNode.G = currentNode.G + cost;
                    adjCoordNode.H = Math.Abs(adjCoord.x - end.x) + Math.Abs(adjCoord.y - end.y); ;
                    adjCoordNode.F = adjCoordNode.G + adjCoordNode.H;
                    adjCoordNode.Parent = currentCoord;
                    adjCoordNode.state = Node.State.Open;
                    openList.Add(adjCoord);
                }
            }
            if(GetNode(end).state == Node.State.Closed)
            {
                List<Vector2Int> path = new List<Vector2Int>();
                Vector2Int backtrsckCoord = end;
                while(backtrsckCoord.x != -1)
                {
                    path.Add(backtrsckCoord);
                    backtrsckCoord = GetNode(backtrsckCoord).Parent;
                }
                return path;
            }
        }
        // Return empty path
        return new List<Vector2Int>();

    }
}
