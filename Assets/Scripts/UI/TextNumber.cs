using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextNumber : Text
{
    [SerializeField] private float scale, offset;

    public void SetText(int number)
    {
        text = (number * scale + offset).ToString();
    }

    public void SetText(float number)
    {
        text = (number * scale + offset).ToString();
    }
}
