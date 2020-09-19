using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
        ObjLoadGenerated(lastLevel as UnityEngine.Object);
    }

    public void ObjLoadGenerated(UnityEngine.Object level)
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
}
