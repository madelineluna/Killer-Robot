// Worked on by Audrey Brainerd
// Edited by Madeline Luna to inlcude SFX source

using UnityEngine;

public enum AudioType
{
    Gunshot,
    Jump,
    DogShoot,
    Pickup,
    LaserHit,
    PistonTrap,
    GenericDeath,
    BossDeath,
    RobotHit,
    DogHit
}

public class AudioEventManager : MonoBehaviour
{
    // define all our audio clips here, and then assign in Inspector

    [Header("Shared SFX Source")]
    public AudioSource sfxSource;

    [Header("Gunshot")]
    public AudioClip[] gunshotAudio = null;
    public float gunshotAudioVolume;

    [Header("Jump")]
    public AudioClip jumpAudio;
    public float jumpAudioVolume;

    [Header("Robo Dog Shooting")]
    public AudioClip dogShootAudio;
    public float dogShootAudioVolume;

    [Header("Get Pickup")]
    public AudioClip pickupAudio;
    public float pickupAudioVolume;

    [Header("Laser Hit")]
    public AudioClip laserHitAudio;
    public float laserHitAudioVolume;

    [Header("Piston Trap")]
    public AudioClip pistonTrapAudio;
    public float pistonTrapAudioVolume;

    [Header("Generic Death")]
    public AudioClip genericDeathAudio;
    public float genericDeathAudioVolume;

    [Header("Boss Death")]
    public AudioClip bossDeathAudio;
    public float bossDeathAudioVolume;

    [Header("Robot Hit")]
    public AudioClip robotHitAudio;
    public float robotHitAudioVolume;

    [Header("Dog Hit")]
    public AudioClip dogHitAudio;
    public float dogHitAudioVolume;

    private int gunshotIndex = 0;

    public static AudioEventManager Instance;

    public void PlayAudio(AudioType audio, Vector3 position, float magnitude = 0)
    {
        switch (audio)
        {
            case AudioType.Gunshot:
                PlayGunshot();
            break;

            case AudioType.Jump:
                PlayClip(jumpAudio, jumpAudioVolume);
            break;

            case AudioType.DogShoot:
                PlayClip(dogShootAudio, dogShootAudioVolume);
            break;

            case AudioType.Pickup:
                PlayClip(pickupAudio, pickupAudioVolume);
                break;

            case AudioType.LaserHit:
                PlayClip(laserHitAudio, laserHitAudioVolume);
            break;

            case AudioType.PistonTrap:
                PlayClip(pistonTrapAudio, pistonTrapAudioVolume);
            break;

            case AudioType.GenericDeath:
                PlayClip(genericDeathAudio, genericDeathAudioVolume);
            break;

            case AudioType.BossDeath:
                PlayClip(bossDeathAudio, bossDeathAudioVolume);
            break;

            case AudioType.RobotHit:
                PlayClip(robotHitAudio, robotHitAudioVolume);
            break;

            case AudioType.DogHit:
                PlayClip(dogHitAudio, dogHitAudioVolume);
            break;
        }
    }

    private void PlayClip(AudioClip clip, float volume)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    private void PlayGunshot()
    {
        if (sfxSource == null || gunshotAudio == null || gunshotAudio.Length == 0) return;

        sfxSource.PlayOneShot(gunshotAudio[gunshotIndex], gunshotAudioVolume);

        int lastIndex = gunshotIndex;
        gunshotIndex = Random.Range(0, gunshotAudio.Length);

        if (gunshotIndex == lastIndex && gunshotAudio.Length > 1)
        {
            gunshotIndex = Random.Range(0, gunshotAudio.Length);
        }
    }

    void Awake()
    {
        Instance = this;
    }

}

