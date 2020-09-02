using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ISelectable
{
    bool Selectable { get; set; }

    bool Selected { get; set; }

    void Select();

    void Deselect();

    UnityEvent OnSelect { get; }

    UnityEvent OnDeselect { get; }
}
