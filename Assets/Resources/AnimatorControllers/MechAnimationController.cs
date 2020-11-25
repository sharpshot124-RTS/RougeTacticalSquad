using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class MechAnimationController : MonoBehaviour
{

    [SerializeField] private NavMeshAgent nav;
    [SerializeField] private string forwardParam, turningParam, fireParam;

    [SerializeField] private Animator _anim;
    public Animator Anim
    {
        get
        {
            if (!_anim)
                _anim = GetComponent<Animator>();

            return _anim;
        }
    }

    private float lastAngle;
    
    public void Update()
    {
        var delta = GetDeltaPosition();
        delta.x = Mathf.Max(delta.x, DeltaTurning());

        Anim.SetFloat(forwardParam, delta.y);
        Anim.SetFloat(turningParam, delta.x);
    }

    public void TriggerFire()
    {
        Anim.SetTrigger(fireParam);
    }

    Vector2 GetDeltaPosition()
    {
        Vector3 velocity = nav.velocity.normalized;

        velocity = transform.InverseTransformDirection(velocity);
        velocity = velocity * (nav.velocity.magnitude / nav.speed);

        return new Vector2(velocity.x, velocity.z);
    }

    float DeltaTurning()
    {
        float deltaAngle = transform.rotation.y - lastAngle;

        lastAngle = transform.rotation.y;

        return deltaAngle * nav.angularSpeed;
    }
}
