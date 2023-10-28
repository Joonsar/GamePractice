using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the enemy prefab
    public int numberOfEnemies = 5; // Number of enemies to spawn
    public float spawnInterval = 3.0f; // Time interval between spawns
    private float nextSpawnTime;
    private int enemiesSpawned = 0;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (enemiesSpawned < numberOfEnemies && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            enemiesSpawned++;
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

}
