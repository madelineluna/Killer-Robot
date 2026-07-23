using UnityEngine;
using System;
public class PlayerHealth1 : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public PlayerController playerController;
    // Event for UI updates
    public event Action<int> OnHealthChanged;
    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
        playerController = GetComponentInParent<PlayerController>();
    }
    public void TakeDamage(int amount)
    {
        if (BossController.winTriggered) return;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
        // Debug.Log("Player took damage. Current health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }
    public void Die()
    {
        // Debug.Log("Player died");

        DeathScreenController deathScreen = FindFirstObjectByType<DeathScreenController>();
        if (deathScreen != null)
        {
            deathScreen.ShowDeathScreen();
        }

        playerController.Die();
    }
}