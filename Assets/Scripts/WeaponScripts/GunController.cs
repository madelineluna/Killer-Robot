// Worked on by:
// Josh Newsome
// This script reuses code from https://learn.unity.com/tutorial/let-s-try-shooting-with-raycasts
using UnityEngine;
using System.Collections;
public class GunController : MonoBehaviour
{
    public bool enableDebug = false;                                    // Set this to true to enable debug messages about what the gun hits
    public int damage = 1;                                              // Set the number of hitpoints that this gun will take away from shot objects with a health script
    public float fireRate = 0.1f;                                      // Number in seconds which controls how often the player can fire
    public float weaponRange = 50f;                                     // Distance in Unity units over which the player can fire
    public float hitForce = 0f;                                       // Amount of force which will be added to objects with a rigidbody shot by the player
    public Transform FireDirection;                                     // Holds a reference to the gun end object, marking the muzzle location of the gun
    public Camera aimCamera;                                            // Holds a reference to the camera which will be used to aim the gun
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible
    public LayerMask hittableLayers;                                    // Layer mask to specify which layers can be hit by the gun
    public PlayerCameraController recoilController;
    public float verticalRecoil = 1f;
    public float horizontalRecoil = 2f;

    void Awake()
    {
        if (recoilController == null && aimCamera != null)
        {
            recoilController = aimCamera.GetComponentInParent<PlayerCameraController>();
        }
    }

    // Shoot is called by PlayerController when the player presses the fire button, 
    // It calculates the position at which the camera is pointing, and then casts a ray to determine if it hits an object within range.
    // It calculates the direction from the gun to the target and then casts another ray to determine if it hits a hittable object.
    public void Shoot()
    {
        if (recoilController != null)
        {
            recoilController.ApplyRecoil(verticalRecoil, horizontalRecoil);
        }

        Vector3 rayOrigin = aimCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        Vector3 cameraTarget;
        if (Physics.Raycast(rayOrigin, aimCamera.transform.forward, out hit, weaponRange))
        {
            cameraTarget = hit.point;
        }
        else
        {
            cameraTarget = rayOrigin + (aimCamera.transform.forward * weaponRange);
        }
        Vector3 shotDirection = (cameraTarget - FireDirection.position).normalized;
        if (Physics.Raycast(FireDirection.position, shotDirection, out hit, weaponRange, hittableLayers))
        {
            int shotDamage = DecreaseDamageByDistance(hit.distance);

            if(enableDebug)
                Debug.Log("Hit: " + hit.collider.name + " for " + shotDamage + " damage at distance " + hit.distance);
            EnemyHealth targetHealth = hit.collider.GetComponentInParent<EnemyHealth>();
            if (targetHealth != null && shotDamage > 0)
            {
                targetHealth.TakeDamage(shotDamage);
            }
                // Barrel damage
            ExplodingBarrel barrel = hit.collider.GetComponent<ExplodingBarrel>();
            if (barrel == null)
                barrel = hit.collider.GetComponentInParent<ExplodingBarrel>();
            if (barrel != null && shotDamage > 0)
            {
                if(enableDebug)
                    Debug.Log("Shot barrel: " + hit.collider.name);
                barrel.TakeDamage(shotDamage);
            }
            if (hit.rigidbody != null && !hit.collider.tag.Equals("Enemy"))
            {
                hit.rigidbody.AddForce (-hit.normal * hitForce);
            }
        }
 

         
        
    }
    private IEnumerator ShotEffect()
    {
        yield return shotDuration;
    }

    private int DecreaseDamageByDistance(float distance)
    {
        if (distance < 10f)
        {
            return damage;
        }

        if (distance < 20f)
        {
            return Mathf.Max(1, Mathf.RoundToInt(damage * 0.5f));
        }

        if (distance <= 50f)
        {
            return Mathf.Max(1, Mathf.RoundToInt(damage * 0.2f));
        }

        return 0;
    }
}
