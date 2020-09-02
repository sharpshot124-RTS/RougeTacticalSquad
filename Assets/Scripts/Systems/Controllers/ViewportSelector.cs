using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ViewportSelector : MonoBehaviour, ISelector
{
    private Coroutine drag = null;

    [SerializeField] private new Camera camera;

    [SerializeField] private RectUnityEvent onDragStart, onDrag, onDragEnd;

    [SerializeField] private RectUnityEvent onDragSigned;

    [SerializeField] private float minDragDist = .05f;

    [SerializeField] private string axis = "Fire1";

    public void Select(ISelectable selected)
    {
        selected.Select();
    }

    public void Select(ISelectable[] selected)
    {
        foreach (var s in selected)
        {
            Select(s);
        }
    }

    public void BeginDrag()
    {
        if(drag != null)
            StopCoroutine(drag);

        drag = StartCoroutine(DragSelect());
    }

    IEnumerator DragSelect()
    {
        Vector2 startPos = Input.mousePosition;

        Rect viewport = new Rect(
            startPos.x, startPos.y, 0, 0);

        onDragStart.Invoke(viewport);

        while (Input.GetButton(axis))
        {
            yield return new WaitForEndOfFrame();

            Vector2 endPos = Input.mousePosition;

            onDragSigned.Invoke(new Rect(startPos, endPos));

            /// startPos and endPos make a box, we need the bottom left and top right corners (or size in this case, cuz of Rect)
            viewport.Set(
                Mathf.Min(startPos.x, endPos.x), //Left
                Mathf.Min(startPos.y, endPos.y), //Bottom
                Mathf.Max(startPos.x, endPos.x) - Mathf.Min(startPos.x, endPos.x), //Width
                Mathf.Max(startPos.y, endPos.y) - Mathf.Min(startPos.y, endPos.y)); //Height

            if (viewport.size.magnitude >= minDragDist)
                onDrag.Invoke(viewport);
        }

        onDragEnd.Invoke(viewport);

        drag = null;
    }

    public void GetSectablesInViewport(Rect viewport)
    {
        //Normalize to screen size
        viewport.Set(
            viewport.x / Screen.width,
            viewport.y / Screen.height,
            viewport.width / Screen.width,
            viewport.height / Screen.height);

        var selectables = FindObjectsOfType<Transform>().Where((c) => c.GetComponent<ISelectable>() != null);

        foreach (var s in selectables)
        {
            if (viewport.Contains(camera.WorldToViewportPoint(s.position)))
            {
                Select(s.GetComponent<ISelectable>());
                Debug.Log("Selected: " + s.name);
            }
        }
    }

    void Update()
    {
        if (Input.GetButtonDown(axis) && drag == null)
        {
            BeginDrag();
        }
    }
}
