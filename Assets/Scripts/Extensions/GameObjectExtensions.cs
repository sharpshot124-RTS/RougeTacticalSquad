using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public static class MonoExtensions
{
    /// <summary>
    /// Transforms a Vector3Int using the Z value as rotation (clockwise)
    /// </summary>
    /// <param name="start">Value to be transformed</param>
    /// <returns>Output Vector2Int point</returns>
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