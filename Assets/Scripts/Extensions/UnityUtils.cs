using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityUtils
{
    /// <summary>
    /// Utility Function for starting a Coroutine.
    /// </summary>
    /// <param name="script">Object to host coroutine from</param>
    /// <param name="action">Action to be executed on step</param>
    /// <param name="yieldType">Type of yield for end of tick</param>
    /// <param name="duration">realtime duration of ticking</param>
    /// <param name="frequency">Number of ticks per step</param>
    /// <returns></returns>
    public static Coroutine StartCoroutine(this MonoBehaviour script, Action action, YieldInstruction yieldType, float duration, int frequency)
    {
        return script.StartCoroutine(GenericCoroutine(action, yieldType, frequency, duration));
    }

    /// <summary>
    /// Utility Function for starting a coroutine with callbacks
    /// </summary>
    /// <param name="script"></param>
    /// <param name="routine"></param>
    /// <param name="onStep"></param>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    public static Coroutine StartCoroutine(MonoBehaviour script, IEnumerable<YieldInstruction> routine, Action onStep = null, Action onComplete = null)
    {
        return script.StartCoroutine(GenericCoroutine(routine, onStep, onComplete));
    }

    /// <summary>
    /// Executes a coroutine with actions attached.
    /// </summary>
    /// <param name="routine"></param>
    /// <param name="onStep"></param>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    static IEnumerator GenericCoroutine(IEnumerable<YieldInstruction> routine, Action onStep, Action onComplete)
    {
        foreach (var step in routine)
        {
            onStep?.Invoke();
            yield return step;
        }
        onComplete?.Invoke();
    }

    /// <summary>
    /// Executes an action every n ticks for x seconds.
    /// </summary>
    /// <param name="action">Action to be executed</param>
    /// <param name="yieldType">Type of yield for end of tick</param>
    /// <param name="freq">Number of ticks between action executions</param>
    /// <param name="duration">realtime duration of ticking</param>
    /// <returns></returns>
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

    /// <summary>
    /// Checks if layer is true within mask
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer">Layer to be checked</param>
    /// <returns>True if layer value is true, else false</returns>
    public static bool ContainsLayer(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}