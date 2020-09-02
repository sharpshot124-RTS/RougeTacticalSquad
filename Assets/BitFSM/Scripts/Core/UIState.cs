using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BitFSM
{
    public class UIState : ScriptableObject
    {
        [HideInInspector] public int index;

#if UNITY_EDITOR
        [HideInInspector] public Rect rect;
        [HideInInspector] public Rect inConnectorRect;
        [HideInInspector] public Rect outConnectorRect;
        public string stateName;
        [HideInInspector] public bool hasInConnector = true;
        [HideInInspector] public bool hasOutConnector = true;
        [HideInInspector] public bool hasTransitions = true;

        [NonSerialized]
        public bool isDragged;
        [NonSerialized]
        public bool isSelected;

        [NonSerialized]
        public Action<int> OnClickOutConnector;
        [NonSerialized]
        public Action<int> OnClickInConnector;
        [NonSerialized]
        public Action<int, int> OnClickTransitionSuccess;
        [NonSerialized]
        public Action<int, int> OnClickTransitionFailure;
        [NonSerialized]
        public Action<int> OnRightClickInConnector;
        [NonSerialized]
        public Action<int> OnClickRemoveState;
        [NonSerialized]
        public Action<int> OnClickCopyState;

        public const float NODE_WIDTH = 150;
        public const float LINE_HEIGHT = 30;
        protected const float CONNECTOR_WIDTH = 20f;
        protected const float CONNECTOR_HEIGHT = 10f;

        public virtual void Init(int index, Vector2 position, string stateName, float zoom)
        {
        }

        public virtual void SetIndex(int index)
        {
            this.index = index;
        }

        public virtual void SetStateName(string stateName)
        {
            this.stateName = stateName;
        }

        #region RENDERING
        public virtual void DrawStateName(GUIStyle normalStyle, Color stateColor, bool useStateColor = false)
        {
            GUIStyle stateTitleStyle = isSelected ? BitFSMSettings.Instance.nodeStyleSelected : normalStyle;
            if (useStateColor)
            {
                GUIStyle stateColorStyle = new GUIStyle(GUI.skin.box);
                Color _stateColor = stateColor;
                _stateColor.a = 1;
                stateColorStyle.normal.background = BitFSMSettings.MakeTex(2, 2, _stateColor);
                GUI.Label(new Rect(0, 0, rect.width, LINE_HEIGHT), stateName, stateTitleStyle);
                GUI.Label(new Rect(rect.width-10, 0, 10, LINE_HEIGHT), "", stateColorStyle);
            } else
            {
                GUI.Label(new Rect(0, 0, rect.width, LINE_HEIGHT), stateName, stateTitleStyle);
            }
            //GUIStyle stateColorStyle = new GUIStyle(GUI.skin.box);
            //nodeStyleNormal.normal.background = BitFSMSettings.MakeTex(2, 2, stateColor);
            //GUI.Box(new Rect(0,0,10,LineAlignment), "", )
            
        }

        public void DrawInConnector(float zoomXOffset, float zoomYOffset)
        {
            //ZOOM MODIFIED RECT
            Rect _inConnectorRect = new Rect(inConnectorRect);
            _inConnectorRect.x -= zoomXOffset;
            _inConnectorRect.y -= zoomYOffset;

            GUI.Box(_inConnectorRect, "", BitFSMSettings.Instance.nodeStyleConnector);
        }

        public void DrawOutConnector(float zoomXOffset, float zoomYOffset)
        {
            //ZOOM MODIFIED RECT
            Rect _outConnectorRect = new Rect(outConnectorRect);
            _outConnectorRect.x -= zoomXOffset;
            _outConnectorRect.y -= zoomYOffset;

            GUI.Box(_outConnectorRect, "", BitFSMSettings.Instance.nodeStyleConnector);
        }

        public static void DrawConnection(Vector2 outCenter, Vector2 inCenter)
        {
            float multiplier = Mathf.Clamp01(Mathf.Abs(outCenter.y - inCenter.y) / 50f);
            Handles.DrawBezier(outCenter, inCenter, outCenter - Vector2.down * 50f * multiplier,
                                inCenter - Vector2.up * 50f * multiplier,
                                Color.white,
                                null,
                                2f
                                );
        }

        public static void DrawTransitionConnection(Vector2 outCenter, Vector2 inCenter, bool success, Color transitionColor)
        {
            float multiplierY = Mathf.Clamp01(Mathf.Abs(outCenter.y - inCenter.y) / 50f);
            float multiplierX = Mathf.Clamp01(Mathf.Abs(outCenter.x - inCenter.x) / 50f);
            Handles.DrawBezier(outCenter, inCenter, outCenter - (success ? -Vector2.right : Vector2.right) * 50f * multiplierX,
                                inCenter - Vector2.up * 50f * multiplierY,
                                transitionColor,
                                null,
                                2f
                                );
        }
        #endregion

        #region EVENTS
        public virtual bool ProcessEvents(Event e, float zoom, Vector2 zoomedMousePosition)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (hasInConnector && inConnectorRect.Contains(zoomedMousePosition))
                        {
                            if (OnClickInConnector != null)
                            {
                                OnClickInConnector(index);
                                e.Use();
                                GUI.changed = true;
                            }
                        }
                        else if (hasOutConnector && outConnectorRect.Contains(zoomedMousePosition))
                        {
                            if (OnClickOutConnector != null)
                            {
                                OnClickOutConnector(index);
                                e.Use();
                                GUI.changed = true;
                            }
                        }
                        else if (rect.Contains(zoomedMousePosition))
                        {
                            if (!isSelected)
                            {
                                isSelected = true;
                                SelectObjectInEditor();
                            }
                            isDragged = true;
                            GUI.changed = true;
                        }
                        else
                        {
                            if (isSelected)
                            {
                                isSelected = false;
                                GUI.changed = true;
                            }
                        }
                    } else if (e.button == 1)
                    {
                        if (hasOutConnector && outConnectorRect.Contains(zoomedMousePosition))
                        {
                            DisconnectOutConnector();
                            e.Use();
                            GUI.changed = true;
                        }
                        if (hasInConnector && inConnectorRect.Contains(zoomedMousePosition))
                        {
                            if (OnRightClickInConnector != null)
                            {
                                OnRightClickInConnector(index);
                                e.Use();
                                GUI.changed = true;
                            }
                        }
                        if (!hasOutConnector && rect.Contains(zoomedMousePosition))
                        {
                            ProcessContextMenu();
                            e.Use();
                            GUI.changed = true;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    isDragged = false;
                    break;
                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta / zoom);
                        GUI.changed = true;
                        e.Use();
                        return true;
                    }
                    break;
            }
            return false;
        }

        public virtual void Drag(Vector2 delta)
        {
            rect.position += delta;
            outConnectorRect.position += delta;
            inConnectorRect.position += delta;
        }
        #endregion

        #region UTILITY
        public virtual void DisconnectOutConnector()
        {

        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove State"), false, () => OnClickRemoveState(index));
            genericMenu.AddItem(new GUIContent("Copy State"), false, () => OnClickCopyState(index));
            genericMenu.ShowAsContext();
        }

        public virtual void SelectObjectInEditor()
        {
            Selection.activeObject = this;
        }

        public Vector2 GetInConnectorCenter(float zoomXOffset, float zoomYOffset)
        {
            //ZOOM MODIFIED RECT
            Rect _inConnectorRect = new Rect(inConnectorRect);
            _inConnectorRect.x -= zoomXOffset;
            _inConnectorRect.y -= zoomYOffset;

            return _inConnectorRect.center;
        }

        public Vector2 GetOutConnectorCenter(float zoomXOffset, float zoomYOffset)
        {
            //ZOOM MODIFIED RECT
            Rect _outConnectorRect = new Rect(outConnectorRect);
            _outConnectorRect.x -= zoomXOffset;
            _outConnectorRect.y -= zoomYOffset;

            return _outConnectorRect.center;
        }
        #endregion
#endif
    }
}
