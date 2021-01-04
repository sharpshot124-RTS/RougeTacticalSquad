using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class NoiseInspector : Editor
{
    Texture2D preview = null;

    SerializedProperty frequency, lacunarity, persistence, octaves, seed;

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        try
        {
            frequency = DrawField("frequency");
        }
        catch (NullReferenceException) { }

        try
        {
            lacunarity = DrawField("lacunarity");
        }
        catch (NullReferenceException) { }

        try
        {
            persistence = DrawField("persistence");
        }
        catch (NullReferenceException) { }

        try
        {
            octaves = DrawField("octaves");
        }
        catch (NullReferenceException) { }

        try
        {
            seed = DrawField("seed");
        }
        catch (NullReferenceException) { }

        if (preview == null || EditorGUI.EndChangeCheck())
        {
            preview = (target as INoise).GetTexture(Vector2.zero, .1f, new Vector2Int(Screen.width / 2, Screen.width / 2));
        }

        GUILayout.Box(preview);

        Repaint();
    }

    SerializedProperty DrawField(string propertyName)
    {
        var prop = serializedObject.FindProperty(propertyName);

        if (prop.type == "float")
            prop.floatValue = EditorGUILayout.FloatField(propertyName, prop.floatValue);
        else if (prop.type == "int")
            prop.intValue = EditorGUILayout.IntField(propertyName, prop.intValue);

        return prop;
    }
}

[CustomEditor(typeof(BillowGenerator))]
public class BillowInspector : NoiseInspector { }

[CustomEditor(typeof(PerlinGenerator))]
public class PerlinInspector : NoiseInspector { }

[CustomEditor(typeof(VoronoiGenerator))]
public class VoronoiInspector : NoiseInspector { }

[CustomEditor(typeof(VoronoiNearestGenerator))]
public class VoronoiNearestInspector : NoiseInspector { }

[CustomEditor(typeof(SpheresGenerator))]
public class SpheresInspector : NoiseInspector { }

[CustomEditor(typeof(CylindersGenerator))]
public class CylindersInspector : NoiseInspector { }

[CustomEditor(typeof(ConstGenerator))]
public class ConstInspector : NoiseInspector { }

[CustomEditor(typeof(RidgedMultifractalGenerator))]
public class RidgedMultifractalInspector : NoiseInspector { }

[CustomEditor(typeof(CheckerGenerator))]
public class CheckerInspector : NoiseInspector { }