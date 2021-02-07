using System;
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
        private Vector2Int? _playerSpawnArea;

        public PlayerPlot()
        {

        }

        public PlayerPlot(Vector2Int playerSpawnArea)
        {
            this._playerSpawnArea = playerSpawnArea;
        }

        public bool Generate(ILevel level)
        {
            if (_playerSpawnArea == null)
            {
                _playerSpawnArea = level.Size;
            }

            Vector3Int playerPos = new Vector3Int((int)((_playerSpawnArea.Value.x * UnityEngine.Random.value / 2)), (int)(_playerSpawnArea.Value.y * UnityEngine.Random.value / 2), 0);

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
