using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(menuName = "Custom/Projectiles/Emitter")]
public class EmitterProjectile : ScriptableObject, IProjectile
{
    public GameObject prefab;
    public float tickRate;

    private float lastTick;

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

    private RaycastUnityEvent _onHit = new RaycastUnityEvent();
    public RaycastUnityEvent OnHit
    {
        get { return _onHit; }
    }

    private List<RaycastHit> hits = new List<RaycastHit>();

    [SerializeField]
    private LayerMask _mask;
    public LayerMask Mask
    {
        get { return _mask; }
        set { _mask = value; }
    }

    [SerializeField] private Object _seeker;
    public ISeek Targets
    {
        get { return _seeker as ISeek; }

        set
        {
            _seeker = value as Object;
        }
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

        if(Transform)
            Transform.SendMessage("Reset");
    }

    public IProjectile Instantiate()
    {
        return new EmitterProjectile()
        {
            Targets = Instantiate(prefab).GetComponent<ISeek>(),
            Mask = this.Mask,
        };
    }

    public void Fire(Vector3 target)
    {
        if (Firing == null)
        {
            lastTick = Time.time;

            Firing = UnityUtils.StartCoroutine(Targets as MonoBehaviour,
                Targets.Seek(Transform.position, target, hit => OnHit.Invoke(hit)),
                //On Step
                () =>
                {
                    Transform.position = StartPosition;
                    Transform.LookAt(target);
                });
        }
    }
}