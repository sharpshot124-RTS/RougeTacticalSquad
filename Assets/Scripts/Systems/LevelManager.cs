using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class LevelManager : MonoBehaviour
{
    static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            if(!_instance)
            {
                _instance = FindObjectOfType<LevelManager>();

                if(!_instance)
                {
                    var go = new GameObject();
                    _instance = go.AddComponent<LevelManager>();
                }
            }

            return _instance;
        }
    }

    public RunData data;

    static ILevel lastLevel;

    public int mainMenu, generated, transition, persistent;
    public float degreeGrowth;

    public IEnumerator LoadGenerated(ILevel level)
    {
        level = level.Instantiate();
        level.Degree += degreeGrowth;
        lastLevel = level;

        var loading = SceneManager.LoadSceneAsync(generated, LoadSceneMode.Additive);
        yield return loading;

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(generated));
        level.Generate();
    }

    public void LoadGenerated()
    {
        ObjLoadGenerated(lastLevel as Object);
    }

    public void ObjLoadGenerated(Object level)
    {
        Instance.StartCoroutine(LoadGenerated(level as ILevel));
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(mainMenu, LoadSceneMode.Additive);
    }

    public void LoadTransition()
    {
        SceneManager.LoadScene(transition, LoadSceneMode.Additive);
    }

    public void UnloadScene(int buildIndex)
    {
        SceneManager.UnloadSceneAsync(buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetEnergyAmmo(Object gun)
    {
        ((IGun)gun).Ammo = Instance.data.EnergyAmmo;
    }

    public void SetBallisticAmmo(Object gun)
    {
        ((IGun)gun).Ammo = Instance.data.BallisticAmmo;
    }

    public void SetHealth1(Object unit)
    {
        ((IUnit)unit).Health = Instance.data.Health1;
    }

    public void SetHealth2(Object unit)
    {
        ((IUnit)unit).Health = Instance.data.Health2;
    }

    public void SetHealth3(Object unit)
    {
        ((IUnit)unit).Health = Instance.data.Health3;
    }

    public void SetHealth4(Object unit)
    {
        ((IUnit)unit).Health = Instance.data.Health4;
    }
}

[Serializable]
public struct RunData
{
    [SerializeField] Object energyAmmo, ballisticAmmo, health1, health2, health3, health4;

    public ICurrency EnergyAmmo => energyAmmo as ICurrency;
    public ICurrency BallisticAmmo => ballisticAmmo as ICurrency;
    public ICurrency Health1 => health1 as ICurrency;
    public ICurrency Health2 => health2 as ICurrency;
    public ICurrency Health3 => health3 as ICurrency;
    public ICurrency Health4 => health4 as ICurrency;
}