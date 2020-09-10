using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float stepDuration;
    Vector3 destination, currenVel, lastPos;

    [SerializeField] private float maxDist = .5f;
    [SerializeField] private float maxAngle = 45;
    [SerializeField] float smoothness = .1f;
     
    [SerializeField] LayerMask mask;

    void Update()
    {
        var dir = destination - target.position;
        var angleDiff = Vector3.Angle(transform.forward, target.forward);
        
        var velDir = (lastPos - target.position).normalized;

        if (dir.magnitude > maxDist || angleDiff >= maxAngle)
        {
            
            dir = Vector3.RotateTowards(dir, velDir, maxAngle, 0);          

            Ray ray = new Ray(target.position - dir + Vector3.up * maxDist, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDist * 2, mask))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);

                transform.rotation = Quaternion.LookRotation(target.forward, hit.normal);
                destination = hit.point;

                StartCoroutine(BeginStep());
            }
        }

        lastPos = target.position;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref currenVel, smoothness);
    }

    IEnumerator BeginStep()
    {
        var start = Time.time;
        var offset = destination - target.position;

        while(Time.time < start + stepDuration)
        {
            destination = target.position + offset;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(target.position, maxDist);
    }
}
