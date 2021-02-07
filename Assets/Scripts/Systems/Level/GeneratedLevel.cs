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
    public List<IPlot> TempPlots = new List<IPlot>();
    public void Generate()
    {
        ContainerTransform = new GameObject(description).transform;

        // Add Objective
        ObjPlot objectivePlot = new ObjPlot();
        TempPlots.Add(objectivePlot);

        // Add Enemies
        EnemyFactory enemyFactory = new EnemyFactory();
        TempPlots.AddRange(enemyFactory.getEnemies(enemiesPerDegree * Degree));

        // Add Player
        PlyrPlot playerPlot = new PlyrPlot();
        TempPlots.Add(playerPlot);

        //Add Zones
        Vector2Int tempVector2Int = new Vector2Int(12, 12);
        ZoneFactory zoneFactory = new ZoneFactory();
        TempPlots.AddRange(zoneFactory.getZones(tempVector2Int));

        // Generate and Bind everything to the primary GameObject
        List<ZonePlot> collisionZonePlots = new List<ZonePlot>();

        foreach (IPlot plot in TempPlots)
        {
            bool hasCollision = plot.Generate(this);
            if (hasCollision && plot.GetType() == typeof(ZonePlot))
            {
                collisionZonePlots.Add((ZonePlot)plot);
            }
        }

        //Fill Gaps
        FillGaps(collisionZonePlots);
    }

    // TODO Check to see if we can validate that there are gaps without looping over everything.
    public void FillGaps(List<ZonePlot> collisionZonePlots)
    {
        foreach (ZonePlot zonePlot in collisionZonePlots)
        {
            int x = zonePlot.getX();
            int y = zonePlot.getY();

            Fill.Transform = new Vector3Int(x, y, 0);

            if (!PlotHelper.Instance.Collides(Plots, Fill))
            {
                Debug.unityLogger.LogWarning("ZoneMissing", "Zone at x: " + x + " and y: " + y + " is missing.");
                //Create Tile
                var plot = Fill.Instantiate(Degree);
                PlotHelper.Instance.PositionPrefab(plot.Tile, plot.Transform, ContainerTransform, AcreSize);

                //Add Building
                Plots.Add(plot);
            }
        }
    }
}
