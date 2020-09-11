using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIExtensions
{
    public static void SetText(this Text element, float text)
    {
        element.text = text.ToString();
    }
}
