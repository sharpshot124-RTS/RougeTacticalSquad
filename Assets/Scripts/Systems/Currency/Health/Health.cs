using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField]
    private float _currentHealth;
    public float CurrentValue
    {
        get { return _currentHealth; }

        set { _currentHealth = value; }
    }

    [SerializeField]
    private float _MaxHealth;
    public float MaxValue
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
    public UnityEvent<float> OnHelthChangeNormalized
    {
        get { return _onHealthChangeNormalized; }
    }

    public void ChangeValue(float delta)
    {
        float lastHealth = CurrentValue;
        CurrentValue = Mathf.Clamp(CurrentValue + delta, 0, MaxValue);

        //If health did not change, dont call events
        if (CurrentValue.Equals(lastHealth))
            return;

        if (CurrentValue <= 0)
        {
            OnDeath.Invoke();
        }
        else
        {
            OnHealthChange.Invoke(CurrentValue / MaxValue);
            OnHealthChange.Invoke(CurrentValue);
        }
    }
}
