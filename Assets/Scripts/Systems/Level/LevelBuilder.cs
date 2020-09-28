using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public List<string> sylables;
    public List<GeneratedLevel> Levels;
    public GameObject UIPlanet;

    public void CreateLevel()
    {
        var level = Levels[Random.Range(0, Levels.Count)];

        level = level.Instantiate() as GeneratedLevel;

        var planet = Instantiate(UIPlanet).GetComponent<UIPlanet>();
        planet.transform.SetParent(transform);

        planet.SetPlanet(level);
        planet.SetName(GenerateName(Random.Range(2, 6)));
    }

    string GenerateName(int sylableCount)
    {
        var result = "";
        for (int i = 0; i < sylableCount; i++)
        {
            result += sylables[Random.Range(0, sylables.Count)];
        }

        return result;
    }
}
