using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the enemy prefab
    public int numberOfEnemies = 5; // Number of enemies to spawn
    public float spawnInterval = 3.0f; // Time interval between spawns
    public float timeBeforeNextSpawn = 6.0f; // Time before spawning from the next point
    private int enemiesSpawned = 0;
    private float nextSpawnTime;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform; // Assuming the player has a "Player" tag

        nextSpawnTime = Time.time + spawnInterval;
        StartCoroutine(SpawnEnemiesWithDelay());
    }

    private IEnumerator SpawnEnemiesWithDelay()
    {
        while (enemiesSpawned < numberOfEnemies)
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnEnemy();
                enemiesSpawned++;
                nextSpawnTime = Time.time + spawnInterval;
            }
            yield return null; // Wait for the next frame
        }

        yield return new WaitForSeconds(timeBeforeNextSpawn);

        // If there are more spawn points, move to the next one
        if (enemiesSpawned >= numberOfEnemies)
        {
            enemiesSpawned = 0;
        }

        // Start spawning enemies again near the player
        StartCoroutine(SpawnEnemiesWithDelay());
    }

    private void SpawnEnemy()
    {
        if (player != null)
        {
            // Define a minimum and maximum distance from the player
            float minDistance = 10.0f; // Minimum distance
            float maxDistance = 15.0f; // Maximum distance

            // Calculate a random distance within the specified range
            float randomDistance = Random.Range(minDistance, maxDistance);

            // Calculate a random angle (in radians) around the player
            float randomAngle = Random.Range(0.0f, 2 * Mathf.PI);

            // Calculate the spawn position based on the random distance and angle
            float spawnX = player.position.x + randomDistance * Mathf.Cos(randomAngle);
            float spawnZ = player.position.z + randomDistance * Mathf.Sin(randomAngle);

            // Create the spawn position Vector3
            Vector3 spawnPosition = new Vector3(spawnX, player.position.y, spawnZ);

            // Use NavMesh.SamplePosition to ensure that the spawn position is within the NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPosition, out hit, maxDistance, NavMesh.AllAreas))
            {
                // Spawn an enemy at the valid position
                Instantiate(enemyPrefab, hit.position, Quaternion.identity);
            }
        }
    }
}
