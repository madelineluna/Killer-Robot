// Written by Madeline Luna

using Unity.VisualScripting;
using UnityEngine;

public class ElevatorFlipTrigger : MonoBehaviour
{
    bool hasActivated = false;
    void OnTriggerEnter(Collider elevator)
    {
        // Flips elevator "home" and "destination" to use as a checkpoint
        if (elevator.gameObject.CompareTag("Elevator") & !hasActivated)
        {
            hasActivated = true;
            // Debug.Log("Elevator interacted successfully");
            ElevatorController ec = elevator.gameObject.GetComponent<ElevatorController>();
            Vector3 swap = ec.topPoint.position;
            ec.topPoint.position = ec.bottomPoint.position;
            ec.bottomPoint.position = swap;
        }
    }
}
