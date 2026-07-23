// Written by Madeline Luna

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth1 playerHealth;
    public TextMeshProUGUI healthText;

    [Header("Health Bar")]
    public Image healthFillImage;

    [Header("Damage Flash")]
    public Image damageFlashImage;
    public float flashAlpha = 0.35f;
    public float flashFadeSpeed = 5f;

    [Header("Heal Flash")]
    public Image healFlashImage;
    public float healFlashAlpha = 0.35f;
    public float healFadeSpeed = 5f;

    private int lastHealth;

    private float targetFillAmount = 1f;

    void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthDisplay;
            UpdateHealthDisplay(playerHealth.currentHealth);
        }

        if (damageFlashImage != null)
        {
            Color c = damageFlashImage.color;
            c.a = 0f;
            damageFlashImage.color = c;
        }

        lastHealth = playerHealth.currentHealth;

        if (healFlashImage != null)
        {
            Color c = healFlashImage.color;
            c.a = 0f;
            healFlashImage.color = c;
        }
    }

    void Update()
    {
        // Smooth health bar animation
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = Mathf.Lerp(
                healthFillImage.fillAmount,
                targetFillAmount,
                Time.deltaTime * 8f
            );
        }

        // Fade out damage flash
        if (damageFlashImage != null)
        {
            Color c = damageFlashImage.color;
            c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * flashFadeSpeed);
            damageFlashImage.color = c;
        }

        // Fade out heal flash
        if (healFlashImage != null)
        {
            Color c = healFlashImage.color;
            c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * healFadeSpeed);
            healFlashImage.color = c;
        }
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthDisplay;
        }
    }

    void UpdateHealthDisplay(int current)
    {
        if (healthText != null)
        {
            healthText.text = "" + current;
        }
    
        if (playerHealth != null && healthFillImage != null)
        {
            targetFillAmount = (float)current / playerHealth.maxHealth;
        }
    
        // Detect change type
        if (current < lastHealth)
        {
            // Damage red flash
            if (damageFlashImage != null)
            {
                Color c = damageFlashImage.color;
                c.a = flashAlpha;
                damageFlashImage.color = c;
            }
        }
        else if (current > lastHealth)
        {
            // Heal green flash
            if (healFlashImage != null)
            {
                Color c = healFlashImage.color;
                c.a = healFlashAlpha;
                healFlashImage.color = c;
            }
        }
    
        lastHealth = current;
    }
}