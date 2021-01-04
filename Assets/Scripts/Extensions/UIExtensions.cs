using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIExtensions
{
    /// <summary>
    /// Overload for settng a Text element with a float
    /// </summary>
    /// <param name="element">Element to display text</param>
    /// <param name="text">Number to be displayed as text</param>
    public static void SetText(this Text element, float text)
    {
        element.text = text.ToString();
    }
}
