using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IProjectile : IEntitiy
{
    Vector3 StartPosition { get; set; }

    Vector3 EndPosition { get; set; }

    Coroutine Firing { get; set; }

    RaycastUnityEvent OnHit { get; }

    ISeek Targets { get; set; }

    LayerMask Mask { get; set; }

    void Fire(Vector3 target);

    void Reset();

    IProjectile Instantiate();


}
