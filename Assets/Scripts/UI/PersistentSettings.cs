// Written by Madeline Luna

using UnityEngine;

public class PersistentSettings : MonoBehaviour
{
    public static PersistentSettings Instance;

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
        }
    }
}