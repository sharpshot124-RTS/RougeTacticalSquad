 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform target;

    public bool position = true, rotation = true;
    public Vector3 positionOffset;

    void LateUpdate()
    {
        if(position)
            transform.position = target.position + positionOffset;

        if (rotation)
            transform.rotation = target.rotation;
    }
}
