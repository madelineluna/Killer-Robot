// Written by Madeline Luna

using UnityEngine;

public class SettingsMenuUI : MonoBehaviour
{
    public GameObject settingsPanel;

    [Header("Tab Panels")]
    public GameObject audioPanel;
    public GameObject controlsPanel;
    public GameObject displayPanel;

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        ShowAudioTab();
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ShowAudioTab()
    {
        audioPanel.SetActive(true);
        controlsPanel.SetActive(false);
        displayPanel.SetActive(false);
    }

    public void ShowControlsTab()
    {
        audioPanel.SetActive(false);
        controlsPanel.SetActive(true);
        displayPanel.SetActive(false);
    }

    public void ShowDisplayTab()
    {
        audioPanel.SetActive(false);
        controlsPanel.SetActive(false);
        displayPanel.SetActive(true);
    }
}
