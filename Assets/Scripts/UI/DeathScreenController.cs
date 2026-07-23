// Worked on by Sam Mohseni and Madeline Luna

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathScreenController : MonoBehaviour
{
    public GameObject deathScreen;
    public TextMeshProUGUI restartText;

    void Start()
    {
        // Ensures game is running at normal speed when scene starts
        Time.timeScale = 1f;
    }

    public void ShowDeathScreen()
    {
        FindFirstObjectByType<MusicManager>().PlayDeathMusic();
        StartCoroutine(ShowDeathScreenDelayed());
    }

    IEnumerator ShowDeathScreenDelayed()
    {
        // Wait for death animation to finish before showing death screen
        yield return new WaitForSeconds(2f);

        deathScreen.SetActive(true);
        restartText.text = "Press Space to Restart";
        // Pause the game
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("StartMenuScene");
    }

}
