using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Status/KnockedBack")]

public class KockBackStatusFactory : StatusFactory<KockBackStatusData, KnockBackStatus> { }

[Serializable]
public class KockBackStatusData
{
    public float blastRadius = 5;
    public float blastSpeed = .5f;
}

public class KnockBackStatus : IStatus<KockBackStatusData>
{
    private KockBackStatusData _data;

    public KockBackStatusData Data
    {
        get { return _data; }
        set { _data = value; }
    }

    private IUnit _target;
    public IUnit Target
    {
        get { return _target; }

        set { _target = value; }
    }

    public void Apply()
    {
        Target.StartCoroutine(KnockBack(Target));
    }

    public IEnumerator KnockBack(IUnit target)
    {
        var start = target.Transform.position;
        var destination = start;
        yield return null;
    }

}