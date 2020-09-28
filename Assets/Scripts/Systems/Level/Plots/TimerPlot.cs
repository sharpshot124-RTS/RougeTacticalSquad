using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_TimerLandPlot", menuName = "Custom/Levels/Plots/Timer")]
public class TimerPlot : ScriptableObject, ILandPlot<Timer>
{
    [SerializeField] private float minutesPerDegree = 1;

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

    public ILandPlot Instantiate(float degree)
    {
        var result = CreateInstance<TimerPlot>();        
        result.Acres = _acres;
        result.Transform = Transform;

        result.Tile = Instantiate(Tile);
        result.minutesPerDegree = minutesPerDegree;

        foreach (var e in result.Feature.events)
        {
            e.atTime = degree * minutesPerDegree * 60;
        }

        return result;
    }
}
