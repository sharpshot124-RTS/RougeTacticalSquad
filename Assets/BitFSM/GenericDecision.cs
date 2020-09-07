using System;
using System.Collections;
using System.Collections.Generic;
using BitFSM;
using UnityEngine;

public abstract class GenericDecision<T> : Decision where T : AIStateController
{
    public override bool Decide(AIStateController controller)
    {
        try
        {
            return Decide(controller as T);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Controller Type Mismatch: " + controller.GetType());
            return false;
        }
    }

    public abstract bool Decide(T controller);
}
