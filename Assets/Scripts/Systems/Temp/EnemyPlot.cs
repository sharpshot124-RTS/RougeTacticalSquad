using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Systems.Level.Plots;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Systems.Temp
{
    class EnemyPlot : IPlot
    {
        public bool Generate(ILevel level)
        {
            Vector3Int pos = new Vector3Int((int)((level.Size.x * (UnityEngine.Random.value / 2) + .5f)), (int)(level.Size.y * ((UnityEngine.Random.value / 2) + .5f)), 0);

            var enemy = level.Enemies.GetPlot(pos.x, pos.y).Instantiate(level.Degree);
            PlotHelper.Instance.PositionPrefab(enemy.Tile, enemy.Transform, level.ContainerTransform, level.AcreSize);

            //Check for collision
            bool hasCollision = PlotHelper.Instance.Collides(level.Plots, enemy);
            return hasCollision;
        }
    }
}
