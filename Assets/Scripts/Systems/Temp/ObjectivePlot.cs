using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Systems.Level.Plots;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Systems.Temp
{
    class ObjectivePlot : IPlot
    {
        public bool Generate(ILevel level)
        {
            //Create Objective
            var obj = level.Root.Instantiate(level.Degree);
            obj.Transform = new Vector3Int(level.Size.x / 2, level.Size.y / 2, UnityEngine.Random.Range(0, 3));
            PlotHelper.Instance.PositionPrefab(obj.Tile, obj.Transform, level.ContainerTransform, level.AcreSize);

            //Add Objective
            level.Plots.Add(obj);

            //Check for collision
            bool hasCollision = PlotHelper.Instance.Collides(level.Plots, obj);
            return hasCollision;
        }
    }
}