using System;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventExtensions
{

}

[Serializable]
public class FloatUnityEvent : UnityEvent<float> { }

[Serializable]
public class Vector3UnityEvent : UnityEvent<Vector3> { }

[Serializable]
public class MultiRaycastUnityEvent : UnityEvent<RaycastHit[]> { }

[Serializable]
public class RectUnityEvent : UnityEvent<Rect> { }