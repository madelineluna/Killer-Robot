
using UnityEngine;
using UnityEngine.AI;

// Standalone boss. Ranged shooter, fire rate ramps faster every 10% HP lost.
// Charge phases at 75/50/25% HP: rush player, slam hit, snap back to ranged.
// Sam Mohseni

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyHealth))]
public class BossEnemyAI : MonoBehaviour
{
    private enum State { Patrol, Chase, Shoot, Attack }
    private State currentState = State.Patrol;

    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public Transform pointD;
    public float patrolSpeed = 2f;

    [Header("Detection")]
    public float detectionRadius = 15f;
    public float losePlayerRadius = 25f;

    [Header("Shooting Settings")]
    public Transform muzzle0;
    public Transform muzzle1;
    public Transform muzzle2;
    public GameObject bulletPrefab;
    public float bulletSpeed = 18f;
    public float bulletLifetime = 3f;
    public AudioType shootingSound;
    [Tooltip("Height offset above player.position to aim at (roughly chest/body height).")]
    public float aimHeightOffset = 1.0f;

    [Header("Fire Rate Ramp (index 0 = 100-91% HP, index 9 = 0-10% HP)")]
    public float[] fireRateByHpBand = new float[]
    {
        3.0f, 2.6f, 2.2f, 1.9f, 1.6f, 1.3f, 1.0f, 0.8f, 0.6f, 0.4f
    };

    [Header("Charge Phases")]
    public float chargeDuration = 8f;
    public float chargeSpeed = 10f;
    public int chargeDamagePerHit = 25;
    public float charge1Threshold = 0.75f;
    public float charge2Threshold = 0.50f;
    public float charge3Threshold = 0.25f;

    [Header("Charge Recovery")]
    public float chargeRecoveryDuration = 2f;
    public float postSlamCooldown = 1.5f;

    [Header("Animation")]
    public Animator animator;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private EnemyHealth bossHealth;
    private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private bool isTouchingPlayer = false;
    private float nextFireTime = 0f;
    private float currentFireRate = 2f;

    private bool charged75 = false;
    private bool charged50 = false;
    private bool charged25 = false;
    private bool isCharging = false;
    private float chargeEndTime = 0f;
    private bool chargeHitLanded = false;
    private float chargeRecoveryEndTime = 0f;
    private float postSlamCooldownEndTime = 0f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        bossHealth = GetComponent<EnemyHealth>();
        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.avoidancePriority = Random.Range(1, 99);

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

        currentFireRate = GetFireRateForHp(1f);
        EnterState(State.Patrol);
    }

    void Update()
    {
        if (player == null) return;
        UpdateState();
        EvaluateTransitions();
    }

    void EnterState(State newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case State.Patrol:
                navMeshAgent.isStopped = false;
                navMeshAgent.speed = patrolSpeed;
                SetRunning(true);
                break;

            case State.Chase:
                navMeshAgent.isStopped = false;
                navMeshAgent.speed = chargeSpeed;
                SetRunning(true);
                break;

            case State.Shoot:
                navMeshAgent.isStopped = true;
                navMeshAgent.velocity = Vector3.zero;
                SetRunning(false);
                break;

            case State.Attack:
                navMeshAgent.isStopped = true;
                navMeshAgent.velocity = Vector3.zero;
                SetRunning(false);

                if (isCharging && player != null)
                {
                    PlayerHealth1 ph = player.GetComponentInParent<PlayerHealth1>();
                    if (ph != null)
                    {
                        ph.TakeDamage(chargeDamagePerHit);
                        //AudioEventManager.Instance.PlayAudio(AudioType.DogHit, transform.position);
                    }
                    
                    chargeHitLanded = true;
                    postSlamCooldownEndTime = Time.time + postSlamCooldown;
                }
                break;
        }
    }

    void UpdateState()
    {
        switch (currentState)
        {
            case State.Patrol: DoPatrol(); break;
            case State.Chase: DoChase(); break;
            case State.Shoot: DoShoot(); break;
            case State.Attack: break;
        }
    }

    void EvaluateTransitions()
    {
        float hpFraction = (float)bossHealth.currentHealth / bossHealth.maxHealth;
        float dist = Vector3.Distance(transform.position, player.position);

        if (!isCharging)
            currentFireRate = GetFireRateForHp(hpFraction);

        if (!isCharging && Time.time >= chargeRecoveryEndTime)
        {
            if (!charged75 && hpFraction <= charge1Threshold) { charged75 = true; StartCharge(); return; }
            else if (!charged50 && hpFraction <= charge2Threshold) { charged50 = true; StartCharge(); return; }
            else if (!charged25 && hpFraction <= charge3Threshold) { charged25 = true; StartCharge(); return; }
        }

        if (isCharging)
        {
            if (Time.time >= chargeEndTime || chargeHitLanded) { EndCharge(); return; }

            if (isTouchingPlayer && currentState != State.Attack)
                EnterState(State.Attack);
            else if (!isTouchingPlayer && currentState != State.Chase)
                EnterState(State.Chase);

            return;
        }

        if (Time.time < postSlamCooldownEndTime)
        {
            if (currentState != State.Patrol)
                EnterState(State.Patrol);
            return;
        }

        if (dist <= detectionRadius && currentState != State.Shoot)
            EnterState(State.Shoot);
        else if (dist > losePlayerRadius && currentState == State.Shoot)
            EnterState(State.Patrol);
    }

    void StartCharge()
    {
        isCharging = true;
        chargeEndTime = Time.time + chargeDuration;
        chargeHitLanded = false;
        EnterState(State.Chase);
    }

    void EndCharge()
    {
        isCharging = false;
        chargeRecoveryEndTime = Time.time + chargeRecoveryDuration;

        if (Time.time < postSlamCooldownEndTime)
        {
            EnterState(State.Patrol);
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= detectionRadius)
            EnterState(State.Shoot);
        else
            EnterState(State.Patrol);
    }

    float GetFireRateForHp(float hpFraction)
    {
        if (fireRateByHpBand == null || fireRateByHpBand.Length == 0) return 2f;
        int band = Mathf.FloorToInt((1f - hpFraction) * fireRateByHpBand.Length);
        band = Mathf.Clamp(band, 0, fireRateByHpBand.Length - 1);
        return fireRateByHpBand[band];
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
        if (muzzle0 == null || bulletPrefab == null) return;
        if (Time.time < nextFireTime) return;

        GameObject bullet0 = Instantiate(bulletPrefab, muzzle0.position, muzzle0.rotation);
        GameObject bullet1 = Instantiate(bulletPrefab, muzzle1.position, muzzle1.rotation);
        GameObject bullet2 = Instantiate(bulletPrefab, muzzle2.position, muzzle2.rotation);
        AudioEventManager.Instance.PlayAudio(shootingSound, transform.position);

        Rigidbody rb0 = bullet0.GetComponent<Rigidbody>();
        Rigidbody rb1 = bullet1.GetComponent<Rigidbody>();
        Rigidbody rb2 = bullet2.GetComponent<Rigidbody>();
        if (rb0 != null)
        {
            // Aim directly at the player's body instead of along muzzle.forward.
            // Lets the boss shoot downward at the player from elevated positions.
            Vector3 aimTarget = player.position + Vector3.up * aimHeightOffset;
            Vector3 aimDirection = (aimTarget - muzzle0.position).normalized;
            rb0.linearVelocity = aimDirection * bulletSpeed;
            rb1.linearVelocity = aimDirection * bulletSpeed;
            rb2.linearVelocity = aimDirection * bulletSpeed;
        }



        Destroy(bullet0, bulletLifetime);
        Destroy(bullet1, bulletLifetime);
        Destroy(bullet2, bulletLifetime);
        nextFireTime = Time.time + currentFireRate;
    }

    void FacePlayer()
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, losePlayerRadius);

        Transform[] points = new Transform[] { pointA, pointB, pointC, pointD };
        Gizmos.color = Color.yellow;
        Transform prev = null;
        foreach (Transform p in points)
        {
            if (p == null) continue;
            Gizmos.DrawSphere(p.position, 0.2f);
            if (prev != null) Gizmos.DrawLine(prev.position, p.position);
            prev = p;
        }
        if (prev != null && pointA != null && prev != pointA)
            Gizmos.DrawLine(prev.position, pointA.position);
    }
}