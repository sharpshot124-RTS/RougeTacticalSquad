using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoundsArea : MonoBehaviour, IArea
{
    [SerializeField] private Color gizmoColor;
    [SerializeField] private bool isWorldSpace;
    [SerializeField] private Bounds bounds;

    [SerializeField] private string _id;
    public string ID
    {
        get { return _id; }
    }

    public Vector3 Center
    {
        get { return isWorldSpace ? bounds.center : transform.position + bounds.center; }
        set
        {
            if (isWorldSpace)
            {
                bounds.center = value;
            }
            else
            {
                transform.position = value;
            }
        }
    }

    public Vector3 Size
    {
        get { return bounds.size; }
    }

    public Transform Transform
    {
        get { return transform; }
    }

    public GameObject GameObject
    {
        get { return gameObject; }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(Center, bounds.size);
    }

    public Vector3 GetPositionInArea(Vector3 targetPoint)
    {
        return bounds.ClosestPoint(targetPoint - Center) + Center;
    }

    public Vector3 GetPoint()
    {
        Vector3 point = Random.insideUnitSphere;
        point.Scale(bounds.extents);

        return Center + point;
    }

    public void Move(Vector3 direction)
    {
        Center += direction;
    }

    public void MoveTo(Vector3 location)
    {
        Center = location;
    }

    public void MoveTo(RaycastHit hit)
    {
        MoveTo(hit.point);
    }
}
