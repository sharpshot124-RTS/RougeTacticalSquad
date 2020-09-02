using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasTargetDecision : GenericDecision<MechController>
{
    public override bool Decide(MechController controller)
    {
        return controller.Seeker.GetTarget.transform;
    }
}
