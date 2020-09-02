using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitFSM
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(AIStateController controller);
    }
}
