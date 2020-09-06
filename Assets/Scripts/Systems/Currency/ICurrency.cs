using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICurrency

{
    float CurrentValue { get; set; }

    float MaxValue { get; set; }

    void ChangeValue(float delta);

}
