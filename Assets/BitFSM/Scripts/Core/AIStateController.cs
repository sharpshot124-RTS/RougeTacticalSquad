using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BitFSM
{
    public class AIStateController : MonoBehaviour
    {
        [Header("AI Config")]
        public BitFSM ai;
        public bool aiEnabled = false;

        [HideInInspector] public AIState currentState = null;
        #if UNITY_EDITOR
        //Variables for live data
        [HideInInspector] public bool stateControllerActiveInEditor = false;
        public bool liveUpdate = false;
        public bool currentStateSceneColor = false;
        public float stateSphereRadius = 0.5f;
        #endif

        protected virtual void Update()
        {
            if (!aiEnabled || ai == null || ai.states == null || ai.states.Count == 0)
            {
                return;
            }

            if (currentState == null)
            {
                //Set the entry state as the ai's first state
                currentState = ai.states[0];
            }

#if UNITY_EDITOR
            if (liveUpdate && Selection.activeObject != null && Selection.activeObject is GameObject)
            {
                if ((Selection.activeObject as GameObject) == this.gameObject)
                {
                    this.stateControllerActiveInEditor = true;
                }
            }
#endif

            currentState.OnUpdate(this);
        }

        public void TransitionToState(AIState transitionToState)
        {
            if (transitionToState != null && transitionToState != currentState)
            {
                currentState.OnExit(this);
                currentState = transitionToState;
                currentState.OnEnter(this);
            }
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            if (currentState != null && currentStateSceneColor)
            {
                Color _currentStateColor = currentState.stateColor;
                _currentStateColor.a = 1;
                Gizmos.color = _currentStateColor;
                Gizmos.DrawWireSphere(transform.position, stateSphereRadius);
                Gizmos.color = Color.white;
            }
        }
#endif
    }
}
