using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtGyro : MonoBehaviour, IGyro
{
    private Quaternion rotation;

    [SerializeField] private float speed = .1f;
    [SerializeField] private Quaternion offset = Quaternion.identity;
    [SerializeField] private Quaternion targetRotation;
    [SerializeField] private bool isLocalSpace;

    private Quaternion origOffset, origTarget;

    public void Awake()
    {
        origOffset = offset;
        origTarget = targetRotation;
    }

    public Quaternion TargetRotation
    {
        get { return targetRotation; }

        set { targetRotation = value; }
    }

    public void LookAt(Vector3 target)
    {
        TargetRotation = Quaternion.LookRotation(target - transform.position);
    }

    public void LookAt(RaycastHit hit)
    {
        LookAt(hit.point);
    }

    public void Reset()
    {
        offset = origOffset;
        targetRotation = origTarget;
    }

    public void LateUpdate()
    {
        rotation = Quaternion.Slerp(rotation, TargetRotation * offset, speed * Time.deltaTime);

        if (isLocalSpace)
        {
            transform.localRotation = rotation;
        }
        else
        {
            transform.rotation = rotation;
        }
    }
}
