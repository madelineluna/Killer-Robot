// Written by Madeline Luna

using UnityEngine;

public class LaserToggle : MonoBehaviour
{
    [Header("References")]
    public GameObject laserBeam;
    public Collider laserTrigger;

    [Header("Timing Settings")]
    public float onTime = 2f;
    public float offTime = 1f;
    public bool startOn = true;

    private bool isOn;
    private float timer;

    void Start()
    {
        isOn = startOn;
        UpdateLaserState();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isOn && timer >= onTime)
        {
            isOn = false;
            timer = 0f;
            UpdateLaserState();
        }
        else if (!isOn && timer >= offTime)
        {
            isOn = true;
            timer = 0f;
            UpdateLaserState();
        }
    }

    void UpdateLaserState()
    {
        if (laserBeam != null)
            laserBeam.SetActive(isOn);

        if (laserTrigger != null)
            laserTrigger.enabled = isOn;
    }
}