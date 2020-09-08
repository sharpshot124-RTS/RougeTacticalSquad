using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ParticleSeek : MonoBehaviour, ISeek
{
    List<GameObject> targets = new List<GameObject>();
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private UnityEvent onSeek;

    public RaycastHit GetTarget
    {
        get { throw new NotImplementedException(); }
    }

    public RaycastHit[] GetTargets
    {
        get { throw new NotImplementedException(); }
    }

    public Vector3 LastPosition
    {
        get { return transform.position; }
    }

    public LayerMask Mask
    {
        get { return particle.collision.collidesWith; }

        set
        {
            var collision = particle.collision;
            collision.collidesWith = value;
        }
    }

    public Transform Transform
    {
        get { return transform; }
    }

    public void Seek(Vector3 target)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<YieldInstruction> Seek(Vector3 target, Action<RaycastHit> onFound)
    {
        Mask = Mask;
        onSeek.Invoke();
        List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();

        foreach (var t in targets)
        {
            particle.GetCollisionEvents(t, events);
            foreach (var e in events)
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, e.intersection, out hit, Mask))
                {
                    onFound.Invoke(hit);
                }
            }
        }

        targets.Clear();

        yield return new WaitForEndOfFrame();
    }

    public void AddTarget(GameObject target)
    {
        if(!targets.Contains(target))
            targets.Add(target);
    }
}
