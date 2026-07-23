// Written by Madeline Luna

using UnityEngine;

public class BallDespawnZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RollingBall"))
        {
            Destroy(other.gameObject);
        }
    }
}