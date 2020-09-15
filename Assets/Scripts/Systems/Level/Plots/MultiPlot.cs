using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New_LandPlot", menuName = "Custom/Levels/Plots/Basic", order = 0)]
public class MultiPlot : ScriptableObject, ILandPlot
{
    [SerializeField] List<ILandPlot> plots;

    [SerializeField] private GameObject _tile;
    public GameObject Tile { get => _tile; set => _tile = value; }

    [SerializeField] Vector3Int _transform;
    public Vector3Int Transform { get => _transform; set => _transform = value; }

    [SerializeField] protected List<Vector2Int> _acres = new List<Vector2Int>();
    public List<Vector2Int> Acres
    {
        get {
            IEnumerable<Vector2Int> result = new List<Vector2Int>();
            foreach(var p in plots)
            {
                result = result.Concat(p.Acres);
            }
            return new List<Vector2Int>(result);
        }

        set => throw new NotImplementedException("Cannot set Acres for MultiPlot objects. Set this.plots instead");
    }

    public IEnumerable<Vector2Int> GetTransformedAcres()
    {
        foreach (var p in plots)
        {
            foreach(var a in GetTransformedAcres())
            {
                yield return
                    new Vector2Int(Transform.x, Transform.y) +
                    new Vector3Int(a.x, a.y, Transform.z).RotatedPosition();
            }
        }
    }

    public ILandPlot Instantiate()
    {
        var result = CreateInstance<MultiPlot>();

        foreach(var p in plots)
        {
            var plot = p.Instantiate();

            result.plots.Add(plot);
        }
        
        result.Transform = Transform;
        result.Tile = Tile;

        return result;
    }
}
