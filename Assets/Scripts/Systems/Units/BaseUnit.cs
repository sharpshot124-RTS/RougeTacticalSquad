﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseUnit : MonoBehaviour, IUnit, ISelectable
{
    private NavMeshAgent _nav;
    public NavMeshAgent Nav
    {
        get { 
            if(_nav == null){
                _nav = GetComponent<NavMeshAgent>();
            }
            return _nav;
        }
    }

    [SerializeField] private float _currentHealth;
    public float CurrentHealth
    {
        get { return _currentHealth; }

        set { _currentHealth = value; }
    }

    [SerializeField] private float _MaxHealth;
    public float MaxHealth
    {
        get { return _MaxHealth; }

        set { _MaxHealth = value; }
    }

    [SerializeField] private FloatUnityEvent _onHealthChange;
    public UnityEvent<float> OnHealthChange
    {
        get { return _onHealthChange; }
    }

    [SerializeField]
    private FloatUnityEvent _onHealthChangeNormalized;
    public UnityEvent<float> OnHealthChangeNormalized
    {
        get { return _onHealthChangeNormalized; }
    }

    [SerializeField] private UnityEvent _onDeath;
    public UnityEvent OnDeath
    {
        get { return _onDeath; }
    }

    [SerializeField] private bool _selectable;
    public bool Selectable
    {
        get { return _selectable; }
        set
        {
            Deselect();
            _selectable = value;
        }
    }

    [SerializeField] private bool _selected = false;
    public bool Selected
    {
        get { return _selected; }

        set { _selected = value; }
    }

    [SerializeField] private UnityEvent _onSelect;
    public UnityEvent OnSelect
    {
        get { return _onSelect; }
    }

    [SerializeField] private UnityEvent _onDeselect;
    public UnityEvent OnDeselect
    {
        get { return _onDeselect; }
    }

    public Transform Transform
    {
        get { return transform; }
    }

    public GameObject GameObject
    {
        get { return gameObject; }
    }

    public float Speed
    {
        get { return Nav.speed; }
        set { Nav.speed = value; }
    }

    public Vector3 Destination
    {
        get { return Nav.pathEndPosition; }

        set { Move(value); }
    }

    public bool Stopped
    {
        get { return !Nav.pathPending &&
            (Nav.isStopped || Nav.isPathStale || Vector3.Distance(Nav.transform.position, Nav.pathEndPosition) <= Nav.stoppingDistance); }
    }

    public void ChangeHealth(float delta)
    {
        float lastHealth = CurrentHealth;
        CurrentHealth = Mathf.Clamp(CurrentHealth + delta, 0, MaxHealth);

        //If health did not change, dont call events
        if(CurrentHealth.Equals(lastHealth))
            return;

        OnHealthChangeNormalized.Invoke(CurrentHealth / MaxHealth);
        OnHealthChange.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            OnDeath.Invoke();
        }
    }

    public void Move(Vector3 direction)
    {
        Nav.destination = Transform.position + direction;
    }

    public void Move(UnityEngine.Object target)
    {
        var area = target as IArea;

        if(area != null)
            MoveTo(area.GetPoint());
    }

    public void Stop()
    {
        Move(Vector3.zero);
    }

    public void Select()
    {
        Select(!Selected);
    }

    public void Deselect()
    {
        Select(false);
    }

    public void Select(bool state)
    {
        if(Selected == state)
            return;

        if(!Selectable)
            return;

        Selected = state;

        if (Selected)
        {
            Debug.Log("Selected");
            OnSelect.Invoke();
        }
        else
        {
            Debug.Log("Deselected");
            OnDeselect.Invoke();
        }   
    }

    public void ToggleSelect()
    {
        Select(!Selected);
    }

    public void MoveTo(Vector3 location)
    {
        NavMeshHit hit;
        if(NavMesh.SamplePosition(location, out hit, 100, 1))
            Nav.destination = hit.position;
    }

    public void MoveTo(RaycastHit location)
    {
        MoveTo(location.point);
    }
}