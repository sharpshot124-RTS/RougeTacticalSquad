using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitFSM
{
    [CreateAssetMenu(menuName = "BitFSM/AI")]
    public class BitFSM : ScriptableObject
    {
#if UNITY_EDITOR
        [HideInInspector] public float zoom = 1f;
        [HideInInspector] public Vector2 zoomCoords = Vector2.zero;
        [HideInInspector] public AIState stateToCopy = null;
#endif 
        public List<AIState> states;
    }
}
