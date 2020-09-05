using System.Collections;
using UnityEngine;

public interface IUnit : IHealth, IMovement, IEntity
{
    // Combined Interface (interface inheriting from several interfaces)

    Coroutine StartCoroutine(IEnumerator coroutine);
}