using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasTargetDecision : GenericDecision<MechController>
{
    public override bool Decide(MechController controller)
    {
        controller.Seeker.Seek(controller.transform.position);
        return controller.Seeker.GetTarget.transform;
    }
}
