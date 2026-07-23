// Written by Madeline Luna

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [Header("Scene To Load")]
    public string gameplaySceneName;

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject rulesPanel;

    // Start the game
    public void StartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    // Quit the application
    public void QuitGame()
    {
        // Debug.Log("Quitting Game");
        Application.Quit();
    }

    // Open the rules panel
    public void OpenRules()
    {
        mainMenuPanel.SetActive(false);
        rulesPanel.SetActive(true);
    }

    // Close rules and return to main menu
    public void CloseRules()
    {
        rulesPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

}