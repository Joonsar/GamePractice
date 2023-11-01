using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 10.0f;
    public int damage = 50;
    public float bulletLifetime = 2.0f;
    public float fireRate = 3.0f; // Initial fire rate
    private float lifeTimer;
    private PlayerXPManager playerXPManager;

    // Initialize the bullet's parameters
    public void InitializeBullet(float speed, int bulletDamage)
    {
        bulletSpeed = speed;
        damage = bulletDamage;
    }


    void Start()
    {
        lifeTimer = bulletLifetime;
        playerXPManager = FindObjectOfType<PlayerXPManager>(); // Reference to the PlayerXPManager script
    }

    void Update()
    {
        // Move the bullet forward using Rigidbody
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0.0f)
        {
            DestroyBullet();
        }
    }

    // Use OnCollisionEnter instead of OnTriggerEnter
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is not the player or an enemy
        if (!collision.gameObject.CompareTag("player") && !collision.gameObject.CompareTag("Enemy"))
        {
            // If it's not the player or an enemy, destroy the bullet (e.g., it could be a wall)
            DestroyBullet();
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
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        // You can return the bullet to an object pool here if you implement object pooling
        Destroy(gameObject);
    }

    public void UpdateBulletStats()
    {
        // Increase bullet speed and damage when the player levels up
        if (playerXPManager != null)
        {
            float speedIncreasePercentage = 0.05f; // 5% increase
            float damageIncreasePercentage = 0.10f; // 10% increase

            bulletSpeed *= (1.0f + speedIncreasePercentage * playerXPManager.level);
            damage = (int)(damage * (1.0f + damageIncreasePercentage * playerXPManager.level));
        }
    }
}
