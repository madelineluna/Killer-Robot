// Written by Madeline Luna

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ElevatorController : MonoBehaviour
{
    [Header("Points")]
    public Transform bottomPoint;
    public Transform topPoint;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float stopDistance = 0.02f;

    [Header("Delay Settings")]
    public float waitAtTopTime = 2f;
    private bool playerOnPlatform = false;
    private bool waitingAtTop = false;
    private float waitTimer = 0f;

    private Vector3 currentTarget;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.position = bottomPoint.position;
        currentTarget = bottomPoint.position;
    }

    void FixedUpdate()
    {
        // If we reached the top and should wait
        if (waitingAtTop)
        {
            waitTimer += Time.fixedDeltaTime;

            if (waitTimer >= waitAtTopTime)
            {
                waitingAtTop = false;
                waitTimer = 0f;

                // After waiting, go back down
                currentTarget = bottomPoint.position;
            }

            return;
        }

        // Determine target
        if (playerOnPlatform)
        {
            currentTarget = topPoint.position;
        }

        // Move elevator
        if (Vector3.Distance(rb.position, currentTarget) > stopDistance)
        {
            Vector3 newPosition = Vector3.MoveTowards(
                rb.position,
                currentTarget,
                moveSpeed * Time.fixedDeltaTime
            );

            rb.MovePosition(newPosition);
        }
        else
        {
            rb.MovePosition(currentTarget);

            if (currentTarget == topPoint.position)
            {
                waitingAtTop = true;
            }
        }
    }

    public void SetPlayerOnPlatform(bool value)
    {
        playerOnPlatform = value;
    }
}