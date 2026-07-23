// Written by Madeline Luna

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DisplaySettingsManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;
    private List<Resolution> uniqueResolutions = new List<Resolution>();

    private void Start()
    {
        SetupResolutions();
        SetupQuality();
        LoadDisplaySettings();
    }

    void SetupResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        uniqueResolutions.Clear();

        List<string> options = new List<string>();
        HashSet<string> seen = new HashSet<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            if (!seen.Contains(option))
            {
                seen.Add(option);
                uniqueResolutions.Add(resolutions[i]);
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = uniqueResolutions.Count - 1;
                }
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
    }

    void SetupQuality()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
    }

    public void SetResolution(int index)
    {
        // Debug.Log("Resolution index set to: " + index);
        Resolution res = uniqueResolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        // Debug.Log("Fullscreen set to: " + isFullscreen);
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetQuality(int index)
    {
        // Debug.Log("Quality set to: " + index);
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("QualityLevel", index);
        PlayerPrefs.Save();
    }

    public void LoadDisplaySettings()
    {
        int resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        int qualityIndex = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        resolutionIndex = Mathf.Clamp(resolutionIndex, 0, uniqueResolutions.Count - 1);

        fullscreenToggle.isOn = fullscreen;
        Screen.fullScreen = fullscreen;

        qualityDropdown.SetValueWithoutNotify(qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);

        resolutionDropdown.SetValueWithoutNotify(resolutionIndex);
        resolutionDropdown.RefreshShownValue();

        Resolution res = uniqueResolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, fullscreen);
    }

    public void SaveDisplaySettings()
    {
        PlayerPrefs.Save();
    }

    public void RefreshUIAfterBinding()
    {
        SetupResolutions();
        SetupQuality();
        LoadDisplaySettings();
    }

}