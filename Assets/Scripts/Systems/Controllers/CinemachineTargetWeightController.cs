using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineTargetWeightController : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup group;
    public int Index = 0;

    public void SetTargetIndex(int index)
    {
        Index = index;
    }

    public void SetWeight(float weight)
    {
        group.m_Targets[Index].weight = weight;
    }
}
