using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun : IDamager
{
    float Range { get; }

    IProjectile Projectile { get; }

    ICurrency Ammo { get; set; }

    void Fire(Vector3 target);

    void ApplyDamage(RaycastHit hit);
}
