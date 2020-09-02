using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour, IDamager
{
    [SerializeField]
    float _damage = 10;
    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    public void ApplyDamage(IHealth target)
    {
        Debug.Log(target != null);

        if (target != null)
        {
            target.ChangeHealth(-_damage);
        }
    }

    public void ApplyDamage(RaycastHit hit)
    {
        var health = hit.transform.GetComponent<IHealth>();

        ApplyDamage(health);
    }

    public void ApplyDamage(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            ApplyDamage(hit);
        }
    }
}
