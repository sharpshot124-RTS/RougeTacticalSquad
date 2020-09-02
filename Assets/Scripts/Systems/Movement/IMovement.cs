using UnityEngine;


public interface IMovement
{  
    bool Stopped { get; }

    float Speed { get; set; }

    void Move(Vector3 direction);

    void MoveTo(Vector3 location);

    Vector3 Destination { get; set; }
}
