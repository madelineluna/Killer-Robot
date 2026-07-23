// Written by Madeline Luna

using UnityEngine;

public class TipsToggle : MonoBehaviour
{
    public GameObject tipsPanel;
    public KeyCode toggleKey = KeyCode.T;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            tipsPanel.SetActive(!tipsPanel.activeSelf);
        }
    }
}