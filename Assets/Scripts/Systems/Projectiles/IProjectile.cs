using UnityEngine;

public interface IProjectile : IEntity
{
    Vector3 StartPosition { get; set; }

    Vector3 EndPosition { get; set; }

    Coroutine Firing { get; set; }

    bool IsFiring { get; }

    RaycastUnityEvent OnHit { get; }

    ISeek Targets { get; set; }

    LayerMask Mask { get; set; }

    void Fire(Vector3 target);

    void Reset();

    IProjectile Instantiate();


}
