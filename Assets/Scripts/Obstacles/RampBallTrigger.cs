// Written by Madeline Luna

using UnityEngine;

public class RampBallTrigger : MonoBehaviour
{
    public RollingBallSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawner.StartSpawning();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawner.StopSpawning();
        }
    }

}