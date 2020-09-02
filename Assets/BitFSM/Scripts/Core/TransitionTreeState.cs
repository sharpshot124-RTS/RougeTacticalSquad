using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitFSM
{
    public class TransitionTreeState : AIState
    {
#if UNITY_EDITOR

        public override void DrawStateName(GUIStyle normalStyle, Color stateColor, bool useStateColor = false)
        {
            GUIStyle stateTitleStyle = isSelected ? BitFSMSettings.Instance.nodeStyleSelected : normalStyle;
            GUI.Label(new Rect(0, 0, rect.width, LINE_HEIGHT), "[TT] " + stateName, stateTitleStyle);
        }

        public override void DrawStateWindow(float zoomXOffset, float zoomYOffset, GUIStyle normalStyle)
        {
            rectNumberOfLines = new int[4];

            float entryActionRectHeight = GetRectHeight(RectType.ENTRY);
            float updateActionRectHeight = GetRectHeight(RectType.UPDATE);
            float exitActionRectHeight = GetRectHeight(RectType.EXIT);
            float transitionRectHeight = GetRectHeight(RectType.TRANSITION);

            rect.height = /**entryActionRectHeight + updateActionRectHeight + exitActionRectHeight + **/transitionRectHeight + LINE_HEIGHT/** + (3 * 2)**/; //The last 3 * 2 is for the 3 lines seperating the actions and transitions

            //ZOOM MODIFIED RECT
            Rect _rect = new Rect(rect);
            _rect.x -= zoomXOffset;
            _rect.y -= zoomYOffset;

            GUILayout.BeginArea(_rect, stateActive ? BitFSMSettings.Instance.nodeStyleActive : normalStyle);

            DrawStateName(BitFSMSettings.Instance.nodeStyleTitle, Color.white, false);

            GUI.BeginGroup(new Rect(0, LINE_HEIGHT, rect.width, rect.height - LINE_HEIGHT));

            //DrawRect(RectType.ENTRY, LINE_HEIGHT);
            //BitFSMSettings.DrawHorizontalLine(2, BitFSMSettings.Instance.styleLineColor);

            //DrawRect(RectType.UPDATE, LINE_HEIGHT + entryActionRectHeight);
            //BitFSMSettings.DrawHorizontalLine(2, BitFSMSettings.Instance.styleLineColor);

            //DrawRect(RectType.EXIT, LINE_HEIGHT + entryActionRectHeight + updateActionRectHeight);
            //BitFSMSettings.DrawHorizontalLine(2, BitFSMSettings.Instance.styleLineColor);

            DrawRect(RectType.TRANSITION, rect.height - transitionRectHeight);

            GUI.EndGroup();

            GUILayout.EndArea();

            if (rectSizeChanged)
            {
                GUI.changed = true;
                rectSizeChanged = false;
            }

            //Draw transition connectors
            if (showTransitions)
            {
                if (transitions != null)
                {
                    for (int i = 0; i < transitions.Count; i++)
                    {
                        Rect _successRect = new Rect(transitions[i].successRect);
                        _successRect.x -= zoomXOffset;
                        _successRect.y -= zoomYOffset;
                        GUI.Box(_successRect, "", BitFSMSettings.Instance.nodeStyleConnectorSuccess);

                        if (!transitions[i].isTransitionTreeConnector)
                        {
                            Rect _failureRect = new Rect(transitions[i].failureRect);
                            _failureRect.x -= zoomXOffset;
                            _failureRect.y -= zoomYOffset;
                            GUI.Box(_failureRect, "", BitFSMSettings.Instance.nodeStyleConnectorFailure);
                        }
                    }
                }
            }
        }

#endif
    }
}
