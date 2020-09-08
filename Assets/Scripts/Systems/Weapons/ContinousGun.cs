using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public class ContinousGun : MonoBehaviour, IGun
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private RaycastUnityEvent onHit, onFire;
    [SerializeField] private UnityEvent onFireStart, onFireEnd;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float firingLength;
    [SerializeField] float endFireTime;


    private Coroutine firing;
    private Dictionary<IHealth, float> lastDamageTimes = new Dictionary<IHealth, float>();
    private Vector3 fireTarget;


    [SerializeField] private Object _ammoCount;

    public ICurrency Ammo
    {
        get { return _ammoCount as ICurrency; }

        set { _ammoCount = value as Object; }
    }


    [SerializeField] private float _damage;

    public float Damage
    {
        get { return _damage; }

        set { _damage = value; }
    }

    [SerializeField] private float _range;

    public float Range
    {
        get { return _range; }
    }

    [SerializeField] private Object projectile;
    private IProjectile _projectileInstance = null;

    public IProjectile Projectile
    {
        get
        {
            if (_projectileInstance == null)
            {
                //Instantiate
                _projectileInstance = ((IProjectile)projectile).Instantiate();

                //Set Properties
                _projectileInstance.Mask = mask;
                var follow = _projectileInstance.Transform.gameObject.AddComponent<FollowTransform>();
                follow.target = muzzle;
                //Projectile.Transform.SetParent(muzzle);
                //Projectile.Transform.localPosition = Vector3.zero;

                //Add Damage tick to OnHit event
                _projectileInstance.OnHit.AddListener(
                    (hit) => onHit.Invoke(hit));
            }

            return _projectileInstance;
        }
    }

    public void ApplyDamage(IHealth target)
    {
        target.ChangeValue(-Damage);
    }

    public void ApplyDamage(RaycastHit hit)
    {
        var health = hit.transform.GetComponent<IHealth>();

        if (health != null)
        {
            ApplyDamage(health);
        }
    }


    public void Fire(Vector3 target)
    {
        if (Ammo != null && Ammo.CurrentValue <= 0)
        {
            return;
        }

        endFireTime = Time.time + firingLength;
        Projectile.StartPosition = muzzle.position;

        var hit = new RaycastHit
        {
            point = target
        };

        onFire.Invoke(hit);

        if (firing == null)
        {
            firing = StartCoroutine(Firing(target));
        }
    }

    public void Fire(RaycastHit hit)
    {
        Fire(hit.point);
    }

    IEnumerator Firing(Vector3 target)
    {
        onFireStart.Invoke();
        Projectile.Fire(target);

        while (Time.time < endFireTime)
        {
            yield return new WaitForSeconds(endFireTime - Time.time);
        }

        firing = null;
        onFireEnd.Invoke();
        Projectile.Reset();
        Ammo.CurrentValue--;
    }
}
