using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class SlowedStatusData
{
    public float slowFactor = .8f;
    public float duration = 2f;
}

public class SlowedStatus : IStatus<SlowedStatusData>
{
    private SlowedStatusData _data;
    public SlowedStatusData Data
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
        Target.transform.GetComponent<IUnit>().StartCoroutine(Slow(Target));
    }

    public IEnumerator Slow(RaycastHit target)
    {
        var unit = target.transform.GetComponent<IUnit>();
        unit.Speed *= Data.slowFactor;

        yield return new WaitForSeconds(Data.duration);

        unit.Speed /= Data.slowFactor;
    }
}