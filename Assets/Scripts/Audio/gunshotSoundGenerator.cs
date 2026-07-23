using UnityEngine;

public class GunshotSoundGenerator : MonoBehaviour
{
    public void playGunshot()
    {
        // Debug.Log("Bang!");
        AudioEventManager.Instance.PlayAudio(AudioType.Gunshot, transform.position);
    }
}
