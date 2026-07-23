using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // The dog
    public Vector3 offset = new Vector3(0, 5, -8);  // Camera offset
    public float smoothSpeed = 0.1f;               // Smooth movement

    void LateUpdate()
    {
        if (target == null) return;

        // Desired camera position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Look at the dog
        transform.LookAt(target);
    }
}