using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
    public Transform firePoint;

    public float moveSpeed = 5.0f;
    public float bulletSpeed = 5.0f;
    public float laserSpeed = 10.0f;
    public float fireRate = 3.0f; // Fire a bullet every 3 seconds
    public float laserRate = 6.0f; // Fire a laser every 6 seconds
    public float rotationSpeed = 10.0f;

    private float nextFireTime;
    private float nextLaserTime;
    private Rigidbody rb;
    private PlayerXPManager playerXPManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nextFireTime = Time.time + fireRate;
        nextLaserTime = Time.time + laserRate;
        playerXPManager = FindObjectOfType<PlayerXPManager>();
    }

    void Update()
    {
        // Player Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime;

        // Apply the movement to the Rigidbody
        rb.MovePosition(rb.position + movement);

        // Player rotation based on mouse input
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 lookAtPoint = ray.GetPoint(rayDistance);
            lookAtPoint.y = transform.position.y; // Keep the same y-position
            Quaternion targetRotation = Quaternion.LookRotation(lookAtPoint - transform.position);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime));
        }

        // Adjust the fire rate based on the player's level
        if (playerXPManager != null)
        {
            float fireRateIncreasePercentage = 0.10f; // 10% decrease in fire rate per level
            fireRate = 3.0f / (1.0f + fireRateIncreasePercentage * playerXPManager.level);
            // Adjust laser rate based on level
            float laserRateIncreasePercentage = 0.05f; // 5% decrease in laser rate per level
            laserRate = 6.0f / (1.0f + laserRateIncreasePercentage * playerXPManager.level);
        }

        // Check if it's time to shoot bullets
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        // Check if it's time to shoot lasers
        if (Time.time >= nextLaserTime && playerXPManager != null && playerXPManager.level >= 2) // Adjust the minimum level required
        {
            ShootLaser();
            nextLaserTime = Time.time + laserRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Get the BulletController script and set the initial bullet speed
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (bulletController != null)
        {
            bulletController.bulletSpeed = bulletSpeed;
        }
    }

    void ShootLaser()
    {
        // Instantiate and fire the laser
        Vector3 laserSpawnOffset = firePoint.forward * 5.0f; // Adjust the offset as needed

        // Calculate the spawn position for the laser
        Vector3 laserSpawnPosition = firePoint.position + laserSpawnOffset;

        // Instantiate and fire the laser at the calculated position
        GameObject laser = Instantiate(laserPrefab, laserSpawnPosition, firePoint.rotation);

        // Get the LaserController script and set any initial properties if needed
        LaserController laserController = laser.GetComponent<LaserController>();
        if (laserController != null)
        {
            // Set the initial laser speed
            laserController.laserSpeed = laserSpeed;

            // Calculate a random direction for the laser (adjust the range as needed)
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            randomDirection.Normalize(); // Normalize the direction vector

            // Set the initial velocity of the laser
            laserController.SetInitialVelocity(randomDirection);
        }
    }
}
