using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BitFSM
{
    public enum RectType
    {
        ENTRY,
        UPDATE,
        EXIT,
        TRANSITION
    }

    public class AIUIState : UIState
    {
        [HideInInspector] public List<Action> entryActions;
        [HideInInspector] public List<Action> updateActions;
        [HideInInspector] public List<Action> exitActions;
        [HideInInspector] public List<Transition> transitions;

#if UNITY_EDITOR
        public Color stateColor;
        [HideInInspector] public bool showEntryActions = false;
        [HideInInspector] public bool showUpdateActions = false;
        [HideInInspector] public bool showExitActions = false;
        [HideInInspector] public bool showTransitions = false;
        protected const float SMALL_LINE_HEIGHT = 20;
        public const float NODE_WIDTH_LARGE = 250;
        public const float objFieldSize = 165;
        public const float objFieldSideMargin = 6;
        public const float paddingObjFieldToButtons = 3;

        [NonSerialized]
        protected bool rectSizeChanged = true;
        [NonSerialized]
        [HideInInspector]
        public bool stateActive = false;
        [NonSerialized]
        [HideInInspector]
        public int activeTransitionIndex = -1;
        [NonSerialized]
        [HideInInspector]
        public int activeTransitionType = 0; //0 is success; 1 is failure
        [NonSerialized]
        [HideInInspector]
        public int activeTransitionDisplayCount = 0;
        public static int maxTransitionDisplayCount = 15;

        private static string[] rectStrings = new string[] { "Entry", "Update", "Exit", "Transitions"};
        [NonSerialized]
        protected int[] rectNumberOfLines;

        public override void Init(int index, Vector2 position, string stateName, float zoom)
        {
            //rectNumberOfLines = new int[4];

            position.x -= ((NODE_WIDTH_LARGE * 0.5f) * zoom);
            rect = new Rect(position.x, position.y, NODE_WIDTH_LARGE, LINE_HEIGHT);

            inConnectorRect = new Rect(position.x + (rect.width * 0.5f) - (CONNECTOR_WIDTH * 0.5f),
                position.y - (CONNECTOR_HEIGHT * 0.5f), CONNECTOR_WIDTH, CONNECTOR_HEIGHT);

            SetStateName(stateName);
            SetIndex(index);
        }

        #region RENDERING
        public virtual void Draw(float zoomXOffset, float zoomYOffset)
        {
            DrawStateWindow(zoomXOffset, zoomYOffset, BitFSMSettings.Instance.nodeStyleNormal);
            DrawInConnector(zoomXOffset, zoomYOffset);
            if (activeTransitionDisplayCount > 0)
            {
                activeTransitionDisplayCount--;
            }
            stateActive = false;
        }

        public virtual void DrawStateWindow(float zoomXOffset, float zoomYOffset, GUIStyle normalStyle)
        {
            rectNumberOfLines = new int[4];

            float entryActionRectHeight = GetRectHeight(RectType.ENTRY);
            float updateActionRectHeight = GetRectHeight(RectType.UPDATE);
            float exitActionRectHeight = GetRectHeight(RectType.EXIT);
            float transitionRectHeight = GetRectHeight(RectType.TRANSITION);

            rect.height = entryActionRectHeight + updateActionRectHeight + exitActionRectHeight + transitionRectHeight + LINE_HEIGHT + (3 * 2); //The last 3 * 2 is for the 3 lines seperating the actions and transitions

            //ZOOM MODIFIED RECT
            Rect _rect = new Rect(rect);
            _rect.x -= zoomXOffset;
            _rect.y -= zoomYOffset;

            GUILayout.BeginArea(_rect, stateActive ? BitFSMSettings.Instance.nodeStyleActive : normalStyle);

            DrawStateName(BitFSMSettings.Instance.nodeStyleTitle, stateColor, true);

            GUI.BeginGroup(new Rect(0, LINE_HEIGHT, rect.width, rect.height - LINE_HEIGHT));

            DrawRect(RectType.ENTRY, LINE_HEIGHT);
            BitFSMSettings.DrawHorizontalLine(2, BitFSMSettings.Instance.styleLineColor);

            DrawRect(RectType.UPDATE, LINE_HEIGHT+entryActionRectHeight);
            BitFSMSettings.DrawHorizontalLine(2, BitFSMSettings.Instance.styleLineColor);

            DrawRect(RectType.EXIT, LINE_HEIGHT+entryActionRectHeight+updateActionRectHeight);
            BitFSMSettings.DrawHorizontalLine(2, BitFSMSettings.Instance.styleLineColor);

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

        public void DrawRect(RectType rectType, float rectStartHeight)
        {
            int rectTypeIndex = (int)rectType;
            bool rectVisibility = GetRectVisibility(rectType);
            bool showRect = EditorGUILayout.Foldout( rectVisibility, rectStrings[rectTypeIndex] + (rectTypeIndex<3?" Actions : ":" : ") + rectNumberOfLines[rectTypeIndex]);

            if (showRect != rectVisibility)
            {
                SetRectVisibility(rectType, showRect);
                EditorUtility.SetDirty(this);
                rectSizeChanged = true;
            }

            if (showRect)
            {
                switch (rectType)
                {
                    case RectType.ENTRY:
                        DrawActionsRect(rectType, entryActions, rectStartHeight);
                        break;
                    case RectType.UPDATE:
                        DrawActionsRect(rectType, updateActions, rectStartHeight);
                        break;
                    case RectType.EXIT:
                        DrawActionsRect(rectType, exitActions, rectStartHeight);
                        break;
                    case RectType.TRANSITION:
                        DrawTransitionsRect(rectType, transitions, rectStartHeight);
                        break;
                }
            }
        }

        private void DrawActionsRect(RectType rectType, List<Action> actions, float startHeight)
        {
            if (actions != null)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    Action currentAction = (Action)EditorGUI.ObjectField(new Rect(objFieldSideMargin, startHeight - 29 + ((i + 1) * SMALL_LINE_HEIGHT * 1.15f), objFieldSize, 16), actions[i], typeof(Action));

                    if (currentAction != actions[i])
                    {
                        actions[i] = currentAction;
                        EditorUtility.SetDirty(this);
                    }

                    GUILayout.Space(objFieldSideMargin + objFieldSize + paddingObjFieldToButtons);

                    if (GUILayout.Button(new GUIContent(BitFSMSettings.Instance.moveDownIcon), GUILayout.Width(SMALL_LINE_HEIGHT), GUILayout.Height(SMALL_LINE_HEIGHT)))
                    {
                        if (i < actions.Count - 1)
                        {
                            currentAction = actions[i];
                            Action nextAction = actions[i + 1];
                            actions[i] = nextAction;
                            actions[i + 1] = currentAction;
                        }
                    }
                    if (GUILayout.Button(new GUIContent(BitFSMSettings.Instance.moveUpIcon), GUILayout.Width(SMALL_LINE_HEIGHT), GUILayout.Height(SMALL_LINE_HEIGHT)))
                    {
                        if (i > 0)
                        {
                            currentAction = actions[i];
                            Action nextAction = actions[i - 1];
                            actions[i] = nextAction;
                            actions[i - 1] = currentAction;
                        }
                    }
                    if (GUILayout.Button(new GUIContent(BitFSMSettings.Instance.deleteIcon), GUILayout.Width(SMALL_LINE_HEIGHT), GUILayout.Height(SMALL_LINE_HEIGHT)))
                    {
                        actions.RemoveAt(i);
                        EditorUtility.SetDirty(this);
                        rectSizeChanged = true;
                    }

                    GUILayout.EndHorizontal();
                }
            }

            GUI.color = Color.green;
            if (GUILayout.Button("Add " + rectStrings[(int) rectType] + " Action", GUILayout.Height(SMALL_LINE_HEIGHT)))
            {
                actions.Add(null);
                EditorUtility.SetDirty(this);
                rectSizeChanged = true;
            }
            GUI.color = Color.white;
        }

        private void DrawTransitionsRect(RectType rectType, List<Transition> transitions, float startHeight)
        {
            bool isTransitionTreeState = this is TransitionTreeState;
            if (transitions != null)
            {
                for (int i = 0; i < transitions.Count; i++)
                {
                    GUILayout.BeginHorizontal();

                    if (!transitions[i].isTransitionTreeConnector)
                    {
                        Decision currentDecision = (Decision)EditorGUI.ObjectField(new Rect(objFieldSideMargin, startHeight - 36 + ((i + 1) * SMALL_LINE_HEIGHT * 1.15f) + (isTransitionTreeState?7:0), objFieldSize, 16), transitions[i].decision, typeof(Decision));

                        if (currentDecision != transitions[i].decision)
                        {
                            transitions[i].decision = currentDecision;
                            EditorUtility.SetDirty(transitions[i]);

                        }
                    } else
                    {
                        string labelStr = "[Transition Tree]";
                        if (transitions[i].successState != null)
                        {
                            labelStr = "[TT] "+transitions[i].successState.stateName;
                        }
                        EditorGUI.LabelField(new Rect(objFieldSideMargin, startHeight - 36 + ((i + 1) * SMALL_LINE_HEIGHT * 1.15f), objFieldSize, 16), labelStr);
                    }

                    GUILayout.Space(objFieldSideMargin + objFieldSize + paddingObjFieldToButtons);

                    if (GUILayout.Button(new GUIContent(BitFSMSettings.Instance.moveDownIcon), GUILayout.Width(SMALL_LINE_HEIGHT), GUILayout.Height(SMALL_LINE_HEIGHT)))
                    {
                        if (i < transitions.Count - 1)
                        {
                            Transition currentTransition = transitions[i];
                            Transition nextTransition = transitions[i + 1];
                            transitions[i] = nextTransition;
                            transitions[i + 1] = currentTransition;
                        }
                    }
                    if (GUILayout.Button(new GUIContent(BitFSMSettings.Instance.moveUpIcon), GUILayout.Width(SMALL_LINE_HEIGHT), GUILayout.Height(SMALL_LINE_HEIGHT)))
                    {
                        if (i > 0)
                        {
                            Transition currentTransition = transitions[i];
                            Transition nextTransition = transitions[i - 1];
                            transitions[i] = nextTransition;
                            transitions[i - 1] = currentTransition;
                        }
                    }
                    if (GUILayout.Button(new GUIContent(BitFSMSettings.Instance.deleteIcon), GUILayout.Width(SMALL_LINE_HEIGHT), GUILayout.Height(SMALL_LINE_HEIGHT)))
                    {
                        Transition prevTransition = transitions[i];
                        transitions.RemoveAt(i);
                        if (prevTransition != null)
                        {
                            DestroyImmediate(prevTransition, true);
                            EditorUtility.SetDirty(this);
                        }
                        rectSizeChanged = true;
                    }

                    GUILayout.EndHorizontal();

                    if (i < transitions.Count && transitions[i] != null)
                    {
                        //Create Failure Connector
                        if (!transitions[i].isTransitionTreeConnector)
                        {
                            transitions[i].failureRect = new Rect(rect.xMin - CONNECTOR_HEIGHT, rect.yMin + (startHeight + (SMALL_LINE_HEIGHT * 1.15f) * (1 + i)) - (CONNECTOR_WIDTH) + SMALL_LINE_HEIGHT - 8 + (isTransitionTreeState ? 7 : 0), CONNECTOR_HEIGHT, CONNECTOR_WIDTH);
                        }

                        //Create Success Connector
                        transitions[i].successRect = new Rect(rect.xMin + rect.width, rect.yMin +
                         (startHeight + (SMALL_LINE_HEIGHT * 1.15f) * (1 + i)) - (CONNECTOR_WIDTH) + SMALL_LINE_HEIGHT - 8 + (isTransitionTreeState ? 7 : 0), CONNECTOR_HEIGHT, CONNECTOR_WIDTH);
                    }
                }
            }

            GUI.color = Color.green;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Transitions", GUILayout.Height(SMALL_LINE_HEIGHT)))
            {
                Transition transition = CreateInstance<Transition>();

                AssetDatabase.AddObjectToAsset(transition, this);

                if (transitions == null)
                {
                    transitions = new List<Transition>();
                }
                transitions.Add(transition);

                EditorUtility.SetDirty(this);

                rectSizeChanged = true;
            }
            if (!isTransitionTreeState)
            {
                if (GUILayout.Button("Add Tree Connector", GUILayout.Height(SMALL_LINE_HEIGHT)))
                {
                    Transition transition = CreateInstance<Transition>();
                    transition.isTransitionTreeConnector = true;

                    AssetDatabase.AddObjectToAsset(transition, this);

                    if (transitions == null)
                    {
                        transitions = new List<Transition>();
                    }
                    transitions.Add(transition);

                    EditorUtility.SetDirty(this);

                    rectSizeChanged = true;
                }
            }
            EditorGUILayout.EndHorizontal();
            GUI.color = Color.white;
        }

        public virtual void DrawConnections(float zoomXOffset, float zoomYOffset)
        {
            if (transitions != null && showTransitions)
            {
                for (int i = 0; i < transitions.Count; i++)
                {
                    if (transitions[i].successState != null)
                    {
                        Vector2 outCenter = GetTransitionCenter(i, zoomXOffset, zoomYOffset, true);
                        Vector2 inCenter = transitions[i].successState.GetInConnectorCenter(zoomXOffset, zoomYOffset);
                        if (activeTransitionIndex == i && activeTransitionType == 0 && activeTransitionDisplayCount > 0)
                        {
                            DrawTransitionConnection(outCenter, inCenter, true, Color.Lerp(Color.white, Color.red, activeTransitionDisplayCount / ((float)maxTransitionDisplayCount)));
                        }
                        else
                        {
                            DrawTransitionConnection(outCenter, inCenter, true, Color.white);
                        }
                    }
                    if (transitions[i].failureState != null)
                    {
                        Vector2 outCenter = GetTransitionCenter(i, zoomXOffset, zoomYOffset, false);
                        Vector2 inCenter = transitions[i].failureState.GetInConnectorCenter(zoomXOffset, zoomYOffset);
                        if (activeTransitionIndex == i && activeTransitionType == 0 && activeTransitionDisplayCount > 0)
                        {
                            DrawTransitionConnection(outCenter, inCenter, false, Color.Lerp(Color.white, Color.red, activeTransitionDisplayCount / ((float)maxTransitionDisplayCount)));
                        }
                        else
                        {
                            DrawTransitionConnection(outCenter, inCenter, false, Color.white);
                        }
                    }
                }
            }
        }
        #endregion

        #region EVENTS
        public override bool ProcessEvents(Event e, float zoom, Vector2 zoomedMousePosition)
        {
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    if (showTransitions && transitions != null)
                    {
                        for (int i = 0; i < transitions.Count; i++)
                        {
                            if (transitions[i].successRect.Contains(zoomedMousePosition))
                            {
                                if (OnClickTransitionSuccess != null)
                                {
                                    OnClickTransitionSuccess(index, i);
                                    if (e.type != EventType.Repaint && e.type != EventType.Layout)
                                    {
                                        e.Use();
                                    }
                                    GUI.changed = true;
                                }
                            }
                            else if (!transitions[i].isTransitionTreeConnector && transitions[i].failureRect.Contains(zoomedMousePosition))
                            {
                                if (OnClickTransitionFailure != null)
                                {
                                    OnClickTransitionFailure(index, i);
                                    if (e.type != EventType.Repaint && e.type != EventType.Layout)
                                    {
                                        e.Use();
                                    }
                                    GUI.changed = true;
                                }
                            }
                        }
                    }
                    else if (e.button == 1)
                    {
                        if (showTransitions && transitions != null)
                        {
                            for (int i = 0; i < transitions.Count; i++)
                            {
                                if (transitions[i].successRect.Contains(zoomedMousePosition))
                                {
                                    transitions[i].successState = null;
                                    EditorUtility.SetDirty(transitions[i]);
                                    if (e.type != EventType.Repaint && e.type != EventType.Layout)
                                    {
                                        e.Use();
                                    }
                                    GUI.changed = true;
                                }
                                else if (transitions[i].failureRect.Contains(zoomedMousePosition))
                                {
                                    transitions[i].failureState = null;
                                    EditorUtility.SetDirty(transitions[i]);
                                    if (e.type != EventType.Repaint && e.type != EventType.Layout)
                                    {
                                        e.Use();
                                    }
                                    GUI.changed = true;
                                }
                            }
                        }
                    }
                }
            }
            return base.ProcessEvents(e, zoom, zoomedMousePosition);
        }
        #endregion

        #region UTILITY
        private bool GetRectVisibility(RectType rectType)
        {
            switch (rectType)
            {
                case RectType.ENTRY:
                    return showEntryActions;
                case RectType.UPDATE:
                    return showUpdateActions;
                case RectType.EXIT:
                    return showExitActions;
                case RectType.TRANSITION:
                    return showTransitions;
            }
            return false;
        }

        private void SetRectVisibility(RectType rectType, bool value)
        {
            switch (rectType)
            {
                case RectType.ENTRY:
                    showEntryActions = value;
                    break;
                case RectType.UPDATE:
                    showUpdateActions = value;
                    break;
                case RectType.EXIT:
                    showExitActions = value;
                    break;
                case RectType.TRANSITION:
                    showTransitions = value;
                    break;
            }
        }

        protected float GetRectHeight(RectType rectType, float lineHeightMultiplier = 1.15f)
        {
            int numberOfLines = 0;
            bool expanded = false;
            int rectIndex = (int)rectType;

            switch (rectType)
            {
                case RectType.ENTRY:
                    if (entryActions != null)
                    {
                        numberOfLines = entryActions.Count;
                    }
                    expanded = showEntryActions;
                    break;
                case RectType.UPDATE:
                    if (updateActions != null)
                    {
                        numberOfLines = updateActions.Count;
                    }
                    expanded = showUpdateActions;
                    break;
                case RectType.EXIT:
                    if (exitActions != null)
                    {
                        numberOfLines = exitActions.Count;
                    }
                    expanded = showExitActions;
                    break;
                case RectType.TRANSITION:
                    if (transitions != null)
                    {
                        numberOfLines = transitions.Count;
                    }
                    expanded = showTransitions;
                    break;
            }

            rectNumberOfLines[rectIndex] = numberOfLines;
            float height = (expanded ? (SMALL_LINE_HEIGHT * (numberOfLines + 2)) : SMALL_LINE_HEIGHT) * lineHeightMultiplier;
            return height;
        }

        public Vector2 GetTransitionCenter(int transitionIndex, float zoomXOffset, float zoomYOffset, bool success)
        {
            if (transitions != null && transitionIndex < transitions.Count)
            {
                //ZOOM MODIFIED RECT
                Rect _connectorRect = new Rect(success ? transitions[transitionIndex].successRect : transitions[transitionIndex].failureRect);
                _connectorRect.x -= zoomXOffset;
                _connectorRect.y -= zoomYOffset;

                return _connectorRect.center;
            }
            return Vector2.zero;
        }

        public virtual void RemoveStateFromChild(AIState stateToRemove)
        {
            if (transitions != null)
            {
                for (int i = 0; i < transitions.Count; i++)
                {
                    if (transitions[i].successState != null && transitions[i].successState.index == stateToRemove.index)
                    {
                        transitions[i].successState = null;
                        EditorUtility.SetDirty(transitions[i]);
                    }
                    if (transitions[i].failureState != null && transitions[i].failureState.index == stateToRemove.index)
                    {
                        transitions[i].failureState = null;
                        EditorUtility.SetDirty(transitions[i]);
                    }
                }
            }
        }

        public virtual void CreateConnection(AIState childState)
        {

        }

        public virtual void CreateTransitionConnection(AIState childState, int transitionIndex, bool success)
        {
            if (transitions != null && transitionIndex < transitions.Count)
            {
                if (success)
                {
                    transitions[transitionIndex].successState = childState;
                    EditorUtility.SetDirty(transitions[transitionIndex]);
                }
                else
                {
                    transitions[transitionIndex].failureState = childState;
                    EditorUtility.SetDirty(transitions[transitionIndex]);
                }
            }
        }
        #endregion
#endif
    }
}
