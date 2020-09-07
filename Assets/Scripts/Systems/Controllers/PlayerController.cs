using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IController
{
    [SerializeField] private InputEventBinding[] actions;

    void Update()
    {
        Listen();
    }

    void Listen()
    {
        //Initialize a RacastHit to be reused for events on the same button press, nullable to check initialization
        //RaycastHit hit;

        foreach (var a in actions)
        {
            if (!a.enabled)
                continue;

            float inputValue;
            try
            {
                inputValue = Input.GetAxis(a.axis);
            }
            catch (ArgumentException)
            {
                Debug.LogWarning(string.Format("Axis '{0}' is not set up.", a.axis));
                continue;
            }

            if (Mathf.Abs(inputValue) >= .01 || Input.GetButton(a.axis))
            {
                if(a.continuous)
                    a.action.InvokeAtCursorPoint(a.mask);
                else if(Input.GetButtonDown(a.axis))
                    a.action.InvokeAtCursorPoint(a.mask);
            }
        }
    }

    public void EnableAll(bool enabled)
    {
        foreach (var a in actions)
        {
            a.enabled = enabled;
        }
    }

    public void EnableBinding(int index)
    {
        actions[index].enabled = true;
    }

    public void DisableBinding(int index)
    {
        actions[index].enabled = false;
    }

    public void FlipBinding(int index)
    {
        actions[index].enabled = !actions[index].enabled;
    }
}

[Serializable]
public class InputEventBinding
{
    public bool enabled = true;
    public string axis;
    public RaycastUnityEvent action;
    public LayerMask mask;
    public bool continuous = true;
}