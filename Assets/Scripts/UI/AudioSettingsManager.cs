// Written by Madeline Luna

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager Instance;

    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Optional UI Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadVolumes();
        ApplySavedVolumes();
    }

    public void SetMasterVolume(float value)
    {
        // Debug.Log("Master volume changed to: " + value);
        value = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    public void LoadVolumes()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (masterSlider != null) masterSlider.value = master;
        if (musicSlider != null) musicSlider.value = music;
        if (sfxSlider != null) sfxSlider.value = sfx;
    }

    public void ApplySavedVolumes()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(master) * 20f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(music) * 20f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfx) * 20f);
    }
}