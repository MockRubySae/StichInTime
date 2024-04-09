using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        string[] mapData = { 
            "****************",
            "*.......*......*",
            "*...,..........*",
            "*..............*",
            "*.....****...,.*",
            "*.,...*........*",
            "*.....*........*",
            "*.....*........*",
            "*.....******...*",
            "*.....*........*",
            "*.,............*",
            "*.....*****....*",
            "*.....*****.,..*",
            "*..............*",
            "*.....,........*",
            "****************",
        };

      //  tex = new Texture2D(GridWidth, GridHeight);
      //  tex.filterMode = FilterMode.Point;
      //  groundMat.SetTexture("_MainTex", tex);

        Nodes = new Node[gridHeight, gridWidth];

        for (int z = 0; z < gridHeight; ++z)
        {
            for (int x = 0; x < gridWidth; ++x)
            {
                Nodes[gridHeight - z - 1, x] = new Node();
                if (mapData[z][x] == '*')
                {
                    Nodes[gridHeight - z - 1, x].Wall = true;
                  
                       
                            Instantiate(preefab, new Vector3(x + 0.5f, 1 ,(gridHeight - z - 1 + 0.5f)), Quaternion.identity);
                        
                        //      tex.SetPixel(x, GridHeight-y-1, Color.red);
                }
                else if (mapData[z][x] == ',')
                {
                    Nodes[gridHeight - z - 1, x].Wall = true;


                    Instantiate(preFabTree, new Vector3(x + 0.5f, 1, (gridHeight - z - 1 + 0.5f)), Quaternion.identity);
                }
                else
                    {
                        Nodes[gridHeight - z - 1, x].Wall = false;
                        //     tex.SetPixel(x, GridHeight-y-1, Color.black);
                    }
                }
            } 
  
      //  tex.Apply();
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

        // Return empty path
        return new List<Vector2Int>();

    }
}
