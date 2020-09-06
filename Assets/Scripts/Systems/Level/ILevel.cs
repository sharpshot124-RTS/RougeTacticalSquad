using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevel
{
    BiomeType Biome { get; set; }

    EnemyType Enemy { get; set; }

    ObjectiveType Objective { get; set; }
}

public enum BiomeType
{
    Valley,
    Hills,
    River
}

public enum EnemyType
{
    Mech
}

public enum ObjectiveType
{
    Survive,
    Destroy
}
