using System;
using UnityEngine.Events;

public interface IHealth
{
    float CurrentHealth { get; set; }

    float MaxHealth { get; set; }

    void ChangeHealth(float delta);

    UnityEvent OnDeath { get; }

    UnityEvent<float> OnHealthChange { get; }

    UnityEvent<float> OnHealthChangeNormalized { get; }
}

