using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float moveSpeed = 5.0f;
    public float bulletSpeed = 5.0f;
    public float fireRate = 3.0f; // Fire a bullet every 3 seconds
    public float rotationSpeed = 10.0f;

    private float nextFireTime;
    private Rigidbody rb;
    private PlayerXPManager playerXPManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nextFireTime = Time.time + fireRate;
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
        }
        // Check if it's time to shoot
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
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
}
