using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
 public GameObject enemyPrefab; // Reference to the enemy prefab
    public int numberOfEnemies = 5; // Number of enemies to spawn
    public float spawnInterval = 3.0f; // Time interval between spawns
    public Transform[] spawnPoints; // Array of spawn points for enemies
    public float enemySpeed = 5.0f; // Speed of spawned enemies

    public float timeBeforeNextSpawn = 6.0f; // Time before spawning from the next point
    private int enemiesSpawned = 0;
    private int currentSpawnPointIndex = 0;
    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
        StartCoroutine(SpawnFromNextPointWithDelay());
    }

    void Update()
    {
        if (enemiesSpawned < numberOfEnemies && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            enemiesSpawned++;
            nextSpawnTime = Time.time + spawnInterval;
        }

        // Check if all enemies have been spawned from the current spawn point
        if (enemiesSpawned >= numberOfEnemies && currentSpawnPointIndex < spawnPoints.Length)
        {
            // Move to the next spawn point and reset the counter
            currentSpawnPointIndex++;
            enemiesSpawned = 0;

            if (currentSpawnPointIndex < spawnPoints.Length)
            {
                StartCoroutine(SpawnFromNextPointWithDelay());
            }
        }
    }

    void SpawnEnemy()
    {
        if (currentSpawnPointIndex < spawnPoints.Length)
        {
            // Spawn an enemy at the current spawn point with the specified speed
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[currentSpawnPointIndex].position, Quaternion.identity);
            // Set the enemy's speed
            EnemySeekPlayer enemyMovement = enemy.GetComponent<EnemySeekPlayer>();
            if (enemyMovement != null)
            {
                enemyMovement.speed = enemySpeed;
            }
        }
    }

    IEnumerator SpawnFromNextPointWithDelay()
    {
        yield return new WaitForSeconds(timeBeforeNextSpawn);
        StartCoroutine(SpawnEnemiesWithDelay(currentSpawnPointIndex));
    }

    IEnumerator SpawnEnemiesWithDelay(int spawnPointIndex)
    {
        while (spawnPointIndex < spawnPoints.Length)
        {
            if (enemiesSpawned < numberOfEnemies)
            {
                SpawnEnemy();
                enemiesSpawned++;
            }
            else
            {
                // If all enemies are spawned from the current point, wait for 6 seconds and then move to the next spawn point
                yield return new WaitForSeconds(6.0f);
                spawnPointIndex++;
                enemiesSpawned = 0;
            }

            // Wait for the spawn interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
