using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallisticSeek : MonoBehaviour, ISeek
{
    [SerializeField] private float gravity;
    [SerializeField] private Vector3 startPosition, endPosition, velocity;
    [SerializeField] private float lateralSpeed, maxArcHeight = 20, lifetime = 20;
    [SerializeField] private int penetrationCount = 0;
    [SerializeField] private bool showDebug = false;

    private List<RaycastHit> targets = new List<RaycastHit>();
    private Coroutine seeking;

    public Transform Transform
    {
        get { return transform; }
    }

    [SerializeField] private LayerMask _mask;
    public LayerMask Mask
    {
        get { return _mask; }

        set { _mask = value; }
    }

    public RaycastHit GetTarget
    {
        get { return targets.Last(); }
    }

    public RaycastHit[] GetTargets
    {
        get { return targets.ToArray(); }
    }

    private Vector3 _lastPosition;
    public Vector3 LastPosition
    {
        get { return _lastPosition; }
    }

    public void Aim(Vector3 target)
    {
        Vector3 vel;
        float grav;

        if (Parabolic.solve_ballistic_arc_lateral(
            startPosition,
            lateralSpeed,
            target,
            Mathf.Max(startPosition.y + maxArcHeight, target.y - (startPosition.y + maxArcHeight)),
            out vel,
            out grav))
        {
            velocity = vel;
            gravity = grav;
        }
        else
        {
            throw new Exception("Cannot solve target parabola");
        }

        endPosition = target;
    }

    public IEnumerable<YieldInstruction> Seek(Vector3 start, Vector3 target, Action<RaycastHit> onFound)
    {
        RaycastHit hit;

        startPosition = start;
        endPosition = target;
        var startTime = Time.time;
        Aim(endPosition);

        //Set initial Projectile position and state
        _lastPosition = Parabolic.PositionAtTime(
            startPosition,
            velocity,
            -gravity,
            0);

        int penCount = penetrationCount;

#if UNITY_EDITOR_WIN
        var count = 0;
#endif

        while (Time.time - startTime <= lifetime)
        {
            yield return new WaitForEndOfFrame();

            var newPos = Parabolic.PositionAtTime(
                startPosition,
                velocity,
                -gravity,
                Time.time - startTime);

            Transform.LookAt(newPos);

#if UNITY_EDITOR_WIN
            if (showDebug && count++ % 2 == 0)
                Debug.DrawLine(_lastPosition, newPos, Color.white, 30);
#endif

            if (Physics.Linecast(_lastPosition, newPos, out hit, Mask))
            {
                targets.Add(hit);
                onFound?.Invoke(hit);

                if (penCount <= 0)
                    break;
                penCount--;
            }

            _lastPosition = newPos;
        }

        targets.Clear();
        seeking = null;
    }

    public void Seek(Vector3 start, Vector3 target)
    {
        if (seeking != null)
            return;

        seeking = UnityUtils.StartCoroutine(this, Seek(start, target, null));
    }
}