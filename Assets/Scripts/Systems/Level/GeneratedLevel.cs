using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New_Level", menuName = "Custom/Levels/Objective Level")]
public class GeneratedLevel : ScriptableObject, ILevel
{
    public float size;
    public float enemiesPerDegree = 2;
    public float noiseZoom = 25;

    public UnityEvent OnGenerated, OnLevelWin;
    protected Transform container;

    [SerializeField] private Object _player;
    public ILandPlot Player { get => _player as ILandPlot; set => _player = value as Object; }

    [SerializeField] private Object _objective;
    public ILandPlot Root { get => _objective as ILandPlot; set => _objective = value as Object; }

    [SerializeField] private float _acreSize;
    public float AcreSize { get => _acreSize; set => _acreSize = value; }

    [SerializeField] private Object _enemies;
    public IZone Enemies { get => _enemies as IZone; set => _enemies = value as Object; }

    [SerializeField] protected List<Object> _zones;
    public List<IZone> Zones => _zones.ConvertAll((z) => z as IZone);

    [SerializeField] private float _degree;
    public float Degree { get => _degree; set => _degree = value; }

    public virtual void Generate()
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

        OnGenerated.Invoke();
    }

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

        result.OnGenerated = result.OnLevelWin = new UnityEvent();

        return result;
    }

    protected List<ILandPlot> GenerateZones(List<ILandPlot> plots)
    {
        float seed = Random.value * 500;
        ILandPlot nextBuilding, root;

        for (int i = 0; i < size; i++)
        {
            root = plots[Random.Range(0, plots.Count)];

            var value = Zones.Count * Mathf.Clamp01(Mathf.PerlinNoise(
                seed + root.Transform.x / noiseZoom,
                seed + root.Transform.y / noiseZoom));

            var zone = Zones[Mathf.RoundToInt(value - .5f)];

            nextBuilding = zone.GetPlot();

            foreach (var cell in GetAdjacent(root))
            {
                nextBuilding.Transform = cell;
                if (!Collides(plots, nextBuilding))
                {
                    //Tile Placed
                    nextBuilding = nextBuilding.Instantiate();
                    PositionPrefab(nextBuilding.Tile, nextBuilding.Transform, container);

                    plots.Add(nextBuilding);
                    break;
                }
            }
        }

        return plots;
    }

    protected List<ILandPlot> GeneratePlayerTile(IEnumerable<ILandPlot> ordered)
    {
        var list = new List<ILandPlot>(ordered);
        foreach (var p in ordered)
        {
            var root = p;
            bool found = false;

            foreach (var cell in GetAdjacent(root))
            {
                Player.Transform = cell;
                if (!Collides(ordered, Player))
                {
                    //Tile Placed
                    var player = Player.Instantiate();
                    PositionPrefab(player.Tile, player.Transform, container);

                    list.Insert(0, player);
                    found = true;
                    break;
                }
            }

            if (found)
                break;
        }

        return list;
    }

    protected List<ILandPlot> GenerateEnemyTiles(IEnumerable<ILandPlot> ordered)
    {
        var list = ordered.ToList();
        ILandPlot nextBuilding, root;

        foreach (var p in ordered.Reverse())
        {
            bool found = false;
            root = p;
            nextBuilding = Enemies.GetPlot();

            foreach (var cell in GetAdjacent(root))
            {
                nextBuilding.Transform = cell;
                if (!Collides(ordered, nextBuilding))
                {
                    //Tile Placed
                    nextBuilding = nextBuilding.Instantiate();
                    PositionPrefab(nextBuilding.Tile, nextBuilding.Transform, container);

                    list.Insert(0, nextBuilding);
                    found = true;
                    break;
                }
            }

            if (found)
                break;
        }
        return list;
    }

    protected bool Collides(IEnumerable<ILandPlot> plots, ILandPlot target)
    {
        foreach(var p in plots)
        {
            foreach(var a in p.GetTransformedAcres())
            {
                foreach(var t in target.GetTransformedAcres())
                {
                    if (t == a)
                        return true;
                }
            }
        }
        return false;
    }

    protected IEnumerable<Vector3Int> GetAdjacent(ILandPlot root)
    {
        foreach(var a in root.GetTransformedAcres())
        {
            for(int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (Mathf.Abs(x) == Mathf.Abs(y))
                        continue;

                    var rotStart = Random.Range(0, 3);
                    for (int z = rotStart; z < rotStart + 4; z++)
                    {
                        yield return new Vector3Int(x + a.x, y + a.y, z);
                    }
                }
            }
        }
    }

    protected bool IsAdjacent(IEnumerable<ILandPlot> plots, ILandPlot target)
    {
        foreach (var p in plots)
        {
            foreach (var a in p.GetTransformedAcres())
            {
                foreach (var t in target.GetTransformedAcres())
                {
                    if (Mathf.Clamp(t.x, a.x - 1, a.x + 1) == t.x ||
                        Mathf.Clamp(t.y, a.y - 1, a.y + 1) == t.y)
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
