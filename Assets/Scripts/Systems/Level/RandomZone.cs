using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_RandomZone", menuName = "Custom/Levels/New Zone")]
public class RandomZone : ScriptableObject, IZone
{
    [SerializeField] List<Object> _plots;
    public List<ILandPlot> Plots => _plots.ConvertAll((p) => p as ILandPlot);

    public string Name { get => name; set => name = value; }

    public ILandPlot GetPlot()
    {
        return _plots[Random.Range(0, _plots.Count)] as ILandPlot;
    }
}
