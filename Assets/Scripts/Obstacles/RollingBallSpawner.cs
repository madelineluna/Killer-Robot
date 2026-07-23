// Written by Madeline Luna
// Modified by Audrey Brainerd

using System.Collections;
using UnityEngine;

public class RollingBallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;

    public GameObject player;
    public float spawnAvoidDistance;

    // Array of spawn points
    public Transform[] spawnPoints;

    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 2.5f;

    [Header("Ball Settings")]
    public float angularSpeed = 1f;
    public float linearSpeed = 1f;

    private bool isSpawning = false;
    private Coroutine spawnRoutine;
    private int lastSpawnIndex = -1;

    public void StartSpawning()
    {
        if (isSpawning) return;

        isSpawning = true;
        spawnRoutine = StartCoroutine(SpawnBalls());
    }

    public void StopSpawning()
    {
        isSpawning = false;

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    private IEnumerator SpawnBalls()
    {
        while (isSpawning)
        {
            SpawnBall();

            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
        }
        spawnRoutine = null;
    }

    void SpawnBall()
    {
        if (spawnPoints == null || spawnPoints.Length == 0 || ballPrefab == null || player == null)
            return;

        int randomIndex;
    
        do
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        }
        while (randomIndex == lastSpawnIndex && spawnPoints.Length > 1);
    
        lastSpawnIndex = randomIndex;
    
        Transform spawnPoint = spawnPoints[randomIndex];
        
        // Added by Audrey. While player is within a certain distance of spawner, keep trying for different spawn locations to avoid ball spawning on player
        
        int maxAttempts = 10;
        int attempts = 0; 

        while (Vector3.Distance(spawnPoint.position, player.transform.position) < spawnAvoidDistance && attempts < maxAttempts)              
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
            spawnPoint = spawnPoints[randomIndex];
            attempts++;
        }

        // Instantiates balls with linear and angular momentum
        
        GameObject ball = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        
        if (ballRb != null) 
        {
            ballRb.angularVelocity = -spawnPoint.right * angularSpeed;
            ballRb.linearVelocity = -spawnPoint.forward * linearSpeed;
        }
    
    }
}