using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    Vector3 destination, currenVel;

    [SerializeField] private float maxDist = .5f;
    [SerializeField] private float maxAngle = 45;
    [SerializeField] float smoothness = .1f;
     
    [SerializeField] LayerMask mask;

    // Update is called once per frame
    void Update()
    {
        var targetDistance = new Vector3(target.position.x, transform.position.y, target.position.z);
        if(Vector3.Distance(transform.position, target.position) > maxDist || Vector3.Angle(transform.forward, target.forward) >= maxAngle)
        {
            var dir = (target.position - transform.position).normalized;

            destination = target.position + target.forward * maxDist * .9f;
            destination.y = transform.position.y;   

            RaycastHit hit;
            if (Physics.Raycast(destination + Vector3.up * maxDist, Vector3.down, out hit, maxDist * 2, mask))
            {
                transform.rotation = Quaternion.LookRotation(target.forward, hit.normal);
                destination = hit.point;
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, destination, ref currenVel, smoothness);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(target.position, maxDist);
    }
}
