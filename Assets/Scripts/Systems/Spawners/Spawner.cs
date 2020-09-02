using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public void Spawn(RaycastHit hit)
    {
        Instantiate(gameObject, hit.point, Quaternion.identity);
    }
}
