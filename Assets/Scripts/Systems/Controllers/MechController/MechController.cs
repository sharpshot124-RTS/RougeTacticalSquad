using System.Collections;
using System.Collections.Generic;
using BitFSM;
using UnityEngine;

public class MechController : AIStateController, IController
{
    [SerializeField] private Object _iUnit;
    public IUnit Unit
    {
        get { return _iUnit as IUnit; }
    }

    [SerializeField] private Object _seeker;
    public ISeek Seeker
    {
        get { return _seeker as ISeek; }
    }

    [SerializeField] private Object _weapon;
    public IGun Weapon
    {
        get { return _weapon as IGun; }
    }

    [SerializeField] private Object _patrolArea;
    public IArea PatrolArea
    {
        get { return _patrolArea as IArea; }
    }

    [SerializeField] private Object _manueverArea;
    public IArea ManueverArea
    {
        get { return _manueverArea as IArea; }
    }

    public void EnableAI(bool state)
    {
        aiEnabled = state;
    }
}