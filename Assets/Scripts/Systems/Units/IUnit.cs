using System.Collections;
using UnityEngine;

public interface IUnit : IHealth, IMovement, IEntitiy
{
    // Combined Interface (interface inheriting from several interfaces)

    Coroutine StartCoroutine(IEnumerator coroutine);
}