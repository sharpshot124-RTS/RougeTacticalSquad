using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityExecutor : MonoBehaviour
{
    void Invoke(UnityEvent called)
    {
        if(enabled)
            called.Invoke();
    }

    void Invoke<T>(UnityEvent<T> called, T arg)
    {
        if(enabled)
            called.Invoke(arg);
    }

    [SerializeField] private UnityEvent OnAwake;
    void Awake()
    {
        Invoke(OnAwake);
    }

    [SerializeField] private UnityEvent OnStart;
    void Start()
    {
        Invoke(OnStart);
    }

    [SerializeField] private UnityEvent OnUpdate;
    void Update()
    {
        Invoke(OnUpdate);
    }

    [SerializeField] private UnityEvent OnLateUpdate;
    void LateUpdate()
    {
        Invoke(OnLateUpdate);
    }

    [SerializeField] private UnityEvent OnFixedUpdate;
    void FixedUpdate()
    {
        Invoke(OnFixedUpdate);
    }

    [SerializeField] private UnityEvent OnCollisionEnterEvent;
    void OnCollisionEnter(Collision c)
    {
        Invoke(OnCollisionEnterEvent);
    }

    [SerializeField] private UnityEvent OnCollisionExitEvent;
    void OnCollisionExit(Collision c)
    {
        Invoke(OnCollisionExitEvent);
    }

    [SerializeField] private UnityEvent OnCollisionStayEvent;
    void OnCollisionStay(Collision c)
    {
        Invoke(OnCollisionStayEvent);
    }

    [SerializeField] private UnityEvent OnTriggerEnterEvent;
    void OnTriggerEnter(Collider c)
    {
        Invoke(OnTriggerEnterEvent);
    }

    [SerializeField] private UnityEvent OnTriggerExitEvent;
    void OnTriggerExit(Collider c)
    {
        Invoke(OnTriggerExitEvent);
    }

    [SerializeField] private UnityEvent OnTriggerStayEvent;
    void OnCollisionStay(Collider c)
    {
        Invoke(OnTriggerStayEvent);
    }

    [SerializeField] private UnityEvent OnEnableEvent;
    void OnEnable()
    {
        Invoke(OnEnableEvent);
    }

    [SerializeField] private UnityEvent OnDisableEvent;
    void OnDisable()
    {
        Invoke(OnDisableEvent);
    }

    [SerializeField] private UnityEvent OnReset;
    void Reset()
    {
        Invoke(OnReset);
    }

    [SerializeField] private RaycastUnityEvent onParticleCollision;
    [SerializeField] private ParticleSystem particle;
    List<ParticleCollisionEvent> particleEvents = new List<ParticleCollisionEvent>();

     void OnParticleCollision(GameObject target)
    {
        if(!particle)
            return;

        int count = particle.GetCollisionEvents(target, particleEvents);
        foreach (var c in particleEvents)
        {
            RaycastHit hit;

            if (Physics.Linecast(transform.position, c.intersection, out hit))
            {
                onParticleCollision.Invoke(hit);
            }
        }
    }
}
