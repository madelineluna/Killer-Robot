using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    [Header("Health")]
    public int barrelHealth = 30;

    [Header("Explosion Settings")]
    public float explosionRadius = 5f;
    public float instantKillRadius = 1.5f;
    public int explosionDamage = 80;
    public float knockbackForce = 700f;

    [Header("References")]
    public GameObject explosionVFXPrefab;

    public LayerMask affectedLayers;

    private bool hasExploded = false;

    public void TakeDamage(int amount)
    {
        if (hasExploded) return;
        barrelHealth -= amount;
        Debug.Log("Barrel hit! Health: " + barrelHealth);
        if (barrelHealth <= 0)
            Explode();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player touched barrel — exploding!");
            Explode();
        }
    }

    void Explode()
    {
        hasExploded = true;

        Debug.Log("=== EXPLODE CALLED ===");
        Debug.Log("Barrel position: " + transform.position);

        // ---- VFX ----
        if (explosionVFXPrefab == null)
        {
            Debug.LogError("explosionVFXPrefab is NULL — not assigned in Inspector!");
        }
        else
        {
            GameObject vfx = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
            if (ps == null)
                Debug.LogError("No ParticleSystem on VFX prefab root!");
            else
                ps.Play();

            Destroy(vfx, 3f);
        }

        // ---- Overlap Sphere ----
        Collider[] hits = affectedLayers == 0
            ? Physics.OverlapSphere(transform.position, explosionRadius)
            : Physics.OverlapSphere(transform.position, explosionRadius, affectedLayers);

        Debug.Log("Total colliders hit: " + hits.Length);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);
            float damageScale = Mathf.Clamp01(1f - (distance / explosionRadius));
            int finalDamage = Mathf.RoundToInt(explosionDamage * damageScale);

            Debug.Log("Hit: " + hit.gameObject.name +
                      " | Distance: " + distance +
                      " | Damage: " + finalDamage);

            // ---- Player ----
            PlayerHealth1 playerHealth = hit.GetComponentInParent<PlayerHealth1>();
            if (playerHealth == null)
                playerHealth = hit.GetComponentInChildren<PlayerHealth1>();

            if (playerHealth != null)
            {
                if (distance <= instantKillRadius)
                {
                    int damage = Mathf.RoundToInt(playerHealth.maxHealth * 0.35f);
                    Debug.Log("Player in kill radius! Dealing 35% = " + damage);
                    playerHealth.TakeDamage(damage);
                }
                else
                {
                    float distanceFactor = 1f - Mathf.InverseLerp(instantKillRadius, explosionRadius, distance);
                    float damagePercent = Mathf.Lerp(0.05f, 0.25f, distanceFactor);
                    int damage = Mathf.RoundToInt(playerHealth.maxHealth * damagePercent);
                    Debug.Log("Player at distance " + distance + " | " + (damagePercent * 100f) + "% = " + damage);
                    playerHealth.TakeDamage(damage);
                }

                // Knockback
                Rigidbody rb = hit.GetComponentInParent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 dir = (hit.transform.position - transform.position).normalized;
                    rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
                }
            }

            // ---- Enemies ----
            if (hit.CompareTag("Enemy") || hit.GetComponentInParent<EnemyHealth>() != null)
            {
                EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();
                if (enemyHealth == null)
                    enemyHealth = hit.GetComponentInChildren<EnemyHealth>();

                if (enemyHealth != null)
                {
                    Debug.Log("Dealing " + finalDamage + " to enemy: " + hit.gameObject.name);
                    enemyHealth.TakeDamage(finalDamage);
                }
            }

            // ---- Chain Barrels ----
            ExplodingBarrel otherBarrel = hit.GetComponent<ExplodingBarrel>();
            if (otherBarrel != null && otherBarrel != this)
                otherBarrel.TakeDamage(99999);

        } 

        // ---- Hide and destroy 
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        Destroy(gameObject, 0.15f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, instantKillRadius);
    }
}