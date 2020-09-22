using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAmmo : Ammo
{
    static SingletonAmmo _instance;

    public static SingletonAmmo Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<SingletonAmmo>();

                if (!_instance)
                {
                    var go = new GameObject();
                    _instance = go.AddComponent<SingletonAmmo>();
                }
            }

            return _instance;
        }
    }

    public new float CurrentValue
    {
        get { return (Instance as Ammo).CurrentValue; }
        set { (Instance as Ammo).CurrentValue = value; }
    }

    public new float MaxValue
    {
        get { return (Instance as Ammo).MaxValue; }
        set { (Instance as Ammo).MaxValue = value; }
    }
}
