// Written by Madeline Luna

using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource deathMusic;
    public AudioSource winMusic;

    public void PlayDeathMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }

        if (deathMusic != null)
        {
            deathMusic.Play();
        }
    }

    public void PlayWinMusic()
    {
        if (backgroundMusic != null)
            backgroundMusic.Stop();

        if (winMusic != null)
            winMusic.Play();
    }
}
