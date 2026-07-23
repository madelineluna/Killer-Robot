// Worked on by
// Sam Mohseni
// Audrey Brainerd
// Josh Newsome
// Madeline Luna

using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour {

    private GameObject player;
    public Transform enemy;
    private NavMeshAgent navMeshAgent;

    [Header("Attack Settings")]
    public int touchDamage = 5;
    public float damageCooldown = 1f;

    private bool isStopped = false;
    private float lastDamageTime = -999f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindAnyObjectByType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update() {
        
        if (player != null && !isStopped) {    
            navMeshAgent.SetDestination(player.transform.position);
        }

        // Face player while enemy stops
        if (isStopped && player != null)
        {
            transform.LookAt(player.transform);
        }

        // Handle death
        if (GetComponent<EnemyHealth>().currentHealth <= 0f) {
            HandleDeath();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Prevent rapid repeated damage
        if (Time.time < lastDamageTime + damageCooldown)
            return;

        PlayerHealth1 playerHealth = collision.gameObject.GetComponent<PlayerHealth1>();

        // In case PlayerHealth is on child/parent
        if (playerHealth == null)
        {
            playerHealth = collision.gameObject.GetComponentInChildren<PlayerHealth1>();
        }
        if (playerHealth == null)
        {
            playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth1>();
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(touchDamage);
            AudioEventManager.Instance.PlayAudio(AudioType.RobotHit, transform.position);
            lastDamageTime = Time.time;

            if (!isStopped)
            {
                StartCoroutine(PauseAfterHit());
            }
        }
    }

    private IEnumerator PauseAfterHit()
    {
        isStopped = true;

        // Stop movement
        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(1f);

        // Resume movement
        navMeshAgent.isStopped = false;

        isStopped = false;
    }

    private void HandleDeath() {
        // to-do implement death animation handling and audio event (this is implemented only for boss at this time)
        //GetComponent<Health>().DestroyObject();
        return;
    }
}
