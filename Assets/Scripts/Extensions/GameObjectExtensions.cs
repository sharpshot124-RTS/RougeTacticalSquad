using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public static class MonoExtensions
{
    public static T GetInterface<T>(this Object script)
    {
        return script.GetInterfaces<T>().FirstOrDefault();
    }

    public static IEnumerable<T> GetInterfaces<T>(this Object script)
    {
        return Object.FindObjectsOfType<MonoBehaviour>().OfType<T>();
    }
}