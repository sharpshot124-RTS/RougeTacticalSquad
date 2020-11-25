using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextNumber : Text
{
    public void SetText(int number)
    {
        base.text = (number).ToString();
    }

    public void SetText(float number)
    {
        base.text = (number).ToString();
    }

}
