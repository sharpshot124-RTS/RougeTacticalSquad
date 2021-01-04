using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FormationArea)), CanEditMultipleObjects]
public class FormationAreaInspector : Editor
{
    public void OnSceneGUI()
    {
        //DrawDefaultInspector();

        FormationArea area = (FormationArea) target;

        for (int i = 0; i < area.paths.Length; i++)
        {
            var points = area.paths[i].Points;

            for (int p = 0; p < points.Length; p++)
            {
                points[p].target = Handles.PositionHandle(points[p].target, Quaternion.identity);

                GUIStyle style = GUIStyle.none;
                style.normal.textColor = Color.white;
                style.fontStyle = FontStyle.Bold;

                Handles.Label(points[p].target, string.Format("Handle {0}", p), style);
            }

            area.paths[i].Points = points;
        }
    }
}
