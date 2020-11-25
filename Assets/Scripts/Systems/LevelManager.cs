using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        //level = level.Instantiate();
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

    public void SetMoney(Object unit)
    {
        ((IUnit)unit).Health = Instance.data.Money;
    }

    public void SetEnergyAmmo(float value)
    {
        Instance.data.EnergyAmmo.CurrentValue = value;
    }

    public void SetBallisticAmmo(float value)
    {
        Instance.data.BallisticAmmo.CurrentValue = value;
    }

    public void SetHealth1(float value)
    {
        Instance.data.Health1.CurrentValue = value;
    }

    public void SetHealth2(float value)
    {
        Instance.data.Health2.CurrentValue = value;
    }

    public void SetHealth3(float value)
    {
        Instance.data.Health3.CurrentValue = value;
    }

    public void SetHealth4(float value)
    {
        Instance.data.Health4.CurrentValue = value;
    }

    public void SetMoney(float value)
    {
        Instance.data.Money.CurrentValue = value;
    }

    public void ChangeEnergyAmmo(int count)
    {
        Instance.data.EnergyAmmo.ChangeValue(count);
    }

    public void ChangeBallisticAmmo(int count)
    {
        Instance.data.BallisticAmmo.ChangeValue(count);
    }

    public void ChangeHealth1(float count)
    {
        Instance.data.Health1.ChangeValue(count);
    }

    public void ChangeHealth2(float count)
    {
        Instance.data.Health2.ChangeValue(count);
    }

    public void ChangeHealth3(float count)
    {
        Instance.data.Health3.ChangeValue(count);
    }

    public void ChangeHealth4(float count)
    {
        Instance.data.Health4.ChangeValue(count);
    }

    public void ChangeMoney(float count)
    {
        Instance.data.Money.ChangeValue(count);
    }

    public void SetBallisticSlider(Slider slider)
    {
        slider.maxValue = Instance.data.BallisticAmmo.MaxValue;
        slider.value = Instance.data.BallisticAmmo.CurrentValue;
    }

    public void SetEnergySlider(Slider slider)
    {
        slider.maxValue = Instance.data.EnergyAmmo.MaxValue;
        slider.value = Instance.data.EnergyAmmo.CurrentValue;
    }

    public void SetHealth1Slider(Slider slider)
    {
        slider.maxValue = Instance.data.Health1.MaxValue;
        slider.value = Instance.data.Health1.CurrentValue;
    }

    public void SetHealth2Slider(Slider slider)
    {
        slider.maxValue = Instance.data.Health2.MaxValue;
        slider.value = Instance.data.Health2.CurrentValue;
    }

    public void SetHealth3Slider(Slider slider)
    {
        slider.maxValue = Instance.data.Health3.MaxValue;
        slider.value = Instance.data.Health3.CurrentValue;
    }

    public void SetHealth4Slider(Slider slider)
    {
        slider.maxValue = Instance.data.Health4.MaxValue;
        slider.value = Instance.data.Health4.CurrentValue;
    }

    public void SetMoneySlider(Slider slider)
    {
        slider.maxValue = Instance.data.Money.MaxValue;
        slider.value = Instance.data.Money.CurrentValue;
    }

    public void SetBallisticText(Text text)
    {
        text.text = Instance.data.BallisticAmmo.CurrentValue.ToString();
    }

    public void SetEnergyText(Text text)
    {
        text.text = Instance.data.EnergyAmmo.CurrentValue.ToString();
    }

    public void SetHealth1Text(Text text)
    {
        text.text = Instance.data.Health1.CurrentValue.ToString();
    }

    public void SetHealth2Text(Text text)
    {
        text.text = Instance.data.Health2.CurrentValue.ToString();
    }

    public void SetHealth3Text(Text text)
    {
        text.text = Instance.data.Health3.CurrentValue.ToString();
    }

    public void SetHealth4Text(Text text)
    {
        text.text = Instance.data.Health4.CurrentValue.ToString();
    }

    public void SetMoneyText(Text text)
    {
        text.text = Instance.data.Money.CurrentValue.ToString();
    }

    public void CheckBallisticChange(float delta)
    {
        Instance.data.BallisticAmmo.CheckValueChange(delta);
    }

    public void CheckEnergyChange(float delta)
    {
        Instance.data.EnergyAmmo.CheckValueChange(delta);
    }

    public void CheckHealth1Change(float delta)
    {
        Instance.data.Health1.CheckValueChange(delta);
    }

    public void CheckHealth2Change(float delta)
    {
        Instance.data.Health2.CheckValueChange(delta);
    }

    public void CheckHealth3Change(float delta)
    {
        Instance.data.Health3.CheckValueChange(delta);
    }

    public void CheckHealth4Change(float delta)
    {
        Instance.data.Health4.CheckValueChange(delta);
    }

    public void CheckMoneyChange(float delta)
    {
        Instance.data.Money.CheckValueChange(delta);
    }
}

[Serializable]
public struct RunData
{
    [SerializeField] Object energyAmmo, ballisticAmmo, health1, health2, health3, health4, money;

    public ICurrency EnergyAmmo => energyAmmo as ICurrency;

    public ICurrency BallisticAmmo => ballisticAmmo as ICurrency;

    public ICurrency Health1 => health1 as ICurrency;

    public ICurrency Health2 => health2 as ICurrency;

    public ICurrency Health3 => health3 as ICurrency;

    public ICurrency Health4 => health4 as ICurrency;

    public ICurrency Money => money as ICurrency;
}