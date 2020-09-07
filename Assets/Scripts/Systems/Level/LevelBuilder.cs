using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] Object _level;
    ILevel Level
    {
        get => _level as ILevel;
    }

    public ObjectiveSet mainObjective;

    public Timer survivalTimer;
    public AnimationCurve survivalTimeCurve;

    public GameObject waveTemplate;

    public GameObject valley, hills, river;

    [SerializeField]
    GameObject[] enemies;

    [SerializeField]
    Object[] objectives;

    public void Build()
    {
        switch (Level.Biome)
        {
            case BiomeType.Valley:
                valley.SetActive(true);
                valley.GetComponent<NavMeshSurface>().BuildNavMesh();
                break;
            case BiomeType.Hills:
                hills.SetActive(true);
                hills.GetComponent<NavMeshSurface>().BuildNavMesh();
                break;
            case BiomeType.River:
                river.SetActive(true);
                river.GetComponent<NavMeshSurface>().BuildNavMesh();
                break;
        }

        switch (Level.Objective)
        {
            case ObjectiveType.Survive:
                mainObjective.locks.Add(new ObjectiveLock()
                {
                    name = "Survived"
                });

                var e = new TimerEvent()
                {
                    atTime = survivalTimeCurve.Evaluate(Level.Degree) * 60,
                    toDo = new UnityEngine.Events.UnityEvent()
                };
                e.toDo.AddListener(() => mainObjective.SetUnlocked(0));

                survivalTimer.events.Add(e);
                break;
            case ObjectiveType.Destroy:
                ObjectiveSet prevWave = null;
                for(int i = 0; i < Level.Degree; i++)
                {
                    var wave = Instantiate(waveTemplate);
                    var waveObj = wave.GetComponent<ObjectiveSet>();

                    mainObjective.locks.Add(new ObjectiveLock()
                    {
                        name = "Wave " + i
                    });

                    waveObj.OnComplete.AddListener(() => mainObjective.SetUnlocked(i));

                    if(prevWave != null)
                    {
                        prevWave.OnComplete.AddListener(() => wave.SetActive(true));
                    }
                    else
                    {
                        wave.SetActive(true);
                    }
                    prevWave = waveObj;
                }
                break;
        }
    }
}
