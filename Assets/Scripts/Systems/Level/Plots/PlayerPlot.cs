using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_PlayerPlot", menuName = "Custom/Levels/Plots/Player", order = 0)]
public class PlayerPlot : ScriptableObject, ILandPlot<PlayerData>
{
    private Vector3Int _transform;
    public Vector3Int Transform
    {
        get => _transform;
        set => _transform = value;
    }

    [SerializeField] List<Vector2Int> _acres;
    public List<Vector2Int> Acres { get => _acres; set => _acres = value; }

    [SerializeField] private GameObject _tile;
    public GameObject Tile { get => _tile; set => _tile = value; }

    public IEnumerable<Vector2Int> GetTransformedAcres()
    {
        foreach (var a in Acres)
        {
            yield return
                new Vector2Int(Transform.x, Transform.y) +
                new Vector3Int(a.x, a.y, Transform.z).RotatedPosition();
        }
    }

    [SerializeField] private PlayerData data;

    public PlayerData Feature { get => data; set => data = value; }

    public ILandPlot Instantiate(float degree)
    {
        var result = CreateInstance<PlayerPlot>();
        result.Feature = Feature;
        result.Tile = Instantiate(Tile);
        result.Acres = Acres;
        result.Transform = Transform;

        return result;
    }
}

public struct PlayerData
{
    public BaseUnit[] units;
}