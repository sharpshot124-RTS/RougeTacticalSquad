using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems.Level.Plots
{
    public interface IPlot
    {
        bool Generate(ILevel level);
    }
}
