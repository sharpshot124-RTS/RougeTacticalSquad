using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, ICurrency
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

    public FloatUnityEvent onHealthChange;
    public FloatUnityEvent onHealthChangeNormalized;
    public StringUnityEvent onHealthChangeString;
    public UnityEvent onDeath;

    public void ChangeValue(float delta)
    {
        float lastHealth = CurrentValue;
        CurrentValue = Mathf.Clamp(CurrentValue + delta, 0, MaxValue);

        //If health did not change, dont call events
        if (CurrentValue.Equals(lastHealth))
            return;

        if (CurrentValue <= 0)
        {
            onDeath.Invoke();
        }
        else
        {
            onHealthChangeNormalized.Invoke(CurrentValue / MaxValue);
            onHealthChange.Invoke(CurrentValue);
            onHealthChangeString.Invoke(CurrentValue.ToString());
        }
    }

    public void CheckValueChange(float delta)
    {
        var sum = CurrentValue + delta;

        if(sum > MaxValue || sum < 0)
        {
            throw new ArgumentOutOfRangeException("Delta is out of range");
        }
    }
}
