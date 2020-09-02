using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    [SerializeField]
    private StatusFactory[] statuses;


    public void ApplyStatuses(IUnit target)
    {
        foreach (var status in statuses)
        {
            var applied = status.GetStatus(target);
            applied.Apply();
        }
    }

    public void ApplyStatuses(RaycastHit hit)
    {
        var unit = hit.transform.GetComponent<IUnit>();

        if (unit != null)
        {
            ApplyStatuses(unit);
        }
    }

    public void ApplyStatuses(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            ApplyStatuses(hit);
        }
    }
}
