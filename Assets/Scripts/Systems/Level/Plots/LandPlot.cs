using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_LandPlot", menuName = "Custom/Levels/Plots/Basic", order = 0)]
public class LandPlot : ScriptableObject, ILandPlot
{
    [SerializeField] private GameObject _tile;
    public GameObject Tile { get => _tile; set => _tile = value; }

    [SerializeField] Vector3Int _transform;
    public Vector3Int Transform { get => _transform; set => _transform = value; }

    [SerializeField] List<Vector2Int> _acres = new List<Vector2Int>();
    public List<Vector2Int> Acres { get => _acres; set => _acres = value; }

    public IEnumerable<Vector2Int> GetTransformedAcres()
    {
        foreach (var a in Acres)
        {
            yield return
                new Vector2Int(Transform.x, Transform.y) +
                new Vector3Int(a.x, a.y, Transform.z).RotatedPosition();
        }
    }

    public ILandPlot Instantiate(float degree)
    {
        var result = CreateInstance<LandPlot>();
        result.Tile = Instantiate(Tile);
        result.Acres = _acres;
        result.Transform = Transform;

        return result;
    }
}
