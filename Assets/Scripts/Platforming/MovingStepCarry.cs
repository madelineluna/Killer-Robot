// Written by Madeline Luna

using UnityEngine;

public class MovingStepCarry : MonoBehaviour
{
    private MovingStep movingStep;

    private void Start()
    {
        movingStep = GetComponentInParent<MovingStep>();

        if (movingStep == null)
        {
            // Debug.LogError("MovingStepCarry could not find MovingStep in parent.");
        }
    }

    private Transform GetPlayerRoot(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
        {
            return other.attachedRigidbody.transform;
        }

        if (other.CompareTag("Player"))
        {
            return other.transform;
        }

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform playerRoot = GetPlayerRoot(other);
        if (playerRoot == null || movingStep == null)
            return;

        playerRoot.SetParent(movingStep.transform);
        // Debug.Log("Player parented to moving step");
    }

    private void OnTriggerStay(Collider other)
    {
        Transform playerRoot = GetPlayerRoot(other);
        if (playerRoot == null || movingStep == null)
            return;

        if (playerRoot.parent != movingStep.transform)
        {
            playerRoot.SetParent(movingStep.transform);
            // Debug.Log("Player re-parented to moving step");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform playerRoot = GetPlayerRoot(other);
        if (playerRoot == null || movingStep == null)
            return;

        if (playerRoot.parent == movingStep.transform)
        {
            playerRoot.SetParent(null);
            // Debug.Log("Player unparented from moving step");
        }
    }
}