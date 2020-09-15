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

    public static Vector2Int RotatedPosition(this Vector3Int start)
    {
        switch(start.z % 4)
        {
            default:
                return new Vector2Int(start.x, start.y);
            case 1:
                return new Vector2Int(start.y, -start.x);
            case 2:
                return new Vector2Int(-start.x, -start.y);
            case 3:
                return new Vector2Int(-start.y, start.x);
        }
    }
}