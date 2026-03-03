using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle fullScreenToggle;

    private void Start()
    {
        RefreshSettings();
    }

    public void RefreshSettings()
    {
        if (volumeSlider != null)
            volumeSlider.value = Settings.musicVolume;
        if (fullScreenToggle != null)
            fullScreenToggle.isOn = Settings.isFullScreen;

        Apply();
    }

    public void Apply()
    {
        if (volumeSlider != null)
            Settings.musicVolume = volumeSlider.value;
        if (fullScreenToggle != null)
            Settings.isFullScreen = fullScreenToggle.isOn;

        Screen.fullScreen = Settings.isFullScreen;

        if (mixer != null)
            mixer.SetFloat("Master", Mathf.Log10(Settings.musicVolume) * 20);
    }
}

