using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float moveSpeed = 5.0f;
    public float bulletSpeed = 10.0f;
    public float fireRate = 3.0f; // Fire a bullet every 3 seconds
    private float nextFireTime;

    public float rotationSpeed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        nextFireTime = Time.time + fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        // Player Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3 (horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement); 

        // Player rotation based on mouse input
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 lookAtPoint = ray.GetPoint(rayDistance);
            lookAtPoint.y = transform.position.y; // Keep the same y-position
            Quaternion targetRotation = Quaternion.LookRotation(lookAtPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }


}
