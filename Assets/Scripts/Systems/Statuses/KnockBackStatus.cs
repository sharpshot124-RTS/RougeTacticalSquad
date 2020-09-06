using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private RaycastHit _target;
    public RaycastHit Target
    {
        get { return _target; }

        set { _target = value; }
    }

    public void Apply()
    {
        Target.transform.GetComponent<IUnit>().StartCoroutine(KnockBack(Target));
    }

    public IEnumerator KnockBack(RaycastHit target)
    {
        var unit = target.transform.GetComponent<IUnit>();
        var startPoint = unit.Transform.position;
        var destination = startPoint - target.point;
        var distance = Data.blastRadius - destination.magnitude;
        destination = startPoint + destination.normalized * distance;

        float startTime = Time.time;
        while (Time.time - startTime < Data.blastSpeed)
        {
            unit.Transform.position = Vector3.Lerp(startPoint, destination, (Time.time - startTime) / Data.blastSpeed);
            yield return new WaitForEndOfFrame();
        }
    }

}