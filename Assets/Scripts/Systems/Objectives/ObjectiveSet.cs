using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveSet : MonoBehaviour, IObjective
{
    public List<ObjectiveLock> locks;
    [SerializeField] private bool autoCheckCompletion;

    [SerializeField] private bool _isComplete;
    public bool IsCompleted
    {
        get { return _isComplete; }

        set { _isComplete = value; }
    }

    [SerializeField] private UnityEvent _onComplete;
    public UnityEvent OnComplete
    {
        get { return _onComplete; }
    }

    public void Completion()
    {
        if (IsCompleted = CheckCompleted())
        {
            _onComplete.Invoke();
        }
    }

    public bool CheckCompleted()
    {
        foreach (var l in locks)
        {
            if (!l.unlocked)
            {
                return false;
            }
        }

        return true;
    }

    public void SetUnlocked(int index)
    {
        locks[index].unlocked = true;

        if(autoCheckCompletion)
            Completion();
    }

    public void SetLocked(int index)
    {
        locks[index].unlocked = false;

        if (autoCheckCompletion)
            Completion();
    }
}

[Serializable]
public class ObjectiveLock
{
    public string name;
    public bool unlocked = false;
}