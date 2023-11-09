using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySeekPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    private NavMeshAgent navMeshAgent;
    public float distanceToPlayer = 2.0f; // Adjust this to control the distance in front of the player
    public float speed = 5.0f; // Speed of the enemy
    public int damageAmount = 10;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            player = GameObject.FindWithTag("player").transform;
        }

        // Set the NavMeshAgent's speed to the specified speed
        navMeshAgent.speed = speed;
    }


    void Update()
    {
        if (player == null)
            return;

        // Calculate the destination point in front of the player
        Vector3 destination = player.position + player.forward * distanceToPlayer;

        // Set the destination of the NavMeshAgent to the calculated position
        navMeshAgent.SetDestination(destination);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            // Check if the collision is with the player
            HealthManager playerHealth = collision.gameObject.GetComponent<HealthManager>();

            if (playerHealth != null)
            {
                // Apply damage to the player
                playerHealth.TakeDamage(damageAmount);

                if (playerHealth.currentHealth <= 0)
                {
                    // Player's health is <= 0, you can handle the player's defeat here
                    playerHealth.Die();
                }
            }
        }
    }


}

