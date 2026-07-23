using UnityEngine;
public class Projectile : MonoBehaviour
{
    public int damage = 10;

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Bullet hit: " + collision.gameObject.name);

        PlayerHealth1 playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth1>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            // Debug.Log("Damage applied!");
            AudioEventManager.Instance.PlayAudio(AudioType.LaserHit, transform.position);
        }
        Destroy(gameObject);
    }
}
