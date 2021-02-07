using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Systems.Level.Plots;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Systems.Temp
{
    class ZoneFactory
    {
        public IEnumerable<ZonePlot> getZones(Vector2Int size)
        {
            float seed = Random.value * 500;

            List<ZonePlot> zonePlots = new List<ZonePlot>();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    zonePlots.Add(new ZonePlot(seed, x, y));
                }
            }

            return zonePlots;
        }

        public IEnumerable<ZonePlot> getZones(Vector2Int wholeArea, Vector2Int excludedArea)
        {
            float seed = Random.value * 500;

            List<ZonePlot> zonePlots = new List<ZonePlot>();

            for (int x = 0; x < wholeArea.x; x++)
            {
                for (int y = 0; y < wholeArea.y; y++)
                {
                    if (!(x < excludedArea.x && y < excludedArea.y))
                    {
                        zonePlots.Add(new ZonePlot(seed, x, y));
                    }
                }
            }

            return zonePlots;
        }
    }
}
