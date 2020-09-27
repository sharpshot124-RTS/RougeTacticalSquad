using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlanet : MonoBehaviour
{
    public StringUnityEvent onNameSet, onZoneSet;
    public ColorUnityEvent onForegroundSet, onBackgroundSet;

    public void SetPlanet(GeneratedLevel level)
    {
        var zones = "";
        foreach(var z in level.Zones)
        {
            zones += z.Name + ", ";
        }

        onZoneSet.Invoke(zones);
        onForegroundSet.Invoke(level.foreground);
        onBackgroundSet.Invoke(level.background);
    }

    public void SetName(string name)
    {
        onNameSet.Invoke(name);
    }
}
