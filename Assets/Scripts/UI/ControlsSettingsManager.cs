// Written by Madeline Luna

using UnityEngine;
using UnityEngine.UI;

public class ControlsSettingsManager : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Toggle invertYToggle;

    private void Start()
    {
        LoadControlsSettings();
    }

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
        // Debug.Log("Sensitivity set to: " + value);
    }

    public void SetInvertY(bool isInverted)
    {
        PlayerPrefs.SetInt("InvertY", isInverted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadControlsSettings()
    {
        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        bool invertY = PlayerPrefs.GetInt("InvertY", 0) == 1;

        sensitivitySlider.value = sensitivity;
        invertYToggle.isOn = invertY;
    }

    public void SaveControlsSettings()
    {
        PlayerPrefs.Save();
    }
}