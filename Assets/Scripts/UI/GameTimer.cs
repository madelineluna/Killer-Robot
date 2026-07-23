// Writted by Madeline Luna

using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public PlayerController player;   // reference to player

    private float timeElapsed = 0f;

    void Update()
    {
        // Stop timer if player is dead
        if (!player.isAlive)
        {
            return;
        }

        timeElapsed += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        int milliseconds = Mathf.FloorToInt((timeElapsed * 100) % 100);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}