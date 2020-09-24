using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DOTStatusData
{
    public float dmgPerTick = 4f;
    public float duration = 4f;
    public float tickRate = 1f;

}

public class DOTStatus : IStatus<DOTStatusData>
{
    private DOTStatusData _data;
    public DOTStatusData Data
    {
        get { return _data; }

        set { _data = value; }
    }

    private RaycastHit _target;
    public RaycastHit Target
    {
        get { return _target; }

        set { _target = Target; }
    }

    public void Apply()
    {
        Target.transform.GetComponent<IUnit>().StartCoroutine(DOT(Target));
    }

    public IEnumerator DOT(RaycastHit target)
    {
        var unit = target.transform.GetComponent<IUnit>();
        var durationLeft = Data.duration;
        while (durationLeft > 0)
        {
            unit.Health.ChangeValue(-Data.dmgPerTick);
            yield return new WaitForSeconds(Data.tickRate);
            durationLeft -= Data.tickRate;
        }
    }
}