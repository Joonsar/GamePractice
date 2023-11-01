using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public GameObject spawnPointPrefab; // Reference to the spawn point prefab
    public float spawnInterval = 10.0f; // Time interval between spawn points
    public float spawnRadius = 20.0f; // Maximum distance from the player

    private void Start()
    {
        StartCoroutine(SpawnSpawnPoints());
    }

    private IEnumerator SpawnSpawnPoints()
    {
        while (true)
        {
            // Calculate a random position within the spawn radius
            Vector3 randomPosition = player.position + Random.insideUnitSphere * spawnRadius;
            randomPosition.y = 0; // Ensure the spawn points stay on the same Y level.

            // Instantiate the spawn point prefab at the random position
            Instantiate(spawnPointPrefab, randomPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
