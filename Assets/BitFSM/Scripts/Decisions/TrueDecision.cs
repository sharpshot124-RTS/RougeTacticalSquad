using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitFSM
{
    public class TrueDecision : Decision
    {
        public override bool Decide(AIStateController controller)
        {
            return true;
        }
    }
}
