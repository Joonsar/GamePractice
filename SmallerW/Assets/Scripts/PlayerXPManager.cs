using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXPManager : MonoBehaviour
{
    public int xp = 0; // Current Xp
    public int level = 1; // Curent level

    public GameObject orbitterPrefab; // Reference to the Orbitter prefab
    private bool orbitterSpawned = false;

    public void AddXP(int amount)
    {
        xp += amount;
        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        if (xp >= 100)
        {
            xp -= 100;
            level++;
            // Level up rewards
            SpawnOrbitter();
        }
    }

    private void SpawnOrbitter()
    {
        if (orbitterPrefab != null && !orbitterSpawned)
        {
            GameObject orbitter = Instantiate(orbitterPrefab, transform.position, Quaternion.identity);
            orbitterSpawned = true;
        }
    }

    // Call this method when you want to allow the Orbitter to spawn again
    public void ResetOrbitterSpawned()
    {
        orbitterSpawned = false;
    }
}
