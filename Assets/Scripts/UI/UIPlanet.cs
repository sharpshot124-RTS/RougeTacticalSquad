using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlanet : MonoBehaviour
{
    public StringUnityEvent onNameSet, onZoneSet, onDescriptionSet;
    public ColorUnityEvent onForegroundSet, onBackgroundSet;
    public Button loadLevel;
    public RectTransform lightingGraphic, bounds;

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
        onDescriptionSet.Invoke(level.description);

        lightingGraphic.anchoredPosition = new Vector3(
            Random.Range(bounds.rect.xMin, bounds.rect.xMax),
            Random.Range(bounds.rect.yMin, bounds.rect.yMax),
            bounds.position.z);

        loadLevel.onClick.AddListener(() =>
        {
            LevelManager.Instance.LoadGenerated(level);
            LevelManager.Instance.UnloadScene(LevelManager.Instance.transition);
        });
    }

    public void SetName(string name)
    {
        onNameSet.Invoke(name);
    }
}
