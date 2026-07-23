using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RangedEnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;

    [Header("Detection")]
    public float detectionRadius = 10f;

    [Header("Shooting")]
    public Transform muzzle;
    public GameObject bulletPrefab;
    public float bulletSpeed = 18f;
    public float fireRate = 2f;
    public float bulletLifeTime = 3f;

    [Header("Animation")]
    public Animator animator;

    private Transform player;
    private Transform currentTarget;
    private bool movingToB = true;
    private float nextFireTime = 0f;
    private NavMeshAgent navMeshAgent;
    private bool playerInRange = false;

    [Header("Enemy Audio")]
    public AudioType shootingSound;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = patrolSpeed;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        currentTarget = pointB;
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            playerInRange = distanceToPlayer <= detectionRadius;
        }

        if (playerInRange)
        {
            // Stop moving completely
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;

            // Face the player
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);

            // Stop running animation
            if (animator != null)
                animator.SetBool("isRunning", false);

            // Shoot
            ShootAtPlayer();
        }
        else
        {
            // Resume patrolling
            navMeshAgent.isStopped = false;

            if (animator != null)
                animator.SetBool("isRunning", true);

            Patrol();
        }
    }

    void Patrol()
    {
        if (pointA == null || pointB == null) return;

        navMeshAgent.SetDestination(currentTarget.position);

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.5f)
        {
            movingToB = !movingToB;
            currentTarget = movingToB ? pointB : pointA;
        }
    }

    void ShootAtPlayer()
    {
        if (muzzle == null || bulletPrefab == null) return;
        if (Time.time < nextFireTime) return;

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        AudioEventManager.Instance.PlayAudio(shootingSound, transform.position);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = muzzle.forward * bulletSpeed;
        }
        Destroy(bullet, bulletLifeTime);

        nextFireTime = Time.time + fireRate;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
        }
    }
}
