using System;
using System.Collections;
using System.Collections.Generic;
using BitFSM;
using UnityEngine;

public class SeekTargetAction : GenericAction<MechController>
{
    public override void Act(MechController controller)
    {
        controller.Seeker.Seek(controller.transform.position, Vector3.zero);
    }
}
