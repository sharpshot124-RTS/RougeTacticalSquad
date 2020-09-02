using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusFactory : ScriptableObject
{
    public abstract IStatus GetStatus(IUnit target);
}

public class StatusFactory<DataT, StatusT> : StatusFactory
    where DataT : class
    where StatusT : IStatus<DataT>, new()
{
    public DataT data;

    public override IStatus GetStatus(IUnit target)
    {
        return new StatusT { Data = this.data, Target = target };
    }
}
