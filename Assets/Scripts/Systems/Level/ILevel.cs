using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base definition for a Level in the modular level system. Contains global data and a collection of Zones
/// </summary>
public interface ILevel
{
    /// <summary>
    /// Number of units per "Acre". Each tile cell is 1x1 acre.
    /// </summary>
    float AcreSize { get; set; }

    /// <summary>
    /// scalar value set at runtime used for scaling the level according to game state (i.e. late game levels have a higher degree)
    /// </summary>
    float Degree { get; set; }

    /// <summary>
    /// Zone used specifically to spawn enemies from during Generate
    /// </summary>
    IZone Enemies { get; }

    /// <summary>
    /// The zones which are used during Generate
    /// </summary>
    List<IZone> Zones { get; }

    /// <summary>
    /// Creates the level within the rendered game world according to current values. This function can take a large amount of time to execute.
    /// </summary>
    void Generate();

    /// <summary>
    /// Copy values to a new ILevel object of same type
    /// </summary>
    /// <returns>ILevel object of same type as this.</returns>
    ILevel Instantiate();
}


/// <summary>
/// Base definition for a Zone. Contains a collection of LandPlots that can be found within the Zone
/// </summary>
public interface IZone
{
    List<ILandPlot> Plots { get; }

    ILandPlot GetPlot(int x, int y);

    string Name { get; set; }
}

public interface ILandPlot
{
    Vector3Int Transform { get; set; }

    List<Vector2Int> Acres { get; set; }

    GameObject Tile { get; set; }

    IEnumerable<Vector2Int> GetTransformedAcres();

    ILandPlot Instantiate(float degree);
}

public interface ILandPlot<T> : ILandPlot
{
    T Feature { get; }
}
