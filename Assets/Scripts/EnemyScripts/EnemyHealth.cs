// Worked on by:
// Sam Mohseni
// Audrey Brainerd

using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject pickupPrefab;
    public GameObject healthPickup;
    public AudioType deathSound = AudioType.GenericDeath;
    private Animator anim;
    // Set canDropItem to false if you do not want this enemy to drop a pickup item
    public bool canDropItem = true;
    // percent chance of dropping a pickup item
    public float dropRate = .33f;
    public GameObject dyingBoss;

    IEnumerator Start()
    {
        currentHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
        if (anim == null)
            yield break;

        anim.enabled = false;
        // disable animator for a short delay in order to desync idle / walking animations
        yield return new WaitForSeconds(Random.Range(0f, .3f));
        anim.enabled = true;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // Debug.Log(gameObject.name + " took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {

        // Debug.Log(gameObject.name + " died!");
        Vector3 spawnPosition = transform.position;
        RaycastHit hit;
        int groundLayer = LayerMask.GetMask("Ground");
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 5f, groundLayer)){
            spawnPosition = hit.point + Vector3.up * 1;
        }
        if (pickupPrefab != null){
            Instantiate(pickupPrefab, spawnPosition, Quaternion.identity);
            // Debug.Log("Pickup instantiated at " + transform.position);
        } else
        {
            {
            if (Random.value < dropRate)
                // to-do change this to allow other drop types, if implemented
                Instantiate(healthPickup, spawnPosition,Quaternion.identity);
                // Debug.Log("Pickup instantiated at " + transform.position);
            }
        }
        AudioEventManager.Instance.PlayAudio(deathSound, transform.position);
        if (gameObject.name.Contains("Boss")) {
            Vector3 bossPosition = gameObject.transform.position;
            Quaternion bossRotation = gameObject.transform.rotation;
            GameObject dying = Instantiate(dyingBoss, bossPosition, bossRotation);
            BossController bossController = GetComponent<BossController>();
            bossController.TriggerWin();
            Animation anim = dying.GetComponentInChildren<Animation>();
            anim.Play("BossDeath");
            Destroy(dying, anim.clip.length);
            Destroy(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
