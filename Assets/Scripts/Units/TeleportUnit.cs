using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUnit : BaseUnit 
{
    public void Teleport(Vector3 destination)
    {
        transform.position = destination;
    }

    public void Teleport(RaycastHit destination){
        Teleport(destination.point);
    }
}
