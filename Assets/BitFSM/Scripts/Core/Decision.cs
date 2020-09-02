using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitFSM
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(AIStateController controller);
    }
}
