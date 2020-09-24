using System.Collections;
using UnityEngine;

public interface IUnit : IMovement, IEntity
{
    ICurrency Health { get; set; }

    Coroutine StartCoroutine(IEnumerator coroutine);
}