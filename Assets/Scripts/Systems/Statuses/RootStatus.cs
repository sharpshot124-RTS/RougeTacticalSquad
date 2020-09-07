using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class RootStatusData
{
    public float rootDuration = 6f;
}

public class RootStatus : IStatus<RootStatusData>
{
    private RootStatusData _data;
    public RootStatusData Data
    {
        get { return _data; }

        set { _data = value; }
    }

    private RaycastHit _target;
    public RaycastHit Target
    {
        get { return _target; }

        set { _target = value; }
    }

    public void Apply()
    {
        Target.transform.GetComponent<IUnit>().StartCoroutine(Root(Target));
    }

    public IEnumerator Root(RaycastHit target)
    {
        var unit = target.transform.GetComponent<IUnit>();
        var orignSpeed = unit.Speed;
        unit.Speed = 0;
        yield return new WaitForSeconds(Data.rootDuration);
        unit.Speed = orignSpeed;
    }
}