using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamager
{
    float Damage { get; set; }

    void ApplyDamage(IHealth target);
}
