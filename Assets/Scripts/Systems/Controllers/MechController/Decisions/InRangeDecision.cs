using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeDecision : GenericDecision<MechController>
{
    public override bool Decide(MechController controller)
    {
        var target = controller.Seeker.GetTarget;

        return Vector3.Distance(target.point, controller.transform.position) <= controller.Weapon.Range;
    }
}
