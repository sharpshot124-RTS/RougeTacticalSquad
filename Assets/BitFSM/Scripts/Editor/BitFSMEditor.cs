using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BitFSM
{
    public class BitFSMEditor : EditorWindow
    {
        public static BitFSMEditor editor = null;
        public static BitFSMSettings settings = null;
        public static float timeSinceLastUpdate = float.MinValue;
        public static AIStateController controller;
        public float updateRate = 0.5f; //in seconds
        public GameObject prevSelectedGO = null;

        private void OnEnable()
        {
            if (settings == null)
            {
                settings = BitFSMSettings.Instance;
            }

            BitFSMRenderer.zoom = settings.currentAI.zoom;
            BitFSMRenderer.zoomWindowOrigin = settings.currentAI.zoomCoords;

            InitializeAI();

            editor = this;
        }

        [UnityEditor.Callbacks.OnOpenAsset(1)]
        private static bool AutoOpenEditor(int instanceID, int line)
        {
            if (Selection.activeObject != null && Selection.activeObject is BitFSM)
            {
                settings = BitFSMSettings.Instance;
                if (settings != null)
                {
                    settings.currentAI = Selection.activeObject as BitFSM;
                    EditorUtility.SetDirty(settings);
                    OpenEditor();
                    return true;
                }
                else
                {
                    Debug.Log("No Settings file found. Please create one.");
                }
            }
            return false;
        }

        public static BitFSMEditor OpenEditor()
        {
            editor = GetWindow<BitFSMEditor>();
            editor.minSize = new Vector2(800, 600);
            editor.titleContent = new GUIContent("BitFSM Editor");

            editor.InitializeAI();

            BitFSMRenderer.zoom = settings.currentAI.zoom;
            BitFSMRenderer.zoomWindowOrigin = settings.currentAI.zoomCoords;

            return editor;
        }

        private void InitializeAI()
        {
            if (settings.currentAI != null && settings.currentAI.states == null)
            {
                CreateEntryState();
            }

            RefreshStateConnections();
        }

        private void CreateEntryState()
        {
            BitFSMRenderer.zoom = settings.currentAI.zoom;
            BitFSMRenderer.zoomWindowOrigin = settings.currentAI.zoomCoords;
            settings.currentAI.states = new List<AIState>();

            //Place entry state
            float xPos = 160;
            float yPos = UIState.NODE_WIDTH;

            EntryState entryState = CreateInstance<EntryState>();
            entryState.hasInConnector = false;
            entryState.hasTransitions = false;
            entryState.Init(0, new Vector2(xPos, yPos), "Entry", BitFSMRenderer.zoom);
            entryState.hideFlags = HideFlags.HideInInspector;
            settings.currentAI.states.Add(entryState);

            AssetDatabase.AddObjectToAsset(entryState, settings.currentAI);

            EditorUtility.SetDirty(settings.currentAI);
        }

        private void RefreshStateConnections()
        {
            if (settings.currentAI.states != null)
            {
                int stateCount = settings.currentAI.states.Count;

                for (int i = 0; i < stateCount; i++)
                {
                    AIState currentState = settings.currentAI.states[i];

                    if (currentState.hasInConnector)
                    {
                        currentState.OnClickInConnector = BitFSMConnectionHandler.OnClickInConnector;
                        currentState.OnRightClickInConnector = BitFSMConnectionHandler.OnRightClickInConnector;
                        currentState.OnClickRemoveState = BitFSMConnectionHandler.OnRemoveState;
                        currentState.OnClickCopyState = BitFSMConnectionHandler.OnCopyState;
                    }
                    if (currentState.hasOutConnector)
                    {
                        currentState.OnClickOutConnector = BitFSMConnectionHandler.OnClickOutConnector;
                    }
                    if (currentState.hasTransitions)
                    {
                        currentState.OnClickTransitionFailure = BitFSMConnectionHandler.OnClickTransitionFailureConnector;
                        currentState.OnClickTransitionSuccess = BitFSMConnectionHandler.OnClickTransitionSuccessConnector;
                    }
                }
            }
            ClearStateLiveData();
        }

        private void ClearStateLiveData()
        {
            if (settings.currentAI.states != null)
            {
                int stateCount = settings.currentAI.states.Count;

                for (int i = 0; i < stateCount; i++)
                {
                    AIState currentState = settings.currentAI.states[i];

                    //Reset transition debug data
                    currentState.activeTransitionDisplayCount = 0;
                    currentState.activeTransitionIndex = -1;
                    currentState.activeTransitionType = 0;
                }
            }
        }

        public void CreateNewState(Vector2 mousePosition, bool isTransitionTreeState)
        {
            //float xPos = mousePosition.x - (State.NODE_WIDTH * 0.5f);
            //float yPos = mousePosition.y - (State.LINE_HEIGHT * 0.5f);
            float xPos = mousePosition.x;
            float yPos = mousePosition.y;

            AIState newState;
            if (!isTransitionTreeState)
            {
                newState = CreateInstance<AIState>();
            } else
            {
                newState = CreateInstance<TransitionTreeState>();
            }

            int stateIndex = settings.currentAI.states.Count;
            string stateName = "State_" + stateIndex;

            newState.Init(stateIndex, new Vector2(xPos, yPos), stateName, BitFSMRenderer.zoom);
            InitializeNewState(newState);

            settings.currentAI.states.Add(newState);

            AssetDatabase.AddObjectToAsset(newState, settings.currentAI);

            EditorUtility.SetDirty(settings.currentAI);
        }

        public void PasteNewState(Vector2 mousePosition)
        {
            float xPos = mousePosition.x;
            float yPos = mousePosition.y;

            AIState newState = Instantiate(settings.currentAI.stateToCopy);

            int stateIndex = settings.currentAI.states.Count;
            string stateName = settings.currentAI.stateToCopy.stateName + "(Copy)";

            newState.Init(stateIndex, new Vector2(xPos, yPos), stateName, BitFSMRenderer.zoom);
            InitializeNewState(newState);

            //Clear transitions and copy new ones
            if (newState.transitions != null && settings.currentAI.stateToCopy.transitions != null)
            {
                newState.transitions.Clear();
                for (int i = 0; i < settings.currentAI.stateToCopy.transitions.Count; i++)
                {
                    Transition transition = Instantiate(settings.currentAI.stateToCopy.transitions[i]);

                    AssetDatabase.AddObjectToAsset(transition, settings.currentAI);

                    newState.transitions.Add(transition);
                }
            }
            EditorUtility.SetDirty(newState);

            settings.currentAI.states.Add(newState);

            AssetDatabase.AddObjectToAsset(newState, settings.currentAI);

            EditorUtility.SetDirty(settings.currentAI);
        }

        private void InitializeNewState(AIState newState)
        {
            newState.hideFlags = HideFlags.HideInInspector;
            newState.hasOutConnector = false;
            newState.OnClickInConnector = BitFSMConnectionHandler.OnClickInConnector;
            newState.OnRightClickInConnector = BitFSMConnectionHandler.OnRightClickInConnector;
            newState.OnClickRemoveState = BitFSMConnectionHandler.OnRemoveState;
            newState.OnClickTransitionSuccess = BitFSMConnectionHandler.OnClickTransitionSuccessConnector;
            newState.OnClickTransitionFailure = BitFSMConnectionHandler.OnClickTransitionFailureConnector;
            newState.OnClickCopyState = BitFSMConnectionHandler.OnCopyState;
        }

        //RENDERING
        private void OnGUI()
        {
            if (settings == null)
            {
                settings = BitFSMSettings.Instance;
            }
            settings.SetupNodeStyles();
            
            //CODE TO CONTROL HIGHLIGHTING THE CURRENT STATE
            if (Selection.activeObject != null && Selection.activeObject is GameObject)
            {
                GameObject currentSelectedGO = Selection.activeObject as GameObject;

                controller = currentSelectedGO.GetComponent<AIStateController>();

                if (controller != null)
                {
                    if (controller.liveUpdate)
                    {
                        if (currentSelectedGO != prevSelectedGO)
                        {
                            ClearStateLiveData();
                        }

                        if (controller.ai == settings.currentAI && controller.currentState != null)
                        {
                            controller.currentState.stateActive = true;
                            //GUI.changed = true;
                        }
                    }
                }
                prevSelectedGO = currentSelectedGO;
            } else
            {
                controller = null;
            }
            //}

            BitFSMEventHandler.HandleEvents(Event.current);
            BitFSMRenderer.Render(position.width, position.height);
        }

        private void Update()
        {
            if (controller != null && controller.liveUpdate)
            {
                if (EditorApplication.isPlaying && !EditorApplication.isPaused)
                {
                    if (Time.time - timeSinceLastUpdate > updateRate)
                    {
                        Repaint();
                        timeSinceLastUpdate = Time.time;
                    }
                }
            }
        }
    }
}
