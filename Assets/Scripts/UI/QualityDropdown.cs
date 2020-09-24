using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualityDropdown : Dropdown
{
    new void Start()
    {
        base.Start();

        options.Clear();

        foreach(var setting in QualitySettings.names)
        {
            options.Add(new OptionData(setting));
        }
    }
}
