using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [Header("References")]
    public Transform muzzle;
    public GameObject bulletPrefab;

    [Header("Shooting")]
    public float bulletSpeed = 18f;
    public float fireRate = 8f;          // bullets per second
    public float bulletLifeTime = 2f;

    [Header("Controls")]
    public bool shootWithLeftClick = true;
    public bool shootWithSpace = true;

    float nextFireTime;

    void Update()
    {
        if (muzzle == null || bulletPrefab == null) return;

        bool shootPressed = false;

        var mouse = Mouse.current;
        var kb = Keyboard.current;

        if (shootWithLeftClick && mouse != null && mouse.leftButton.isPressed)
            shootPressed = true;

        if (shootWithSpace && kb != null && kb.spaceKey.isPressed)
            shootPressed = true;

        if (shootPressed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + (1f / fireRate);
            Fire();
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);

        // Give it velocity forward
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = muzzle.forward * bulletSpeed; 
        }

        Destroy(bullet, bulletLifeTime);
    }
}
