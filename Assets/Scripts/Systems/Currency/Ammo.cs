using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour, ICurrency
{
    private float _curentAmmo;
    public float CurrentValue
    {
        get { return _currentAmmo; }
        set { _currentAmmo = value; }
    }

}
