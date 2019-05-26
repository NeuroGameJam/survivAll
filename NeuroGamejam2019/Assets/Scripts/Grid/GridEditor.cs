using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralGrid))]
public class GridEditor : Editor
{
    private ProceduralGrid proceduralGrid;
    private void OnDisable()
    {
        proceduralGrid.GetHexagons();
    }
    private void OnEnable()
    {
        proceduralGrid = FindObjectOfType<ProceduralGrid>();
        proceduralGrid.GetHexagons();

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Build Level"))
        {
            proceduralGrid.ClearLevel();
            proceduralGrid.GenerateGrid();
        }
    }
}

