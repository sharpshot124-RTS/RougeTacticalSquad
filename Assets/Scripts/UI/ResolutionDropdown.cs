using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDropdown : Dropdown
{
    new List<OptionData> options
    {
        get
        {
            base.options.Clear();
            foreach (var setting in Screen.resolutions)
            {
                base.options.Add(new OptionData(string.Format("{0}x{1}", Screen.width, Screen.height)));
            }
            return base.options;
        }

        set
        {
            base.options = value;
        }
    }
}
