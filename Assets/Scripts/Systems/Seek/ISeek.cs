using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISeek : IEntity
{
    IEnumerable<YieldInstruction> Seek(Vector3 target, Action<RaycastHit> onFound);

    void Seek(Vector3 target);

    Vector3 LastPosition { get; }

    RaycastHit GetTarget { get; }

    RaycastHit[] GetTargets { get; }

    LayerMask Mask { get; set; }

    Transform Transform { get; }
}
