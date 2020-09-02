using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitFSM
{
    public class AIState : AIUIState
    {
        public virtual void OnEnter(AIStateController controller)
        {
            if (entryActions != null)
            {
                for (int i = 0; i < entryActions.Count; i++)
                {
                    if (entryActions[i] != null)
                    {
                        entryActions[i].Act(controller);
                    }
                }
            }
        }

        public virtual void OnUpdate(AIStateController controller)
        {
            if (updateActions != null)
            {
                for (int i = 0; i < updateActions.Count; i++)
                {
                    if (updateActions[i] != null)
                    {
                        updateActions[i].Act(controller);
                    }
                }
            }

            RunTransitions(controller);

#if UNITY_EDITOR
            //FOR LIVE DATA
            controller.stateControllerActiveInEditor = false;
#endif
        }

        public virtual bool RunTransitions(AIStateController controller)
        {
            if (transitions != null)
            {
                for (int i = 0; i < transitions.Count; i++)
                {
                    bool transitionResult = transitions[i].Evaluate(controller);

                    if (!transitions[i].isTransitionTreeConnector)
                    {
                        if (transitionResult && transitions[i].successState != null)
                        {
#if UNITY_EDITOR
                            if (controller.stateControllerActiveInEditor)
                            {
                                activeTransitionIndex = i;
                                activeTransitionType = 0;
                                activeTransitionDisplayCount = maxTransitionDisplayCount;
                            }
#endif
                            return true;
                        }
                        else if (transitionResult == false && transitions[i].failureState != null)
                        {
#if UNITY_EDITOR
                            if (controller.stateControllerActiveInEditor)
                            {
                                activeTransitionIndex = i;
                                activeTransitionType = 1;
                                activeTransitionDisplayCount = maxTransitionDisplayCount;
                            }
#endif
                            return true;
                        }
                    } else
                    {
                        //If the transitiontree transition is successful then don't evaluate any of the other transitions
                        if (transitionResult)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public virtual void OnExit(AIStateController controller)
        {
            if (exitActions != null)
            {
                for (int i = 0; i < exitActions.Count; i++)
                {
                    if (exitActions[i] != null)
                    {
                        exitActions[i].Act(controller);
                    }
                }
            }
        }
    }
}
