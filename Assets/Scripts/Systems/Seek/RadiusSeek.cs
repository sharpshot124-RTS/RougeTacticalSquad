using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadiusSeek : MonoBehaviour, ISeek
{
    [SerializeField] private bool showGizmos;
    [SerializeField] private Color gizmoColor;
    [SerializeField] private float radius = 15;

    [SerializeField] private RaycastUnityEvent onTargetAquired;

    private List<RaycastHit> targets = new List<RaycastHit>();

    [SerializeField] private LayerMask _mask;
    public LayerMask Mask
    {
        get { return _mask; }

        set { _mask = value; }
    }

    public Transform Transform
    {
        get { return transform; }
    }

    public Vector3 LastPosition
    {
        get { return Transform.position; }
    }

    public IEnumerable<YieldInstruction> Seek(Vector3 target, Action<RaycastHit> onFound)
    {
        targets.Clear();

        var colliders = Physics.OverlapSphere(target, radius, _mask).Where(t => !t.transform.IsChildOf(transform));
        if (colliders.Count() > 0)
        {
            RaycastHit hit;

            foreach (var c in colliders)
            {
                if (Physics.Raycast(
                    new Ray(target, c.transform.position - target),
                    out hit, radius, _mask))
                {
                    targets.Add(hit);

                    if(onFound != null)
                        onFound.Invoke(hit);

                    yield return new WaitForEndOfFrame();
                }
            }
        }

        targets = new List<RaycastHit>(targets.OrderBy((t) => Vector3.Distance(target, t.point)));
    }

    public void OnDrawGizmos()
    {
        if(!showGizmos)
            return;

        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, radius);
    }

    public RaycastHit GetTarget
    {
        get 
        {
            try
            {
                return targets.Last();
            }
            catch
            {
                return new RaycastHit();
            }
        }
    }

    public RaycastHit[] GetTargets
    {
        get { return targets.ToArray(); }
    }

    public void Seek(Vector3 target)
    {
        //Seek(target, null);

        foreach(var test in Seek(target, (r) => onTargetAquired.Invoke(r)))
        {

        }
    }

    public void Seek()
    {
        Seek(transform.position);
    }
}