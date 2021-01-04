using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Areas are constructs used for getting a point within arbitrary parameters and contexts.
/// </summary>
public interface IArea : IEntity
{
    /// <summary>
    /// Base location of the Area
    /// </summary>
    Vector3 Center { get; }

    /// <summary>
    /// Base scale of the Area
    /// </summary>
    Vector3 Size { get; }

    /// <summary>
    /// Get point according to point input
    /// </summary>
    /// <param name="targetPoint">Input value for the area</param>
    /// <returns>Calculated output point</returns>
    Vector3 GetPositionInArea(Vector3 targetPoint);

    /// <summary>
    /// Gets the next available point
    /// </summary>
    /// <returns>A point within the area</returns>
    Vector3 GetPoint();
}
