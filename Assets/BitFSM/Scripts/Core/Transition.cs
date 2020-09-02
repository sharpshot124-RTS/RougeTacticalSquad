using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitFSM
{
    public class Transition : ScriptableObject
    {
        public Decision decision;
        public AIState successState = null;
        public AIState failureState = null;
        public bool isTransitionTreeConnector = false;

#if UNITY_EDITOR
        [HideInInspector] public Rect successRect;
        [HideInInspector] public Rect failureRect;
#endif

        public bool Evaluate(AIStateController controller)
        {
            if (!isTransitionTreeConnector)
            {
                if (decision != null)
                {
                    if (decision.Decide(controller))
                    {
                        controller.TransitionToState(successState);
                        return true;
                    }
                    else
                    {
                        controller.TransitionToState(failureState);
                    }
                }
            } else
            {
                if (successState != null)
                {
                    return successState.RunTransitions(controller);
                }   
            }
            return false;
        }
    }
}
