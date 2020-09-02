using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManueverAction : GenericAction<MechController>
{
    public override void Act(MechController controller)
    {
        controller.Unit.MoveTo(controller.ManueverArea.GetPoint());
    }
}
