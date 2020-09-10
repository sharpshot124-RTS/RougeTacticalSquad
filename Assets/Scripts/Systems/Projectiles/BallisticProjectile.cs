using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Custom/Projectiles/Ballistic")]
public class BallisticProjectile : ScriptableObject, IProjectile
{
    public GameObject prefab;

    private Vector3 _startPosition;
    public Vector3 StartPosition
    {
        get { return _startPosition; }

        set { _startPosition = value; }
    }

    private Vector3 _endPosition;
    public Vector3 EndPosition
    {
        get { return _endPosition; }

        set { _endPosition = value; }
    }

    private Coroutine _firing = null;
    public Coroutine Firing
    {
        get { return _firing; }

        set { _firing = value; }
    }

    public Transform Transform
    {
        get { return Targets.Transform; }
    }

    private RaycastHitUnityEvent _onHit = new RaycastHitUnityEvent();
    public RaycastHitUnityEvent OnHit
    {
        get { return _onHit; }
    }

    public LayerMask Mask
    {
        get { return Targets.Mask; }
        set { Targets.Mask = value; }
    }

    private ISeek _targets;

    public ISeek Targets
    {
        get { return _targets; }

        set { _targets = value; }
    }

    public bool IsFiring
    {
        get { return Firing == null; }
    }

    public Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return Transform.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
    }

    public void Move(Vector3 direction)
    {
        Transform.position += direction;
    }

    public void MoveTo(Vector3 location)
    {
        Transform.position = location;
    }

    public void Reset()
    {
        if (Firing != null)
        {
            Transform.GetComponent<MonoBehaviour>().StopCoroutine(Firing);
            Firing = null;
        }

        Transform.SendMessage("Reset");
    }

    public IProjectile Instantiate()
    {
        var projectile = CreateInstance<BallisticProjectile>();
        projectile.Targets = Instantiate(prefab).GetComponent<ISeek>();
        return projectile;
    }

    public void Fire(Vector3 target)
    {
        if (Firing == null)
        {
            Transform.position = StartPosition;
            Reset();
            EndPosition = target;
            Firing = 
                UnityUtils.StartCoroutine(
                    Targets as MonoBehaviour, 
                    Targets.Seek(EndPosition,
                    (hit) => OnHit.Invoke(hit)),
                    //OnStep    
                    () => Transform.position = Targets.LastPosition,
                    //OnComplete
                    () => Firing = null);

        }
    }
}
