using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Systems.Level.Plots;
using UnityEngine;

namespace Assets.Scripts.Systems.Temp
{
    public class ZonePlot : IPlot
    {
        private float seed;
        private int x, y;

        public ZonePlot(float seed, int x, int y)
        {
            this.seed = seed;
            this.x = x;
            this.y = y;
        }

        public bool Generate(ILevel level)
        {
            //Get noise value for zone selection
            var value = level.Zones.Count * Mathf.Clamp01(Mathf.PerlinNoise(
                seed + x / level.NoiseZoom,
                seed + y / level.NoiseZoom));

            //Get selected zone
            var zone = level.Zones[Mathf.RoundToInt(value - .5f)];

            //Get plot
            var plot = zone.GetPlot(x, y);

            //Check for collision
            bool hasCollision = PlotHelper.Instance.Collides(level.Plots, plot);
            if (!hasCollision)
            {
                //Create Tile
                plot = plot.Instantiate(level.Degree);
                PlotHelper.Instance.PositionPrefab(plot.Tile, plot.Transform, level.ContainerTransform, level.AcreSize);

                //Add Building
                level.Plots.Add(plot);
            }
            else
            {
                Debug.unityLogger.LogWarning("ZoneCollision", "Zone at x: " + x + " and y: " + y + " had a collision.");
            }

            return hasCollision;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }
    }
}
