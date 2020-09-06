using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalLevel : MonoBehaviour, ILevel
{
    [SerializeField] private BiomeType _biome;
    public BiomeType Biome { get => _biome; set => _biome = value; }

    [SerializeField] private EnemyType _enemy;
    public EnemyType Enemy { get => _enemy; set => _enemy = value; }

    public ObjectiveType Objective { get => ObjectiveType.Survive; set => throw new System.NotImplementedException(); }

    [SerializeField] private int _degree;
    public int Degree { get => _degree; set => _degree = value; }
}
