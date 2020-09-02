using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityUtils
{
    public static Coroutine StartCoroutine(this MonoBehaviour script, Action action, YieldInstruction yieldType, float duration, int frequency)
    {
        return script.StartCoroutine(GenericCoroutine(action, yieldType, frequency, duration));
    }

    public static bool ContainsLayer(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static Coroutine StartCoroutine(MonoBehaviour script, IEnumerable<YieldInstruction> routine, Action onStep = null, Action onComplete = null)
    {
        return script.StartCoroutine(GenericCoroutine(routine, onStep, onComplete));
    }

    static IEnumerator GenericCoroutine(IEnumerable<YieldInstruction> routine, Action onStep, Action onComplete)
    {
        foreach (var step in routine)
        {
            onStep?.Invoke();
            yield return step;
        }
        onComplete?.Invoke();
    }

    static IEnumerator GenericCoroutine(Action action, YieldInstruction yieldType, int freq, float duration)
    {
        var start = Time.time;
        var time = start;
        int count = 0;

        while (time < start + duration)
        {
            yield return yieldType;

            count++;
            if (count >= freq)
            {
                action();
                count = 0;
            }
        }
    }
}

public enum YieldType
{
    EndOfFrame,
    FixedUpdate,
    Milliseconds
}