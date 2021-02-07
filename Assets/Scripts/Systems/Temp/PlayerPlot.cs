using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Systems.Level.Plots;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Systems.Temp
{
    class PlayerPlot: IPlot
    {
        public bool Generate(ILevel level)
        {
            Vector3Int playerPos = new Vector3Int((int)((level.Size.x * UnityEngine.Random.value / 2)), (int)(level.Size.y * UnityEngine.Random.value / 2), 0);

            //Create Player Tile
            var player = level.Player.Instantiate(level.Degree);
            player.Transform = playerPos;
            PlotHelper.Instance.PositionPrefab(player.Tile, player.Transform, level.ContainerTransform, level.AcreSize);

            //Add Player
            level.Plots.Add(player);

            //Check for collision
            bool hasCollision = PlotHelper.Instance.Collides(level.Plots, player);
            return hasCollision;
        }
    }
}
