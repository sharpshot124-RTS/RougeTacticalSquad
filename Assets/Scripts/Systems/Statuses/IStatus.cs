using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus
{
    void Apply();
}

public interface IStatus<T> : IStatus
{
    T Data { get; set; }
    IUnit Target { get; set; }
}
