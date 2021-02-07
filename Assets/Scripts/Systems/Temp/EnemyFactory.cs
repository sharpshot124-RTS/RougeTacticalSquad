using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Systems.Level.Plots;
using EnemPlot = Assets.Scripts.Systems.Temp.EnemyPlot;

namespace Assets.Scripts.Systems.Temp
{
    class EnemyFactory
    {
        public IEnumerable<IPlot> getEnemies(float enemiesPerDegree)
        {
            List<EnemPlot> enemies = new List<EnemPlot>();

            for (int e = 0; e < enemiesPerDegree; e++)
            {
                enemies.Add(new EnemPlot());
            }

            return enemies;
        }
    }
}
