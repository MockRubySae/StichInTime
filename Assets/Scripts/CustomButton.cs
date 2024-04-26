using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(Pathfind))]
public class CustomButton : Editor
{
   public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Pathfind pathfind = (Pathfind)target;
        if (GUILayout.Button("LoadMap"))
        {
            pathfind.LoadMap();
        }
        if (GUILayout.Button("ClearMap"))
        {
            if (Application.isPlaying)
            {
                pathfind.ClearMap();
                for (int i = 0; i < 2; i++)
                {
                    DestroyImmediate(GameObject.FindWithTag("Terrain"));
                    if (GameObject.FindWithTag("Terrain") != null)
                    {
                        i = 0;
                    }
                }
            }
            else if (GameObject.FindWithTag("Terrain") != null)
            {
                for(int i = 0; i < 2; i++)
                {
                    DestroyImmediate(GameObject.FindWithTag("Terrain"));
                    if(GameObject.FindWithTag("Terrain") != null)
                    {
                        i = 0;
                    }
                }

            }
        }
    }
}
