using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New_SurvivalLevel", menuName = "Custom/Levels/Survival")]
public class SuvivalLevel : GeneratedLevel
{
    public AnimationCurve timeRequirementCurve;
    public float degree;

    public new ILandPlot Root => base.Root as TimerPlot;

    public new void Generate()
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
        for (int e = 0; e < maxEnemies; e++)
        {
            plots = GenerateEnemyTiles(plots);
        }

        //Create event for winning and set parameters
        var winCondition = new TimerEvent()
        {
            atTime = timeRequirementCurve.Evaluate(degree) * 60,
            toDo = new UnityEngine.Events.UnityEvent()
        };

        //Attach UnityEvent to event
        winCondition.toDo.AddListener(() =>
        {
            OnLevelWin.Invoke();
        });

        //Attach event to tile
        var timer = obj as TimerPlot;
        timer.Feature.events.Clear();
        timer.Feature.events.Add(winCondition);

        OnGenerated.Invoke();
    }
}
