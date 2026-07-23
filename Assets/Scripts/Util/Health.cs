// Worked on by:
// Josh Newsome

using UnityEngine;

public class Health : MonoBehaviour
{
    public bool isDead = false;
    public float health = 100f;

    public void TakeDamage(float damage) {
        health -= damage;
        
        if (health <= 0f) {
            isDead = true;
        }
    }

    public void DestroyObject() {
        Destroy(gameObject);
    }
}
