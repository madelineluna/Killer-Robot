// Written by Madeline Luna

using UnityEngine;

public class RulesPanelManager : MonoBehaviour
{
    [Header("Tab Panels")]
    public GameObject rulesTab;
    public GameObject controlsTab;

    public void ShowRulesTab()
    {
        rulesTab.SetActive(true);
        controlsTab.SetActive(false);
    }

    public void ShoqControlsTab()
    {
        controlsTab.SetActive(true);
        rulesTab.SetActive(false);
    }
}
