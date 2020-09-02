using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IObjective
{
    bool IsCompleted { get; set; }

    void Completion();

    UnityEvent OnComplete { get; }
}
