using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BitFSM
{
    public class BitFSMConnectionHandler
    {
        public static int selectedInStateIndex = -1;
        public static int selectedOutStateIndex = -1;
        public static int selectedTransitionSuccessIndex = -1;
        public static int selectedTransitionFailureIndex = -1;

        public static void DrawConnections(float zoomXOffset, float zoomYOffset)
        {
            BitFSMSettings settings = BitFSMSettings.Instance;
            if (settings.currentAI.states != null)
            {
                int stateCount = settings.currentAI.states.Count;

                for (int i = 0; i < stateCount; i++)
                {
                    settings.currentAI.states[i].DrawConnections(zoomXOffset, zoomYOffset);
                }
            }

            if (selectedInStateIndex > -1)
            {
                Vector2 inCenter = settings.currentAI.states[selectedInStateIndex].GetInConnectorCenter(zoomXOffset, zoomYOffset);
                Vector2 outCenter = BitFSMEventHandler.mousePosition / BitFSMRenderer.zoom;

                UIState.DrawConnection(outCenter, inCenter);
                GUI.changed = true;
            } else if (selectedOutStateIndex > -1)
            {
                if (selectedTransitionFailureIndex > -1 || selectedTransitionSuccessIndex > -1)
                {
                    if (settings.currentAI.states[selectedOutStateIndex].showTransitions)
                    {
                        Vector2 inCenter = BitFSMEventHandler.mousePosition / BitFSMRenderer.zoom;
                        Vector2 outCenter = Vector2.zero;

                        if (selectedTransitionSuccessIndex > -1)
                        {
                            outCenter = settings.currentAI.states[selectedOutStateIndex].GetTransitionCenter(selectedTransitionSuccessIndex, zoomXOffset, zoomYOffset, true);
                            UIState.DrawTransitionConnection(outCenter, inCenter, true, Color.white);
                        }
                        else
                        {
                            outCenter = settings.currentAI.states[selectedOutStateIndex].GetTransitionCenter(selectedTransitionFailureIndex, zoomXOffset, zoomYOffset, false);
                            UIState.DrawTransitionConnection(outCenter, inCenter, false, Color.white);
                        }

                        
                        GUI.changed = true;
                    }
                }
                else
                {
                    Vector2 inCenter = BitFSMEventHandler.mousePosition / BitFSMRenderer.zoom;
                    Vector2 outCenter = settings.currentAI.states[selectedOutStateIndex].GetOutConnectorCenter(zoomXOffset, zoomYOffset);

                    UIState.DrawConnection(outCenter, inCenter);
                    GUI.changed = true;
                }
            }
        }

        public static void ClearConnectionMarkers()
        {
            if (selectedInStateIndex > -1 || selectedOutStateIndex > -1 || selectedTransitionSuccessIndex > -1 || selectedTransitionFailureIndex > -1)
            {
                selectedInStateIndex = -1;
                selectedOutStateIndex = -1;
                selectedTransitionSuccessIndex = -1;
                selectedTransitionFailureIndex = -1;
                GUI.changed = true;
            }
        }

        #region CONNECTOR CLICK ACTIONS
        public static void OnClickInConnector(int inStateIndex)
        {
            selectedInStateIndex = inStateIndex;

            if (selectedOutStateIndex > -1)
            {
                CreateConnection();
                ClearConnectionMarkers();
            }
        }

        public static void OnClickOutConnector(int outStateIndex)
        {
            selectedOutStateIndex = outStateIndex;

            if (selectedInStateIndex > -1)
            {
                CreateConnection();
                ClearConnectionMarkers();
            }
        }

        public static void OnClickTransitionSuccessConnector(int stateIndex, int transitionIndex)
        {
            selectedOutStateIndex = stateIndex;
            selectedTransitionSuccessIndex = transitionIndex;

            //Debug.Log("Transition Success Clicked - State:"+stateIndex+"; Transition:" + transitionIndex);

            if (selectedInStateIndex > -1)
            {
                CreateConnection();
                ClearConnectionMarkers();
            }
        }

        public static void OnClickTransitionFailureConnector(int stateIndex, int transitionIndex)
        {
            selectedOutStateIndex = stateIndex;
            selectedTransitionFailureIndex = transitionIndex;

            //Debug.Log("Transition Failure Clicked - State:" + stateIndex + "; Transition:" + transitionIndex);

            if (selectedInStateIndex > -1)
            {
                CreateConnection();
                ClearConnectionMarkers();
            }
        }

        public static void OnRightClickInConnector(int inStateIndex)
        {
            for (int i=0; i < BitFSMSettings.Instance.currentAI.states.Count; i++)
            {
                if (i != inStateIndex)
                {
                    BitFSMSettings.Instance.currentAI.states[i].RemoveStateFromChild(BitFSMSettings.Instance.currentAI.states[inStateIndex]);
                }
            }
        }

        public static void OnRemoveState(int stateIndex)
        {
            //Disconnect the state from any transition to it
            OnRightClickInConnector(stateIndex);

            AIState stateToRemove = BitFSMSettings.Instance.currentAI.states[stateIndex];

            //Remove the state from the states list
            BitFSMSettings.Instance.currentAI.states.RemoveAt(stateIndex);           


            //reindex the remaining states
            for (int i=0; i < BitFSMSettings.Instance.currentAI.states.Count; i++)
            {
                BitFSMSettings.Instance.currentAI.states[i].index = i;
            }

            //if the state has any actions, or transitions, and decisions, destroy them all
            if (stateToRemove.transitions != null)
            {
                for (int i=stateToRemove.transitions.Count - 1; i > -1; i--)
                {
                    Object.DestroyImmediate(stateToRemove.transitions[i],true);
                }
            }

            //No finally destroy the state itself
            Object.DestroyImmediate(stateToRemove, true);
            
            EditorUtility.SetDirty(BitFSMSettings.Instance.currentAI);
        }

        public static void OnCopyState(int stateIndex)
        {
            BitFSM currentAI = BitFSMSettings.Instance.currentAI;
            currentAI.stateToCopy = currentAI.states[stateIndex];
        }

        public static void CreateConnection()
        {
            if (selectedTransitionFailureIndex > -1 || selectedTransitionSuccessIndex > -1)
            {
                if (selectedTransitionSuccessIndex > -1)
                {
                    AIState selectedOutState = BitFSMSettings.Instance.currentAI.states[selectedOutStateIndex];
                    AIState selectedInState = BitFSMSettings.Instance.currentAI.states[selectedInStateIndex];

                    if (!selectedOutState.transitions[selectedTransitionSuccessIndex].isTransitionTreeConnector && !(selectedInState is TransitionTreeState))
                    {
                        selectedOutState.CreateTransitionConnection(selectedInState, selectedTransitionSuccessIndex, true);
                    } else if (selectedOutState.transitions[selectedTransitionSuccessIndex].isTransitionTreeConnector && (selectedInState is TransitionTreeState) && !(selectedOutState is TransitionTreeState))
                    {
                        selectedOutState.CreateTransitionConnection(selectedInState, selectedTransitionSuccessIndex, true);
                    }
                } else
                {
                    BitFSMSettings.Instance.currentAI.states[selectedOutStateIndex].CreateTransitionConnection(BitFSMSettings.Instance.currentAI.states[selectedInStateIndex], selectedTransitionFailureIndex, false);
                }
                //Debug.Log("Got here");
            }
            else
            {
                BitFSMSettings.Instance.currentAI.states[selectedOutStateIndex].CreateConnection(BitFSMSettings.Instance.currentAI.states[selectedInStateIndex]);
            }
        }
        #endregion
    }
}
