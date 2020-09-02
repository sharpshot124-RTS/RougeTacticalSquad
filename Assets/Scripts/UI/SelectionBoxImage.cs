using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionBoxImage : Image
{
    private Coroutine drag = null;

    public void SetRect(Rect viewport)
    {
        rectTransform.position = viewport.position;
        rectTransform.sizeDelta = viewport.size;
    }

}
