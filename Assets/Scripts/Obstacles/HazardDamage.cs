// Written by Madeline Luna

using UnityEngine;
using System.Collections;

public class HazardDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 20;
    public float damageCooldown = 1f;

    private bool canDamage = true;

    private void OnTriggerStay(Collider other)
    {
        if (!canDamage) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth1 playerHealth = other.GetComponentInParent<PlayerHealth1>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                StartCoroutine(DamageCooldownRoutine());
            }
        }
    }

    private IEnumerator DamageCooldownRoutine()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}