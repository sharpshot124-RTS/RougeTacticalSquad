using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField]
    private float _currentHealth;
    public float CurrentHealth
    {
        get { return _currentHealth; }

        set { _currentHealth = value; }
    }

    [SerializeField]
    private float _MaxHealth;
    public float MaxHealth
    {
        get { return _MaxHealth; }

        set { _MaxHealth = value; }
    }

    [SerializeField]
    private FloatUnityEvent _onHealthChange;
    public UnityEvent<float> OnHealthChange
    {
        get { return _onHealthChange; }
    }

    [SerializeField]
    private UnityEvent _onDeath;
    public UnityEvent OnDeath
    {
        get { return _onDeath; }
    }

    [SerializeField]
    private FloatUnityEvent _onHealthChangeNormalized;
    public UnityEvent<float> OnHealthChangeNormalized
    {
        get { return _onHealthChangeNormalized; }
    }

    public void ChangeHealth(float delta)
    {
        float lastHealth = CurrentHealth;
        CurrentHealth = Mathf.Clamp(CurrentHealth + delta, 0, MaxHealth);

        //If health did not change, dont call events
        if (CurrentHealth.Equals(lastHealth))
            return;

        if (CurrentHealth <= 0)
        {
            OnDeath.Invoke();
        }
        else
        {
            OnHealthChange.Invoke(CurrentHealth / MaxHealth);
            OnHealthChange.Invoke(CurrentHealth);
        }
    }
}
