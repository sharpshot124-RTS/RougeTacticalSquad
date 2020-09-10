using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class RaycastHitUnityEvent : UnityEvent<RaycastHit>
{
    public void InvokeMany(RaycastHit[] hits)
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

        if (hit.transform)
            Invoke(hit);
        else
        {
            Debug.LogWarning("No target!");
        }
    }
}
