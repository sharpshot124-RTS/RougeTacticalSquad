using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public BoolUnityEvent OnPause;
    bool paused = false;

    public void Pause()
    {
        paused = !paused;

        Time.timeScale = paused ? 0 : 1;

        OnPause.Invoke(paused);
    }

}
