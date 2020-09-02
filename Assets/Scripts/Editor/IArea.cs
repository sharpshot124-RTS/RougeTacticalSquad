using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArea : IEntitiy
{
    Vector3 Center { get; }

    Vector3 Size { get; }

    Vector3 GetPositionInArea(Vector3 targetPoint);

    Vector3 GetPoint();
}
