using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Status/DOT")]
public class DOTStatusFactory : StatusFactory<DOTStatusData, DOTStatus> { }

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

    private IUnit _target;
    public IUnit Target
    {
        get { return _target; }

        set { _target = Target; }
    }

    public void Apply()
    {
        Target.StartCoroutine(DOT(Target));
    }

    public IEnumerator DOT(IUnit target)
    {
        var durationLeft = Data.duration;
        while (durationLeft > 0)
        {
            target.ChangeValue(-Data.dmgPerTick);
            yield return new WaitForSeconds(Data.tickRate);
            durationLeft -= Data.tickRate;
        }
    }
}