using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float time;
    bool paused;

    public List<TimerEvent> events;

    public void Update()
    {
        if (!paused)
            time += Time.deltaTime;
        
        foreach(var e in events)
        {
            var diff = time - e.atTime;
            if(diff <= Time.deltaTime && diff > 0)
            {
                e.toDo.Invoke();
            }
        }
    }

    public void Pause(bool pause)
    {
        paused = pause;
    }

    public void Reset()
    {
        time = 0;
    }

    public void SetTime(float time)
    {
        this.time = time;
    }
}

[Serializable]
public class TimerEvent
{
    public UnityEvent toDo;
    public float atTime;
}