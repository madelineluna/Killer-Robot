// Written by Madeline Luna

using UnityEngine;

public class CrushingPistonController : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform topPoint;
    public Transform bottomPoint;

    [Header("Movement Settings")]
    public float moveDownSpeed = 8f;
    public float moveUpSpeed = 3f;
    public float waitAtTopTime = 1.5f;
    public float waitAtBottomTime = 0.4f;
    public float stopDistance = 0.02f;

    private enum PistonState
    {
        WaitingAtTop,
        MovingDown,
        WaitingAtBottom,
        MovingUp
    }

    private PistonState currentState;
    private float waitTimer = 0f;

    void Start()
    {
        if (topPoint == null || bottomPoint == null)
        {
            // Debug.LogError("CrushingPistonController: TopPoint and BottomPoint not assigned in the Inspector.");
            enabled = false;
            return;
        }

        transform.position = topPoint.position;
        currentState = PistonState.WaitingAtTop;
    }

    void Update()
    {
        switch (currentState)
        {
            case PistonState.WaitingAtTop:
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitAtTopTime)
                {
                    waitTimer = 0f;
                    currentState = PistonState.MovingDown;
                }
                break;

            case PistonState.MovingDown:
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    bottomPoint.position,
                    moveDownSpeed * Time.deltaTime
                );

                if (Vector3.Distance(transform.position, bottomPoint.position) <= stopDistance)
                {
                    transform.position = bottomPoint.position;
                    currentState = PistonState.WaitingAtBottom;
                }
                break;

            case PistonState.WaitingAtBottom:
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitAtBottomTime)
                {
                    waitTimer = 0f;
                    currentState = PistonState.MovingUp;
                }
                break;

            case PistonState.MovingUp:
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    topPoint.position,
                    moveUpSpeed * Time.deltaTime
                );

                if (Vector3.Distance(transform.position, topPoint.position) <= stopDistance)
                {
                    transform.position = topPoint.position;
                    currentState = PistonState.WaitingAtTop;
                    // To-Do : Fix audio timing / distance falloff
                    // AudioEventManager.Instance.PlayAudio(AudioType.PistonTrap, transform.position); 
                }
                break;
        }
    }
}