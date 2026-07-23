// Written by Madeline Luna

using UnityEngine;

public class ElevatorCarry : MonoBehaviour
{
    private ElevatorController elevator;

    private void Start()
    {
        elevator = GetComponentInParent<ElevatorController>();

        if (elevator == null)
        {
            // Debug.LogError("ElevatorCarry could not find ElevatorController in parent.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (elevator != null)
            {
                elevator.SetPlayerOnPlatform(true);
                // Debug.Log("player on platform");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (elevator != null)
            {
                elevator.SetPlayerOnPlatform(false);
                // Debug.Log("player left platform");
            }
        }
    }

}