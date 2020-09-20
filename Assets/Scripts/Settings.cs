using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public int resolutionIndex;
    public AudioMixer masterAudio;

    public void SetResolution(int index)
    {
        var res = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
    }

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void SetQuality(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    public void SetMasterVolume(float value)
    {
        masterAudio.SetFloat("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        masterAudio.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        masterAudio.SetFloat("SFXVolume", value);
    }
}
