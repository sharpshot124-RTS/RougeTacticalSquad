using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BitFSM
{
    public class BitFSMRenderer
    {
        public const float zoomMin = 0.1f;
        public const float zoomMax = 10.0f;

        public static float zoom = 1;
        public static Vector2 zoomWindowOrigin = Vector2.zero;
        public static float windowWidth;
        public static float windowHeight;
        public static bool liveUpdate = false;

        private static BitFSMSettings settings = null;

        public static void Render(float windowWidth, float windowHeight)
        {
            settings = BitFSMSettings.Instance;
            settings.SetupNodeStyles();

            BitFSMRenderer.windowWidth = windowWidth;
            BitFSMRenderer.windowHeight = windowHeight;

            GUI.color = new Color(0.3f, 0.3f, 0.3f, 1);
            GUI.Box(new Rect(0, 0, windowWidth, windowHeight), "");
            GUI.color = Color.white;

            DrawZoomArea();

            DrawNonZoomedArea();

            DrawHorizontalToolbar();
        }

        public static void DrawZoomArea()
        {
            EditorZoomArea.Begin(zoom, new Rect(0, 0, windowWidth, windowHeight));

            DrawGrid(20, 0.1f, Color.gray, 5);

            DrawNodes();

            BitFSMConnectionHandler.DrawConnections(zoomWindowOrigin.x, zoomWindowOrigin.y);

            EditorZoomArea.End();
        }

        private static void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor, int majDiv)
        {
            Vector2 zoomedWindowDimension = ZoomedWindowDimensions();

            int widthDivs = Mathf.CeilToInt(zoomedWindowDimension.x / gridSpacing);
            int heightDivs = Mathf.CeilToInt(zoomedWindowDimension.y / gridSpacing);

            Color normalColor = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
            Color majorColor = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity * 2f);

            DrawVerticalGridLines(gridSpacing, zoomedWindowDimension.y, widthDivs, majDiv, normalColor, majorColor);
            DrawHorizontalGridLines(gridSpacing, zoomedWindowDimension.x, heightDivs, majDiv, normalColor, majorColor);
        }

        private static void DrawNodes()
        {
            BitFSMSettings settings = BitFSMSettings.Instance;
            if (settings.currentAI.states != null)
            {
                int stateCount = settings.currentAI.states.Count;

                for (int i = 0; i < stateCount; i++)
                {
                    settings.currentAI.states[i].Draw(zoomWindowOrigin.x, zoomWindowOrigin.y);
                }
            }
        }

        private static void DrawVerticalGridLines(float gridSpacing, float zoomedWindowHeight, int widthDivs, int majDiv, Color normalColor, Color majorColor)
        {
            float xOffset = -zoomWindowOrigin.x / gridSpacing;
            int xOffsetNum = Mathf.FloorToInt(xOffset);
            xOffset = (xOffset - xOffsetNum) * gridSpacing;

            for (int i = 0; i < widthDivs; i++)
            {
                if ((i - xOffsetNum) % majDiv == 0)
                {
                    Handles.color = majorColor;
                }
                else
                {
                    Handles.color = normalColor;
                }
                Vector3 startPosition = new Vector3((gridSpacing * i) + xOffset, 0, 0);
                Vector3 endPosition = new Vector3((gridSpacing * i) + xOffset, zoomedWindowHeight, 0);
                Handles.DrawLine(startPosition, endPosition);
            }
        }

        private static void DrawHorizontalGridLines(float gridSpacing, float zoomedWindowWidth, int heightDivs, int majDiv, Color normalColor, Color majorColor)
        {
            float yOffset = -zoomWindowOrigin.y / (gridSpacing);
            int yOffsetNum = Mathf.FloorToInt(yOffset);
            yOffset = (yOffset - yOffsetNum) * (gridSpacing);

            for (int i = 0; i < heightDivs; i++)
            {
                if ((i - yOffsetNum) % majDiv == 0)
                {
                    Handles.color = majorColor;
                }
                else
                {
                    Handles.color = normalColor;
                }
                Vector3 startPosition = new Vector3(0, (gridSpacing * i) + yOffset, 0);
                Vector3 endPosition = new Vector3(zoomedWindowWidth, (gridSpacing * i) + yOffset, 0);
                Handles.DrawLine(startPosition, endPosition);
            }
        }

        private static Vector2 ZoomedWindowDimensions()
        {
            Vector2 topLeft = ConvertScreenCoordsToZoomCoords(Vector2.zero);
            Vector2 bottomRight = ConvertScreenCoordsToZoomCoords(new Vector2(windowWidth, windowHeight));

            Vector2 zoomedWindowDimension = new Vector2(bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);

            return zoomedWindowDimension;
        }

        public static void DrawHorizontalToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Width(windowWidth * 1.001f));

            GUILayout.FlexibleSpace();

            //if (BitFSMEditor.editor != null && BitFSMEditor.controller != null)
            //{
            //    if (BitFSMEditor.controller.liveUpdate)
            //    {
            //        GUI.color = Color.green;
            //    }
            //    else
            //    {
            //        GUI.color = Color.white;
            //    }
            //    if (GUILayout.Button("Live Update", EditorStyles.toolbarButton))
            //    {
            //        //zoomWindowOrigin = Vector2.zero;
            //        //settings.currentAI.zoomCoords = zoomWindowOrigin;
            //        //EditorUtility.SetDirty(settings.currentAI);
            //        BitFSMEditor.controller.liveUpdate = !BitFSMEditor.controller.liveUpdate;
            //    }
            //    GUI.color = Color.white;
            //}

            if (GUILayout.Button("Reset Drag Coordinates", EditorStyles.toolbarButton))
            {
                zoomWindowOrigin = Vector2.zero;
                settings.currentAI.zoomCoords = zoomWindowOrigin;
                EditorUtility.SetDirty(settings.currentAI);
            }

            if (GUILayout.Button("Reset Zoom", EditorStyles.toolbarButton))
            {
                zoom = 1;
                settings.currentAI.zoom = zoom;
                EditorUtility.SetDirty(settings.currentAI);
            }

            EditorGUILayout.LabelField("ZOOM", GUILayout.Width(186));
            float newZoom = EditorGUI.Slider(new Rect(windowWidth - 152, 1f, 150.0f, 15.0f), zoom, zoomMin, zoomMax);
            if (newZoom != zoom)
            {
                zoom = newZoom;
                settings.currentAI.zoom = zoom;
                EditorUtility.SetDirty(settings.currentAI);
            }

            GUILayout.EndHorizontal();
        }

        public static void DrawNonZoomedArea()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            labelStyle.normal.textColor = new Color(1,1,1,0.33f);
            labelStyle.fontSize = 20;
            float padding = 20f;
            GUILayout.BeginArea(new Rect(padding, padding*1.5f, windowWidth - (2* padding), windowHeight - (2.5f* padding)));
            EditorGUILayout.LabelField(settings.currentAI.name, labelStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndArea();
            
        }

        //UTILITY
        public static Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
        {
            return screenCoords / zoom + zoomWindowOrigin;
        }
    }
}
