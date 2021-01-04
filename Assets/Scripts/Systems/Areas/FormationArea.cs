using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Collection of paths with positions. Used for situations regarding several sets of arbitrary positions. Uses DoTween.
/// </summary>
public class FormationArea : MonoBehaviour, IArea
{
    public TweenPath[] paths;
    private int lastIndex;

    public Vector3 Center
    {
        get
        {
            return new Vector3(
                paths.Average(p => p.Value.x),
                paths.Average(p => p.Value.y),
                paths.Average(p => p.Value.z));
        }
    }

    public Vector3 Size
    {
        get
        {
            var longest = paths.OrderBy(p => p.Path.PathLength()).Last();

            return longest.GetPoint(1) - longest.GetPoint(0);
        }
    }

    public void Start()
    {
        foreach (var path in paths)
        {
            path.Path.Play();
        }
    }

    public void ProceedPath(int index)
    {
        paths[index].Path.Play();
    }

    public void PausePath(int index)
    {
        paths[index].Path.Pause();
    }

    public void RestartPath(int index)
    {
        paths[index].Path.Restart();
    }

    public Transform Transform
    {
        get { return transform; }
    }

    public Vector3 GetPositionInArea(Vector3 targetPoint)
    {
        return paths.OrderBy(p => Vector3.Distance(p.Value, targetPoint)).First().Value;
    }

    public Vector3 GetPoint()
    {
        return paths[lastIndex++ % paths.Length].GetPoint();
    }
}

[Serializable]
public class TweenPath
{
    private Vector3 _value;
    public Vector3 offset;

    public Vector3 Value
    {
        get { return _value + offset; }
        set { _value = value; }
    }

    [SerializeField] private Vector3UnityEvent onPathStart, onPathComplete, onPathUpdate;
    private Tween _path = null;
    public Tween Path
    {
        get
        {
            if (_path == null)
                _path = CreateSequence();

            return _path;
        }
    }

    [SerializeField]
    private PathSegment[] _points;

    public PathSegment[] Points
    {
        get { return _points; }
        set { _points = value; }
    }

    Tween CreateSequence()
    {
        Sequence result = DOTween.Sequence().OnStart(() =>
            onPathStart.Invoke(Value)).OnComplete(() =>
            onPathComplete.Invoke(Value)).OnUpdate(() =>
            onPathUpdate.Invoke(Value));

        for (int i = 0; i < Points.Length; i++)
        {
            int index = i;
            result.Append(DOTween.To(() => _value, v => _value = v, Points[i].target, Points[i].duration));
            result.AppendCallback(() =>
            {
                result.Pause();

                Points[index].OnComplete.Invoke();
            });
        }

        return result;
    }

    public Vector3 GetPoint(float? time = null)
    {
        return Value;
    }
}

[Serializable]
public class PathSegment
{
    public Vector3 target;
    public float duration;

    public UnityEvent OnComplete;
}