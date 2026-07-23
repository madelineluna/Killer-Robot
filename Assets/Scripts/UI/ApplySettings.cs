// Written by Madeline Luna

using UnityEngine;

public class ApplySettings : MonoBehaviour
{
    public AudioSettingsManager audioSettings;
    public ControlsSettingsManager controlsSettings;
    public DisplaySettingsManager displaySettings;

    public void ApplyAllSettings()
    {
        controlsSettings.SaveControlsSettings();
        displaySettings.SaveDisplaySettings();

        PlayerPrefs.Save();
        // Debug.Log("Settings applied.");
    }
}