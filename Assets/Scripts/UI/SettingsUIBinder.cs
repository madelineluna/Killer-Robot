// Written by Madeline Luna

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUIBinder : MonoBehaviour
{
    [Header("Scene UI Manager")]
    public StartMenuController startMenuController;

    [Header("Settings Menu UI Panels")]
    public GameObject settingsPanel;
    public GameObject audioPanel;
    public GameObject controlsPanel;
    public GameObject displayPanel;

    [Header("Audio UI")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Controls UI")]
    public Slider sensitivitySlider;
    public Toggle invertYToggle;

    [Header("Display UI")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;

    private PersistentSettings persistentSettings;

    private void Start()
    {
        persistentSettings = PersistentSettings.Instance;

        if (persistentSettings == null)
        {
            // Debug.LogWarning("SettingsUIBinder: PersistentSettings.Instance is null.");
            return;
        }

        if (startMenuController == null)
        {
            startMenuController = FindFirstObjectByType<StartMenuController>();
        }

        BindSettingsMenuUI();
        BindAudio();
        BindControls();
        BindDisplay();
        BindApplySettings();
    }

    void BindSettingsMenuUI()
    {
        if (startMenuController == null) return;

        SettingsMenuUI menuUI = startMenuController.GetComponent<SettingsMenuUI>();
        if (menuUI == null) return;

        menuUI.settingsPanel = settingsPanel;
        menuUI.audioPanel = audioPanel;
        menuUI.controlsPanel = controlsPanel;
        menuUI.displayPanel = displayPanel;
    }

    void BindAudio()
    {
        AudioSettingsManager audioManager = persistentSettings.GetComponent<AudioSettingsManager>();
        if (audioManager == null) return;

        audioManager.masterSlider = masterSlider;
        audioManager.musicSlider = musicSlider;
        audioManager.sfxSlider = sfxSlider;

        if (masterSlider != null)
        {
            masterSlider.onValueChanged.RemoveAllListeners();
            masterSlider.onValueChanged.AddListener(audioManager.SetMasterVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.AddListener(audioManager.SetSFXVolume);
        }

        audioManager.LoadVolumes();
        audioManager.ApplySavedVolumes();
    }

    void BindControls()
    {
        ControlsSettingsManager controlsManager = persistentSettings.GetComponent<ControlsSettingsManager>();
        if (controlsManager == null) return;

        controlsManager.sensitivitySlider = sensitivitySlider;
        controlsManager.invertYToggle = invertYToggle;

        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.RemoveAllListeners();
            sensitivitySlider.onValueChanged.AddListener(controlsManager.SetSensitivity);
        }

        if (invertYToggle != null)
        {
            invertYToggle.onValueChanged.RemoveAllListeners();
            invertYToggle.onValueChanged.AddListener(controlsManager.SetInvertY);
        }

        controlsManager.LoadControlsSettings();
    }

    void BindDisplay()
    {
        DisplaySettingsManager displayManager = persistentSettings.GetComponent<DisplaySettingsManager>();
        if (displayManager == null) return;

        displayManager.resolutionDropdown = resolutionDropdown;
        displayManager.qualityDropdown = qualityDropdown;
        displayManager.fullscreenToggle = fullscreenToggle;

        if (resolutionDropdown != null)
        {
            resolutionDropdown.onValueChanged.RemoveAllListeners();
            resolutionDropdown.onValueChanged.AddListener(displayManager.SetResolution);
        }

        if (qualityDropdown != null)
        {
            qualityDropdown.onValueChanged.RemoveAllListeners();
            qualityDropdown.onValueChanged.AddListener(displayManager.SetQuality);
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.RemoveAllListeners();
            fullscreenToggle.onValueChanged.AddListener(displayManager.SetFullscreen);
        }

        displayManager.RefreshUIAfterBinding();
    }

    void BindApplySettings()
    {
        ApplySettings applySettings = persistentSettings.GetComponent<ApplySettings>();
        if (applySettings == null) return;

        applySettings.audioSettings = persistentSettings.GetComponent<AudioSettingsManager>();
        applySettings.controlsSettings = persistentSettings.GetComponent<ControlsSettingsManager>();
        applySettings.displaySettings = persistentSettings.GetComponent<DisplaySettingsManager>();
    }
}