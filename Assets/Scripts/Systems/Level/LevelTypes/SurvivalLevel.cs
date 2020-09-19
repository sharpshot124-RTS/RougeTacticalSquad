using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New_SurvivalLevel", menuName = "Custom/Levels/Survival")]
public class SurvivalLevel : GeneratedLevel
{
    public AnimationCurve timeRequirementCurve;

    public new ILandPlot Root { get => base.Root as TimerPlot; set => base.Root = value; }

    public override void Generate()
    {
        container = new GameObject().transform;
        List<ILandPlot> plots = new List<ILandPlot>();

        //Add root tile
        var obj = Root.Instantiate();
        obj.Transform = new Vector3Int(0, 0, Random.Range(0, 3));
        PositionPrefab(obj.Tile, obj.Transform, container);
        plots.Add(obj);

        //Generate Zones
        plots = GenerateZones(plots);

        //Order by distance
        plots = plots.OrderBy((p) => { return -Vector2Int.Distance(obj.Transform.RotatedPosition(), p.Transform.RotatedPosition()); }).ToList();

        //Generate Player        
        plots = GeneratePlayerTile(plots);

        //Set Enemies
        for (int e = 0; e < enemiesPerDegree * Degree; e++)
        {
            plots = GenerateEnemyTiles(plots);
        }

        //Create event for winning and set parameters
        var winCondition = new TimerEvent()
        {
            atTime = timeRequirementCurve.Evaluate(Degree) * 60,
            toDo = new UnityEvent()
        };

        //Attach UnityEvent to event
        winCondition.toDo.AddListener(() =>
        {
            OnLevelWin.Invoke();

            LevelManager.Instance.UnloadScene(2); // 2 = Generated build index
            LevelManager.Instance.LoadTransition();
        });

        //Attach event to tile
        var timer = obj as TimerPlot;

        timer.Feature.events.Clear();
        timer.Feature.events.Add(winCondition);

        OnGenerated.Invoke();
    }

    public override ILevel Instantiate()
    {
        Debug.Log("survival Instantiated");

        var result = CreateInstance<SurvivalLevel>();

        result.AcreSize = AcreSize;
        result.Enemies = Enemies;
        result.enemiesPerDegree = enemiesPerDegree;
        result.Player = Player;
        result.Root = Root;
        result.timeRequirementCurve = timeRequirementCurve;
        result.size = size;
        result._zones = Zones.ConvertAll<Object>((z) => z as Object);
        result.Degree = Degree;

        result.OnGenerated = result.OnLevelWin = new UnityEvent();

        return result;
    }
}
