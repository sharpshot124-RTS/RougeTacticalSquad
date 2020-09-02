using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BitFSM
{
    public class BitFSMEventHandler
    {
        private static BitFSMSettings settings = null;
        public static Vector2 mousePosition;

        public static void HandleEvents(Event e)
        {
            settings = BitFSMSettings.Instance;

            mousePosition = e.mousePosition;

            HandleStateEvents(e);
            HandleEditorEvents(e);
        }

        private static void HandleStateEvents(Event e)
        {
            BitFSMSettings settings = BitFSMSettings.Instance;
            if (settings.currentAI.states != null)
            {
                int stateCount = settings.currentAI.states.Count;
                Vector2 zoomedMousePosition = BitFSMRenderer.ConvertScreenCoordsToZoomCoords(e.mousePosition);

                for (int i = 0; i < stateCount; i++)
                {
                    if (settings.currentAI.states[i].ProcessEvents(e, BitFSMRenderer.zoom, zoomedMousePosition))
                    {
                        EditorUtility.SetDirty(settings.currentAI);
                    }
                }
            }
        }

        private static void HandleEditorEvents(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    BitFSMConnectionHandler.ClearConnectionMarkers();
                } else if (e.button == 1)
                {
                    BitFSMConnectionHandler.ClearConnectionMarkers();
                    ProcessContextMenu(e.mousePosition);
                }
            }

            // Allow adjusting the zoom with the mouse wheel as well. In this case, use the mouse coordinates
            // as the zoom center instead of the top left corner of the zoom area. This is achieved by
            // maintaining an origin that is used as offset when drawing any GUI elements in the zoom area.
            if (e.type == EventType.ScrollWheel)
            {
                Vector2 screenCoordsMousePos = e.mousePosition;
                Vector2 delta = e.delta;
                Vector2 zoomCoordsMousePos = BitFSMRenderer.ConvertScreenCoordsToZoomCoords(screenCoordsMousePos);
                float zoomDelta = -delta.y / 150.0f;
                float oldZoom = BitFSMRenderer.zoom;
                BitFSMRenderer.zoom += zoomDelta;
                BitFSMRenderer.zoom = Mathf.Clamp(BitFSMRenderer.zoom, BitFSMRenderer.zoomMin, BitFSMRenderer.zoomMax);

                settings.currentAI.zoom = BitFSMRenderer.zoom;

                BitFSMRenderer.zoomWindowOrigin += (zoomCoordsMousePos - BitFSMRenderer.zoomWindowOrigin) - (oldZoom / BitFSMRenderer.zoom) * (zoomCoordsMousePos - BitFSMRenderer.zoomWindowOrigin);
                settings.currentAI.zoomCoords = BitFSMRenderer.zoomWindowOrigin;
                EditorUtility.SetDirty(settings.currentAI);

                BitFSMConnectionHandler.ClearConnectionMarkers();

                e.Use();
            }

            // Allow moving the zoom area's origin by dragging with the middle mouse button or dragging
            // with the left mouse button with Alt pressed.
            if (e.type == EventType.MouseDrag &&
                (e.button == 0 && e.modifiers == EventModifiers.Alt) ||
                (e.button == 2))
            {
                Vector2 delta = -Event.current.delta;
                delta /= BitFSMRenderer.zoom;

                BitFSMRenderer.zoomWindowOrigin += delta;
                settings.currentAI.zoomCoords = BitFSMRenderer.zoomWindowOrigin;
                EditorUtility.SetDirty(settings.currentAI);

                BitFSMConnectionHandler.ClearConnectionMarkers();

                if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
                {
                    Event.current.Use();
                }
            }
        }

        private static void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add New State"), false, () => OnClickAddState(mousePosition));
            genericMenu.AddItem(new GUIContent("Add New Transition Tree State"), false, () => OnClickAddState(mousePosition, true));
            if (settings.currentAI.stateToCopy != null)
            {
                genericMenu.AddItem(new GUIContent("Paste State"), false, () => OnPasteState(mousePosition));
            }
            genericMenu.ShowAsContext();
        }

        private static void OnClickAddState(Vector2 mousePosition, bool isTransitionTreeState = false)
        {
            BitFSMEditor.editor.CreateNewState(BitFSMRenderer.ConvertScreenCoordsToZoomCoords(mousePosition), isTransitionTreeState);
        }

        private static void OnPasteState(Vector2 mousePosition)
        {
            BitFSMEditor.editor.PasteNewState(BitFSMRenderer.ConvertScreenCoordsToZoomCoords(mousePosition));
        }
    }
}
