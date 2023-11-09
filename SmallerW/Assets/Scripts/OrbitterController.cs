using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitterController : MonoBehaviour
{
    public float orbitSpeed = 10f;
    public float orbitRadius = 5f;
    public int damage = 50;
    public float orbitterLifetime = 10f;

    private Vector3 initialPosition;
    private PlayerXPManager playerXPManager;
    private Transform target;
    private Renderer orbitterRenderer;

    void Start()
    {
        FindPlayer(); // Find the player GameObject

        if (target == null)
        {
            Debug.LogError("Target (player) is not found!");
            enabled = false; // Disable the script if the target is not found.
        }

        initialPosition = transform.position;

        orbitterRenderer = GetComponent<Renderer>(); // Get the rendering component

        StartCoroutine(StartLifetimeCountdown());
    }

    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        // Calculate the new position based on orbiting.
        Vector3 orbitPosition = initialPosition;
        orbitPosition.x = target.position.x + Mathf.Cos(Time.time * orbitSpeed) * orbitRadius;
        orbitPosition.z = target.position.z + Mathf.Sin(Time.time * orbitSpeed) * orbitRadius;
        transform.position = orbitPosition;

        // Make the satellite always look at the target (optional).
        transform.LookAt(target);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the enemy's health script
            HealthManager enemyHealth = collision.gameObject.GetComponent<HealthManager>();

            if (enemyHealth != null)
            {
                // Deal damage to the enemy
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    IEnumerator StartLifetimeCountdown()
    {
        while (true)
        {
            // Wait for the orbitterLifetime duration
            yield return new WaitForSeconds(orbitterLifetime);

            // Disable the rendering component to hide the object
            orbitterRenderer.enabled = false;

            // Wait for 10 seconds before reactivating
            yield return new WaitForSeconds(10f);

            // Reactivate the rendering component to make the object visible
            orbitterRenderer.enabled = true;

            // Move to initial position
            transform.position = initialPosition;
        }
    }
}
