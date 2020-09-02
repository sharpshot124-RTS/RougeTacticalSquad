using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAction : GenericAction<MechController>
{
    public override void Act(MechController controller)
    {
        if(controller.PatrolArea == null)
            return;

        controller.Unit.MoveTo(controller.PatrolArea.GetPoint());
    }
}
