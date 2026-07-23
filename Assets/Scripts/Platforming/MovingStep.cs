// Written by Madeline Luna

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingStep : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public bool moveSideToSide = true;
    public float sideDistance = 2f;
    public float sideSpeed = 2f;

    [Header("Vertical Movement")]
    public bool moveUpAndDown = false;
    public float verticalDistance = 1f;
    public float verticalSpeed = 2f;

    [Header("Direction Axes")]
    public bool useXForSideMovement = true;

    [Header("Timing")]
    public float startOffset = 0f;

    private Vector3 startPosition;
    private Vector3 previousPosition;
    private Rigidbody rb;
    private float elapsed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        startPosition = rb.position;
        previousPosition = rb.position;
        elapsed = startOffset;
    }

    void FixedUpdate()
    {
        elapsed += Time.fixedDeltaTime;

        float sideOffset = 0f;
        float verticalOffset = 0f;

        if (moveSideToSide)
        {
            sideOffset = Mathf.Sin(elapsed * sideSpeed) * sideDistance;
        }

        if (moveUpAndDown)
        {
            verticalOffset = Mathf.Sin(elapsed * verticalSpeed) * verticalDistance;
        }

        Vector3 targetPosition = startPosition;

        if (useXForSideMovement)
        {
            targetPosition.x += sideOffset;
        }
        else
        {
            targetPosition.z += sideOffset;
        }

        targetPosition.y += verticalOffset;

        previousPosition = rb.position;
        rb.MovePosition(targetPosition);
    }

    public Vector3 GetPlatformVelocity()
    {
        return (rb.position - previousPosition) / Time.fixedDeltaTime;
    }

}