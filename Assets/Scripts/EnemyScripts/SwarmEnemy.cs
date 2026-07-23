using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SwarmEnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;

    [Header("Chase Settings")]
    public float detectionRadius = 10f;
    public float chaseSpeed = 5f;

    [Header("Attack Settings")]
    public float damagePerSecond = 1f;
    public float damageInterval = 1f;

    [Header("Animation")]
    public Animator animator;

    private Transform player;
    private Transform currentTarget;
    private bool movingToB = true;
    private bool isChasing = false;
    private bool isTouchingPlayer = false;
    private float nextDamageTime = 0f;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
         navMeshAgent = GetComponent<NavMeshAgent>();

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
        {
            // Debug.Log("NavMesh Y: " + hit.position.y);
            // Debug.Log("Agent Y: " + transform.position.y);
            // Debug.Log("Gap: " + (hit.position.y - transform.position.y));
        }
        else
        {
            // Debug.LogError("NO NAVMESH FOUND — rebake your Navigation!");
        }
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = patrolSpeed;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        currentTarget = pointB;

        // Debug.Log("RoboDog position: " + transform.position);

    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRadius)
            {
                isChasing = true;
            }

            if (isChasing)
            {
                Chase();
            }
            else
            {
                Patrol();
            }

            // Deal damage if touching player
            if (isTouchingPlayer && Time.time >= nextDamageTime)
            {
                PlayerHealth1 playerHealth = player.GetComponent<PlayerHealth1>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage((int)damagePerSecond);
                    nextDamageTime = Time.time + damageInterval;
                    // Debug.Log("Swarm enemy dealt damage!");
                }
            }
        }
    }

    void Patrol()
    {
        if (pointA == null || pointB == null) return;

        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.SetDestination(currentTarget.position);

        if (animator != null)
            animator.SetBool("isRunning", true);

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.5f)
        {
            movingToB = !movingToB;
            currentTarget = movingToB ? pointB : pointA;
        }
    }

    void Chase()
    {
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(player.position);

        if (animator != null)
            animator.SetBool("isRunning", true);
    }

    // To-Do: Determine if collision or trigger enter should be used for contact with player
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }

    // consider using trigger rather than collision to make hitbox larger than model
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
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
