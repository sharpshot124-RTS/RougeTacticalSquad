using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusFactory : ScriptableObject
{
    public abstract IStatus GetStatus(RaycastHit target);
}

public class StatusFactory<DataT, StatusT> : StatusFactory
    where DataT : class
    where StatusT : IStatus<DataT>, new()
{
    public DataT data;

    public override IStatus GetStatus(RaycastHit target)
    {
        return new StatusT { Data = this.data, Target = target };
    }
}
