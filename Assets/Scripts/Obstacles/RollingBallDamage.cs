// Written by Madeline Luna

using UnityEngine;

public class RollingBallDamage : MonoBehaviour
{
    public float damageCooldown = 1f;
    private float lastHitTime = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time < lastHitTime + damageCooldown)
                return;

            lastHitTime = Time.time;

            PlayerHealth1 player = collision.gameObject.GetComponent<PlayerHealth1>();

            if (player != null)
            {
                player.TakeDamage(10);
            }
        }
    }
}