using UnityEngine;

public interface IGyro
{
    Quaternion TargetRotation { get; set; }

    void Reset();
}
