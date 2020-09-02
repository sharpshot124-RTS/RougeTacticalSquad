using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BitFSM
{
    public class EntryState : AIState
    {
        public AIState initialState = null;

        public override void OnUpdate(AIStateController controller)
        {
            controller.TransitionToState(initialState);
        }

#if UNITY_EDITOR
        public override void Init(int index, Vector2 position, string stateName, float zoom)
        {
            float xMin = position.x - ((NODE_WIDTH * 0.5f) * zoom);
            rect = new Rect(xMin, position.y, NODE_WIDTH, LINE_HEIGHT);

            outConnectorRect = new Rect(position.x - (CONNECTOR_WIDTH * 0.5f),
                position.y + LINE_HEIGHT - (CONNECTOR_HEIGHT * 0.5f), CONNECTOR_WIDTH, CONNECTOR_HEIGHT);

            SetStateName(stateName);
            SetIndex(index);
        }

        public override void Draw(float zoomXOffset, float zoomYOffset)
        {
            DrawStateWindow(zoomXOffset, zoomYOffset, BitFSMSettings.Instance.nodeStyleEntry);
            DrawOutConnector(zoomXOffset, zoomYOffset);
        }

        public override void DrawStateWindow(float zoomXOffset, float zoomYOffset, GUIStyle normalStyle)
        {
            GUIStyle style = isSelected ? BitFSMSettings.Instance.nodeStyleSelected : normalStyle;

            //ZOOM MODIFIED RECT
            Rect _rect = new Rect(rect);
            _rect.x -= zoomXOffset;
            _rect.y -= zoomYOffset;

            GUILayout.BeginArea(_rect, style);
            DrawStateName(BitFSMSettings.Instance.nodeStyleEntry, Color.white, false);
            GUILayout.EndArea();
        }

        public override void DrawConnections(float zoomXOffset, float zoomYOffset)
        {
            if (initialState != null)
            {
                Vector2 outCenter = GetOutConnectorCenter(zoomXOffset, zoomYOffset);
                Vector2 inCenter = initialState.GetInConnectorCenter(zoomXOffset, zoomYOffset);

                DrawConnection(outCenter, inCenter);
            }
        }

        public void ResetInitialState()
        {
            initialState = null;
        }

        public override void CreateConnection(AIState childState)
        {
            initialState = childState;
        }

        public override void RemoveStateFromChild(AIState stateToRemove)
        {
            if (initialState != null && initialState.index == stateToRemove.index)
            {
                initialState = null;
                EditorUtility.SetDirty(this);
            }
        }

        public override void DisconnectOutConnector()
        {
            if (initialState != null)
            {
                initialState = null;
                EditorUtility.SetDirty(this);
            }
        }
#endif
    }
}
