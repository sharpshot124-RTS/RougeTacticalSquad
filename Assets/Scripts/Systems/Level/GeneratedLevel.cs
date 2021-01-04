using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New_Level", menuName = "Custom/Levels/New Level")]
public class GeneratedLevel : ScriptableObject, ILevel
{
    public Vector2Int size;
    public float enemiesPerDegree = 2;
    public float noiseZoom = 25;

    public Color background, foreground;
    public string description;

    protected Transform container;

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
        result.size = size;
        result._zones = Zones.ConvertAll<Object>((z) => z as Object);
        result.Degree = Degree;
        result.background = background;
        result.foreground = foreground;
        result.description = description;
        result._fill = _fill;

        return result;
    }

    List<ILandPlot> plots = new List<ILandPlot>();
    public void Generate()
    {
        container = new GameObject(description).transform;       

        //Create Objective
        var obj = Root.Instantiate(Degree);
        obj.Transform = new Vector3Int(size.x / 2, size.y / 2, Random.Range(0, 3));
        PositionPrefab(obj.Tile, obj.Transform, container);

        //Add Objective
        plots.Add(obj);

        //Add Enemies
        for (int e = 0; e < enemiesPerDegree * Degree; e++)
        {
            Vector3Int pos = new Vector3Int((int)((size.x * (Random.value / 2) + .5f)), (int)(size.y * ((Random.value / 2) + .5f)), 0);

            var enemy = Enemies.GetPlot(pos.x, pos.y).Instantiate(Degree);
            PositionPrefab(enemy.Tile, enemy.Transform, container);

            plots.Add(enemy);
        }

        Vector3Int playerPos = new Vector3Int((int)((size.x * Random.value / 2)), (int)(size.y * Random.value / 2), 0);

        //Create Player Tile
        var player = Player.Instantiate(Degree);
        player.Transform = playerPos;
        PositionPrefab(player.Tile, player.Transform, container);

        //Add Player
        plots.Add(player);

        //Add Zones
        GenerateZones(plots);

        //Fill Gaps
        FillGaps(plots);
    }

    protected void GenerateZones(List<ILandPlot> plots)
    {
        float seed = Random.value * 500;
        ILandPlot nextBuilding;

        for(int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                //Get noise value for zone selection
                var value = Zones.Count * Mathf.Clamp01(Mathf.PerlinNoise(
                    seed + x / noiseZoom,
                    seed + y / noiseZoom));

                //Get selected zone
                var zone = Zones[Mathf.RoundToInt(value - .5f)];

                //Get plot
                nextBuilding = zone.GetPlot(x, y);

                //Check for collision
                if (Collides(plots, nextBuilding))
                {                    
                    continue;
                }

                //Create Tile
                nextBuilding = nextBuilding.Instantiate(Degree);                
                PositionPrefab(nextBuilding.Tile, nextBuilding.Transform, container);                

                //Add Building
                plots.Add(nextBuilding);
            }
        }
    }

    public void FillGaps(List<ILandPlot> plots)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Fill.Transform = new Vector3Int(x, y, 0);

                if (Collides(plots, Fill))
                    continue;

                //Create Tile
                var plot = Fill.Instantiate(Degree);
                PositionPrefab(plot.Tile, plot.Transform, container);

                //Add Building
                plots.Add(plot);
            }
        }
    }

    protected bool Collides(IEnumerable<ILandPlot> plots, ILandPlot target)
    {
        foreach (var p in plots)
        {
            var dist = p.Transform - target.Transform;
            if (p.Acres.Count + target.Acres.Count < Mathf.Abs(dist.x) + Mathf.Abs(dist.y))
                continue;

            foreach (var a in p.GetTransformedAcres())
            {
                foreach (var t in target.GetTransformedAcres())
                {
                    if (t == a)
                        return true;
                }
            }
        }
        return false;
    }

    protected void PositionPrefab(GameObject prefab, Vector3Int pos, Transform container)
    {
        prefab.transform.position = new Vector3(
            AcreSize * pos.x,
            0,
            AcreSize * pos.y);

        prefab.transform.rotation = Quaternion.Euler(0, 90 * pos.z, 0);

        prefab.transform.SetParent(container);
    }
}
