using System;
using UnityEngine.Events;

public interface IHealth : ICurrency
{

    UnityEvent OnDeath { get; }

    UnityEvent<float> OnHealthChange { get; }

    UnityEvent<float> OnHelthChangeNormalized { get; }
}

