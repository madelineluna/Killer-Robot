// Worked on by:
// Josh Newsome

using UnityEngine;

public class MeleeWeaponScript : MonoBehaviour
{
    private Transform meleePoint;
    private float hitRange = 1.5f;
    private int meleeDamage = 35;
    private LayerMask enemyLayer;

    private bool canHit;
    private bool hasHitThisSwing;

    public void EnableHitbox()
    {
        canHit = true;
        hasHitThisSwing = false;
        TryDamageEnemies();
    }

    public void DisableHitbox()
    {
        canHit = false;
        hasHitThisSwing = false;
    }

    void Update()
    {
        if (!canHit)
        {
            return;
        }

        TryDamageEnemies();
    }

    private void TryDamageEnemies()
    {
        if (hasHitThisSwing)
        {
            return;
        }

        Transform hitPoint = meleePoint != null ? meleePoint : transform;
        Collider[] hits = enemyLayer.value == 0
            ? Physics.OverlapSphere(hitPoint.position, hitRange, Physics.AllLayers, QueryTriggerInteraction.Collide)
            : Physics.OverlapSphere(hitPoint.position, hitRange, enemyLayer, QueryTriggerInteraction.Collide);

        if (hits.Length == 0 && enemyLayer.value != 0)
        {
            hits = Physics.OverlapSphere(hitPoint.position, hitRange, Physics.AllLayers, QueryTriggerInteraction.Collide);
        }

        foreach (Collider hit in hits)
        {
            EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();
            if (enemyHealth == null)
            {
                continue;
            }

            enemyHealth.TakeDamage(meleeDamage);
            hasHitThisSwing = true;
            canHit = false;
            break;
        }
    }
}
