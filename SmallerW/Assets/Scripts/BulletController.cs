using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 10.0f;
    public int damage = 50;
    public float bulletLifetime = 2.0f; // Adjust this to set the bullet's lifetime

    private float lifeTimer;

    void Start()
    {
        lifeTimer = bulletLifetime;

        // Set the bullet's rotation to match the player's forward direction
        Transform playerTransform = GameObject.FindWithTag("player").transform; // Assuming "Player" tag is used for the player GameObject
        transform.rotation = playerTransform.rotation;

        // Ensure the bullet moves straight by resetting its position to match the player's position
        transform.position = playerTransform.position;
    }

    void Update()
    {
        // Move the bullet forward in the direction of the bullet's own local forward vector
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);

        // Decrease the timer
        lifeTimer -= Time.deltaTime;

        // Check if the bullet's lifetime has expired
        if (lifeTimer <= 0.0f)
        {
            Destroy(gameObject); // Destroy the bullet when its lifetime is up
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is not the player or an enemy
        if (!collision.gameObject.CompareTag("player") && !collision.gameObject.CompareTag("Enemy"))
        {
            // If it's not the player or an enemy, destroy the bullet (e.g., it could be a wall)
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the enemy's health script
            HealthManager enemyHealth = collision.gameObject.GetComponent<HealthManager>();

            if (enemyHealth != null)
            {
                // Deal damage to the enemy
                enemyHealth.TakeDamage(damage);
            }

            // Destroy the bullet when it hits an enemy
            Destroy(gameObject);
        }
    }
}
