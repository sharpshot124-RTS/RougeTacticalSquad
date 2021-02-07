using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Systems.Level.Plots;
using Assets.Scripts.Systems.Temp;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using ObjPlot = Assets.Scripts.Systems.Temp.ObjectivePlot;
using PlyrPlot = Assets.Scripts.Systems.Temp.PlayerPlot;

[CreateAssetMenu(fileName ="New_Level", menuName = "Custom/Levels/New Level")]
public class GeneratedLevel : ScriptableObject, ILevel
{
    public Vector2Int Size
    {
        get { return size; }
        private set { size = value; }
    }

    public Vector2Int size;
    public float enemiesPerDegree = 2;
    public float NoiseZoom { get; set; } = 25;

    public Color background, foreground;
    public string description;

    public Transform ContainerTransform { get; private set; }

    [SerializeField] private Object _player;
    public ILandPlot<PlayerData> Player { get => _player as ILandPlot<PlayerData>; set => _player = value as Object; }

    [SerializeField] private Object _root;
    public ILandPlot Root { get => _root as ILandPlot; set => _root = value as Object; }

    [SerializeField] private Object _fill;
    public ILandPlot Fill { get => _fill as ILandPlot; set => _fill = value as Object; }

    [SerializeField] private float _acreSize;
    public float AcreSize { get => _acreSize; set => _acreSize = value; }

    [SerializeField] private Object _enemies;
    public IZone Enemies { get => _enemies as IZone; set => _enemies = value as Object; }

    [SerializeField] protected List<Object> _zones;
    public List<IZone> Zones => _zones.ConvertAll((z) => z as IZone);

    [SerializeField] private float _degree;
    public float Degree { get => _degree; set => _degree = value; }

    public virtual ILevel Instantiate()
    {
        var result = CreateInstance<GeneratedLevel>();

        result.AcreSize = AcreSize;
        result.Enemies = Enemies;
        result.enemiesPerDegree = enemiesPerDegree;
        result.Player = Player;
        result.Root = Root;
        result.Size = Size;
        result._zones = Zones.ConvertAll<Object>((z) => z as Object);
        result.Degree = Degree;
        result.background = background;
        result.foreground = foreground;
        result.description = description;
        result._fill = _fill;

        return result;
    }

    public List<ILandPlot> Plots { get;  } = new List<ILandPlot>();
    public List<IPlot> TempInitPlots = new List<IPlot>();
    public List<IPlot> TempAfterInitPlots = new List<IPlot>();
    public Vector2Int InitSize { get; set; } = new Vector2Int(12, 12);
    ZoneFactory zoneFactory = new ZoneFactory();

    public List<IPlot> GetAllPlots()
    {
        return (List<IPlot>)TempInitPlots.Concat(TempAfterInitPlots);
    }

    public void ResetPlots()
    {
        TempInitPlots = new List<IPlot>();
        TempAfterInitPlots = new List<IPlot>();
    }

    public void GenerateInit()
    {
        ContainerTransform = new GameObject(description).transform;

        // Add Objective
        ObjPlot objectivePlot = new ObjPlot();
        TempInitPlots.Add(objectivePlot);

        // Add Enemies
        EnemyFactory enemyFactory = new EnemyFactory();
        TempInitPlots.AddRange(enemyFactory.getEnemies(enemiesPerDegree * Degree));

        // Add Player In Init Area
        PlyrPlot playerPlot = new PlyrPlot(InitSize);
        TempInitPlots.Add(playerPlot);

        //Add Zones In Init Area
        TempInitPlots.AddRange(zoneFactory.getZones(InitSize));

        // Generate and Bind init plots to the primary GameObject
        List<ZonePlot> initCollisionZonePlots = new List<ZonePlot>();

        foreach (IPlot plot in TempInitPlots)
        {
            bool hasCollision = plot.Generate(this);
            if (hasCollision && plot.GetType() == typeof(ZonePlot))
            {
                initCollisionZonePlots.Add((ZonePlot) plot);
            }
        }

        // Fill Gaps
        FillGaps(initCollisionZonePlots);
    }

    public IEnumerator GenerateAfterInit()
    {
        // Get Remaining Zones
        TempAfterInitPlots.AddRange(zoneFactory.getZones(Size, InitSize));

        // Generate and Bind init plots to the primary GameObject
        List<ZonePlot> afterInitCollisionZonePlots = new List<ZonePlot>();

        // Post Process Remaining Zones (Possibly in chunks?)
        foreach (var plot in TempAfterInitPlots)
        {
            bool hasCollision = plot.Generate(this);
            if (hasCollision && plot.GetType() == typeof(ZonePlot))
            {
                afterInitCollisionZonePlots.Add((ZonePlot) plot);
            }

            yield return new WaitForEndOfFrame();
        }

        // Fill Gaps
        yield return FillGapsOverTime(afterInitCollisionZonePlots);
    }

    private IEnumerator FillGapsOverTime(List<ZonePlot> collisionZonePlots)
    {
        foreach (ZonePlot zonePlot in collisionZonePlots)
        {
            int x = zonePlot.GetX();
            int y = zonePlot.GetY();

            FillGap(x, y);

            yield return new WaitForEndOfFrame();
        }
    }

    // TODO Check to see if we can validate that there are gaps without looping over everything.
    public void FillGaps(List<ZonePlot> collisionZonePlots)
    {
        foreach (ZonePlot zonePlot in collisionZonePlots)
        {
            int x = zonePlot.GetX();
            int y = zonePlot.GetY();

            FillGap(x, y);

        }
    }

    public void FillGap(int x, int y)
    {
        Fill.Transform = new Vector3Int(x, y, 0);

        if (!PlotHelper.Instance.Collides(Plots, Fill))
        {
            //Create Tile
            var plot = Fill.Instantiate(Degree);
            PlotHelper.Instance.PositionPrefab(plot.Tile, plot.Transform, ContainerTransform, AcreSize);

            //Add Building
            Plots.Add(plot);
        }
    }
}
