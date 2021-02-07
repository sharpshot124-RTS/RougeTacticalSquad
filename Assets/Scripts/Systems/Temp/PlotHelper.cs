using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems.Temp
{
    class PlotHelper
    {
        private static PlotHelper _instance;

        private PlotHelper()
        {
        }

        public static PlotHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlotHelper();
                }
                return _instance;
            }
        }

        public bool Collides(IEnumerable<ILandPlot> plots, ILandPlot target)
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

        public void PositionPrefab(GameObject prefab, Vector3Int pos, Transform containerTransform, float AcreSize)
        {
            prefab.transform.position = new Vector3(
                AcreSize * pos.x,
                0,
                AcreSize * pos.y);

            prefab.transform.rotation = Quaternion.Euler(0, 90 * pos.z, 0);

            prefab.transform.SetParent(containerTransform);
        }
    }
}