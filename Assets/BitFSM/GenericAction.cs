using System;
using System.Collections;
using System.Collections.Generic;
using BitFSM;
using UnityEngine;
using Action = BitFSM.Action;

public abstract class GenericAction<T> : Action where T : AIStateController
{
    public override void Act(AIStateController controller)
    {
        try
        {
            Act((T)controller);
        }
        catch(Exception e)
        {
            Debug.LogWarning(e.Message + e.StackTrace);
        }
    }

    public abstract void Act(T controller);
}
