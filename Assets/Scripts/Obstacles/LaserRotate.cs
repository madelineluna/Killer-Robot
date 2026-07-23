// Written by Madeline Luna

using UnityEngine;

public class LaserRotate : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 45f; // degrees per second
    public bool rotateX = true;
    public bool rotateY = false;
    public bool rotateZ = false;

    void Update()
    {
        float x = rotateX ? rotationSpeed * Time.deltaTime : 0f;
        float y = rotateY ? rotationSpeed * Time.deltaTime : 0f;
        float z = rotateZ ? rotationSpeed * Time.deltaTime : 0f;

        transform.Rotate(x, y, z);
    }
}