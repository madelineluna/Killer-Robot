using UnityEngine;
using UnityEngine.AI;

// Sam Mohseni
// EnemyType: Swarm = melee chaser, Ranged = stops and shoots
// Added: enemies aggro and chase indefinitely when damaged, until they close in. Lose radius decays during engagement.

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum EnemyType { Swarm, Ranged }

    [Header("Enemy Type")]
    public EnemyType enemyType = EnemyType.Swarm;

    public enum State { Patrol, Chase, Shoot, Attack }
    protected State currentState = State.Patrol;

    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public Transform pointD;
    public float patrolSpeed = 2f;

    [Header("Detection")]
    public float detectionRadius = 10f;
    public float losePlayerRadius = 30f;

    [Header("Lose Radius Decay")]
    public float losePlayerDecayRate = 2f;
    public float losePlayerRadiusFloor = 15f;

    [Header("Chase Settings (Swarm only)")]
    public float chaseSpeed = 5f;

    [Header("Melee Attack Settings (Swarm only)")]
    public float damagePerHit = 10f;
    public float damageInterval = 1f;

    [Header("Separation Settings (Swarm only)")]
    public float separationRadius = 2f;
    public float separationForce = 3f;

    [Header("Shooting Settings (Ranged only)")]
    public Transform muzzle;
    public GameObject bulletPrefab;
    public float bulletSpeed = 18f;
    public float fireRate = 2f;
    public float bulletLifetime = 3f;
    public AudioType shootingSound;

    [Header("Animation")]
    public Animator animator;

    [Header("Debug")]
    public bool debugLogDecay = false;

    protected Transform player;
    protected NavMeshAgent navMeshAgent;
    private EnemyHealth enemyHealth;
    private int lastKnownHealth;

    private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private bool isTouchingPlayer = false;
    private float nextDamageTime = 0f;
    private float nextFireTime = 0f;
    private float currentLosePlayerRadius;

    private bool isAggro = false;

    private float nextDecayLogTime = 0f;

    protected virtual void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.avoidancePriority = Random.Range(1, 99);

        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null)
            lastKnownHealth = enemyHealth.currentHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        int count = 0;
        if (pointA != null) count++;
        if (pointB != null) count++;
        if (pointC != null) count++;
        if (pointD != null) count++;

        patrolPoints = new Transform[count];
        int i = 0;
        if (pointA != null) patrolPoints[i++] = pointA;
        if (pointB != null) patrolPoints[i++] = pointB;
        if (pointC != null) patrolPoints[i++] = pointC;
        if (pointD != null) patrolPoints[i++] = pointD;

        if (patrolPoints.Length == 0)
            Debug.LogWarning("[EnemyAI] " + gameObject.name + " has no patrol points assigned.");

        currentLosePlayerRadius = losePlayerRadius;
        EnterState(State.Patrol);
    }

    protected virtual void Update()
    {
        if (player == null) return;

        CheckForDamage();
        UpdateState();
        EvaluateTransitions();

        if (currentState == State.Chase || currentState == State.Shoot || currentState == State.Attack)
        {
            currentLosePlayerRadius = Mathf.Max(
                losePlayerRadiusFloor,
                currentLosePlayerRadius - losePlayerDecayRate * Time.deltaTime
            );

            if (debugLogDecay && Time.time >= nextDecayLogTime)
            {
                Debug.Log($"[EnemyAI] {gameObject.name} state={currentState} aggro={isAggro} loseRadius={currentLosePlayerRadius:F1}");
                nextDecayLogTime = Time.time + 1f;
            }
        }

        if (enemyType == EnemyType.Swarm)
            ApplySeparation();
    }

    void CheckForDamage()
    {
        if (enemyHealth == null) return;

        if (enemyHealth.currentHealth < lastKnownHealth)
        {
            OnDamaged();
        }

        lastKnownHealth = enemyHealth.currentHealth;
    }

    void OnDamaged()
    {
        isAggro = true;
        currentLosePlayerRadius = losePlayerRadius;

        if (enemyType == EnemyType.Swarm)
            EnterState(State.Chase);
        else if (enemyType == EnemyType.Ranged)
            EnterState(State.Shoot);
    }

    protected virtual void EnterState(State newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case State.Patrol:
                navMeshAgent.isStopped = false;
                navMeshAgent.speed = patrolSpeed;
                SetRunning(true);
                currentLosePlayerRadius = losePlayerRadius;
                break;

            case State.Chase:
                navMeshAgent.isStopped = false;
                navMeshAgent.speed = chaseSpeed;
                SetRunning(true);
                break;

            case State.Shoot:
            case State.Attack:
                navMeshAgent.isStopped = true;
                navMeshAgent.velocity = Vector3.zero;
                SetRunning(false);
                break;
        }
    }

    protected virtual void UpdateState()
    {
        switch (currentState)
        {
            case State.Patrol: DoPatrol(); break;
            case State.Chase: DoChase(); break;
            case State.Shoot: DoShoot(); break;
            case State.Attack: DoAttack(); break;
        }
    }

    protected virtual void EvaluateTransitions()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (isAggro)
        {
            if (dist <= detectionRadius)
            {
                isAggro = false;
            }
            else
            {
                if (enemyType == EnemyType.Ranged && currentState != State.Chase)
                    EnterState(State.Chase);
                else if (enemyType == EnemyType.Swarm && currentState != State.Chase)
                    EnterState(State.Chase);
                return;
            }
        }

        if (enemyType == EnemyType.Swarm)
        {
            if (isTouchingPlayer && currentState != State.Attack)
                EnterState(State.Attack);

            else if (!isTouchingPlayer)
            {
                if (dist <= detectionRadius && currentState == State.Patrol)
                    EnterState(State.Chase);

                else if (dist > currentLosePlayerRadius && currentState == State.Chase)
                    EnterState(State.Patrol);

                else if (currentState == State.Attack)
                    EnterState(dist <= currentLosePlayerRadius ? State.Chase : State.Patrol);
            }
        }
        else if (enemyType == EnemyType.Ranged)
        {
            if (dist <= detectionRadius && currentState != State.Shoot)
                EnterState(State.Shoot);

            else if (dist > currentLosePlayerRadius && currentState == State.Shoot)
                EnterState(State.Patrol);
        }
    }

    void DoPatrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);

        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.5f)
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void DoChase()
    {
        navMeshAgent.SetDestination(player.position);
    }

    void DoShoot()
    {
        FacePlayer();

        if (muzzle == null || bulletPrefab == null) return;
        if (Time.time < nextFireTime) return;

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        AudioEventManager.Instance.PlayAudio(shootingSound, transform.position);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = muzzle.forward * bulletSpeed;

        Destroy(bullet, bulletLifetime);
        nextFireTime = Time.time + fireRate;
    }

    void DoAttack()
    {
        FacePlayer();

        if (Time.time < nextDamageTime) return;

        PlayerHealth1 ph = player.GetComponentInParent<PlayerHealth1>();
        if (ph != null)
        {
            ph.TakeDamage((int)damagePerHit);
            //AudioEventManager.Instance.PlayAudio(AudioType.DogHit, transform.position);
            nextDamageTime = Time.time + damageInterval;
            Debug.Log($"[EnemyAI] {gameObject.name} dealt {damagePerHit} melee damage.");
        }
    }

    protected void FacePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    void SetRunning(bool running)
    {
        if (animator != null)
            animator.SetBool("isRunning", running);
    }

    void ApplySeparation()
    {
        Collider[] neighbors = Physics.OverlapSphere(transform.position, separationRadius);
        Vector3 pushTotal = Vector3.zero;
        int count = 0;

        foreach (Collider col in neighbors)
        {
            if (col.gameObject == gameObject) continue;
            if (!col.gameObject.CompareTag("Enemy")) continue;

            Vector3 pushDir = transform.position - col.transform.position;
            float distance = pushDir.magnitude;
            if (distance > 0)
            {
                pushTotal += pushDir.normalized / distance;
                count++;
            }
        }

        if (count > 0)
        {
            pushTotal /= count;
            navMeshAgent.velocity += pushTotal * separationForce * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isTouchingPlayer = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            isTouchingPlayer = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            isTouchingPlayer = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Static configured lose radius (faded blue)
        Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, losePlayerRadius);

        // Live decaying lose radius (bright cyan) - only visible in play mode
        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, currentLosePlayerRadius);
        }

        if (enemyType == EnemyType.Swarm)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, separationRadius);
        }

        Transform[] points = new Transform[] { pointA, pointB, pointC, pointD };
        Gizmos.color = Color.yellow;
        Transform prev = null;
        foreach (Transform p in points)
        {
            if (p == null) continue;
            Gizmos.DrawSphere(p.position, 0.2f);
            if (prev != null)
                Gizmos.DrawLine(prev.position, p.position);
            prev = p;
        }
        if (prev != null && pointA != null && prev != pointA)
            Gizmos.DrawLine(prev.position, pointA.position);
    }
}