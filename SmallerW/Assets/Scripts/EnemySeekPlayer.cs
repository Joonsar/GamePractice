using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySeekPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    private NavMeshAgent navMeshAgent;
    public float distanceToPlayer = 2.0f; // Adjust this to control the distance in front of the player

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            player = GameObject.FindWithTag("player").transform;
        }
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
}

