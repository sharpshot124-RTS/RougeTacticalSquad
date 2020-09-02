using System;
using System.Collections;
using System.Collections.Generic;
using BitFSM;
using UnityEngine;

public class StopAction : GenericAction<MechController>
{
    public override void Act(MechController controller)
    {
        controller.Unit.Move(Vector3.zero);
    }
}
