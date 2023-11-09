using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float laserSpeed = 10.0f;
    public int damage = 50;
    public float laserLifetime = 2.0f;
    public float fireRate = 6.0f; // Initial fire rate
    private float lifeTimer;
    private PlayerXPManager playerXPManager;
    private Vector3 initialVelocity;

    // Initialize the bullet's parameters
    public void InitializeBullet(float speed, int bulletDamage)
    {
        laserSpeed = speed;
        damage = bulletDamage;
    }


    void Start()
    {
        lifeTimer = laserLifetime;
        playerXPManager = FindObjectOfType<PlayerXPManager>(); // Reference to the PlayerXPManager script
    }

    void Update()
    {
        // Move the bullet forward using the initial velocity
        transform.Translate(initialVelocity * Time.deltaTime);
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0.0f)
        {
            DestroyBullet();
        }
    }

    public void SetInitialVelocity(Vector3 direction)
    {
        initialVelocity = direction.normalized * laserSpeed;
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

            // Destroy the bullet when it hits an enemy
            DestroyBullet();
        }
        else
        {
            // If the laser hits something other than an enemy, destroy the bullet (e.g., it could be a wall)
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        // You can return the bullet to an object pool here if you implement object pooling
        Destroy(gameObject);
    }
}
