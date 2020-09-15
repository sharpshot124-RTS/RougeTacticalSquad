using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_TimerLandPlot", menuName = "Custom/Levels/Plots/Timer")]
public class TimerPlot : ScriptableObject, ILandPlot<Timer>
{
    [SerializeField] private Vector3Int _transform;
    public Vector3Int Transform 
    { 
        get => _transform; 
        set => _transform = value; 
    }

    public Timer Feature => Tile.GetComponent<Timer>();

    [SerializeField] List<Vector2Int> _acres;
    public List<Vector2Int> Acres { get => _acres; set => _acres = value; }

    [SerializeField] private GameObject _tile;
    public GameObject Tile { get => _tile; set => _tile = value; }

    public IEnumerable<Vector2Int> GetTransformedAcres()
    {
        foreach(var a in Acres)
        {
            yield return 
                new Vector2Int(Transform.x, Transform.y) +
                new Vector3Int(a.x, a.y, Transform.z).RotatedPosition();
        }
    }

    public ILandPlot Instantiate()
    {
        var result = CreateInstance<TimerPlot>();
        result.Tile = Instantiate(Tile);
        result.Acres = _acres;
        result.Transform = Transform;

        return result;
    }
}
