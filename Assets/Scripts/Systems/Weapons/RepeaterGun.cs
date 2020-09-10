using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class RepeaterGun : MonoBehaviour, IGun
{
#if UNITY_EDITOR_WIN
    private bool showDebug = false;
#endif
    //private Dictionary<IProjectile, GameObject> pool = new Dictionary<IProjectile, GameObject>();
    private List<IProjectile> pool = new List<IProjectile>();

    private float lastFire = -1;
    private Coroutine constantFire;
    private Vector3 target;

    [SerializeField] private Vector3 muzzleSpread;
    [SerializeField] private float maxArcHeight = 20;
    [SerializeField] private LayerMask mask;
    [SerializeField] private RaycastHitUnityEvent onFire, onHit;


    [SerializeField] private float _range;

    public float Range
    {
        get { return _range; }
    }

    [SerializeField] private Object _ammoCount;

    public ICurrency Ammo
    {
        get { return _ammoCount as ICurrency; }

        set { _ammoCount = value as Object; }
    }
    [SerializeField] float _damage = 10;
    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    [SerializeField] private Object _projectile;
    public IProjectile Projectile
    {
        get { return _projectile as IProjectile; }
    }

    [SerializeField] private Transform _muzzle;
    public Transform Muzzle
    {
        get { return _muzzle; }
        set { _muzzle = value; }
    }


    [SerializeField] private float _fireRate = 10;
    public float FireRate
    {
        get { return _fireRate; }
        set { _fireRate = value; }
    }

    Vector3 GetSpread()
    {
        var result = Random.insideUnitSphere;
        result.Scale(muzzleSpread);

        return result;
    }

    public void Fire(Vector3 target)
    {
        this.target = target;

        if (CanFire())
        {
            var bullet = GetBullet();

            bullet.StartPosition = Muzzle.position;
            bullet.Fire(target + GetSpread());

            RaycastHit hit;
            Physics.Raycast(new Ray(Muzzle.position, target), out hit);
            hit.point = target;
            onFire.Invoke(hit);

            lastFire = Time.time;
            Ammo.CurrentValue--;
        }
    }

    public void Fire(RaycastHit hit)
    {
        Fire(hit.point);
    }

    IProjectile GetBullet()
    {
        foreach (var p in pool)
        {
            if (p.Firing == null)
                return p;
        }

        IProjectile newProj = Projectile.Instantiate();
        newProj.OnHit.AddListener((hit) => onHit.Invoke(hit));
        newProj.Mask = mask;

        newProj.Transform.gameObject.SetActive(true);

        pool.Add(newProj);

        return newProj;
    }

    bool CanFire()
    {
        if (Ammo.CurrentValue <= 0)
        {
            return false;
        }

        float nextFireTime = lastFire + (1 / FireRate);
        return Time.time >= nextFireTime;
    }

    public void BeginFiring()
    {
        StopFiring();

        constantFire = StartCoroutine(ConstantFire());
    }

    public void StopFiring()
    {
        if (constantFire != null)
            StopCoroutine(constantFire);

        constantFire = null;
    }

    public IEnumerator ConstantFire()
    {
        while (true)
        {
            Fire(target);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ApplyDamage(IHealth target)
    {
        if (target != null)
        {
            target.ChangeValue(-_damage);
        }
    }

    public void ApplyDamage(RaycastHit hit)
    {
        var health = hit.transform.GetComponent<IHealth>();

        ApplyDamage(health);
    }
}