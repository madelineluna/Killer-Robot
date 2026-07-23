using UnityEngine;

public class JumpSoundGenerator : MonoBehaviour
{
    public void playJump()
    {
        AudioEventManager.Instance.PlayAudio(AudioType.Jump, transform.position);
    }
}
