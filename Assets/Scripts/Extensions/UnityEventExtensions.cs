using System;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventExtensions
{

}

[Serializable]
public class FloatUnityEvent : UnityEvent<float> { }

[Serializable]
public class Vector3UnityEvent : UnityEvent<Vector3> { }

[Serializable]
public class RaycastUnityEvent : UnityEvent<RaycastHit>
{
    public void Invoke(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            Invoke(hit);
        }
    }

    public void InvokeAtCursorPoint(LayerMask mask)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        Physics.Linecast(ray.origin,
            ray.origin + (ray.direction * 10000), out hit, mask);

        if(hit.transform)
            Invoke(hit);
        else
        {
            Debug.LogWarning("No target!");
        }
    }
}

[Serializable]
public class MultiRaycastUnityEvent : UnityEvent<RaycastHit[]> { }

[Serializable]
public class RectUnityEvent : UnityEvent<Rect> { }