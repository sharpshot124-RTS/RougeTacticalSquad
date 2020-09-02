using System;
using System.Collections;
using System.Collections.Generic;
using BitFSM;
using UnityEngine;

public class FireAction : GenericAction<MechController>
{
    public override void Act(MechController controller)
    {
        if (!controller.Seeker.GetTarget.transform)
            return;

        controller.Weapon.Fire(controller.Seeker.GetTarget.point);
    }
}
