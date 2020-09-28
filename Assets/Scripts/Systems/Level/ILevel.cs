using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevel
{
    float AcreSize { get; set; }

    float Degree { get; set; }

    IZone Enemies { get; }

    List<IZone> Zones { get; }

    void Generate();

    ILevel Instantiate();
}

public interface IZone
{
    List<ILandPlot> Plots { get; }

    ILandPlot GetPlot();

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
