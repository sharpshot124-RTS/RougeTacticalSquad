using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_PlayerPlot", menuName = "Custom/Levels/Plots/Player", order = 0)]
public class PlayerPlot : LandPlot
{
    public new ILandPlot Instantiate()
    {
        var result = CreateInstance<PlayerPlot>();
        result.Tile = Instantiate(Tile);
        result.Acres = Acres;
        result.Transform = Transform;

        return result;
    }
}
