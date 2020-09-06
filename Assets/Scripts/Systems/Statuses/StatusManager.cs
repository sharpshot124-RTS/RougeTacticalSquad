using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    [SerializeField]
    private StatusFactory[] statuses;


    // public void ApplyStatuses(IUnit target)
    // {
    //     foreach (var status in statuses)
    //     {
    //         var applied = status.GetStatus(target);
    //         applied.Apply();
    //     }
    // }

    public void ApplyStatuses(RaycastHit target)
    {
        // var unit = hit.transform.GetComponent<IUnit>();

        foreach (var status in statuses)
        {
            var applied = status.GetStatus(target);
            applied.Apply();
        }
    }

    public void ApplyStatuses(RaycastHit[] targets)
    {
        foreach (var hit in targets)
        {
            ApplyStatuses(hit);
        }
    }
}
