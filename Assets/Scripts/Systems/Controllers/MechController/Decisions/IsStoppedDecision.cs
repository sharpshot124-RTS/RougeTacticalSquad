using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsStoppedDecision : GenericDecision<MechController>
{
    public override bool Decide(MechController controller)
    {
        return controller.Unit.Stopped;
    }
}
