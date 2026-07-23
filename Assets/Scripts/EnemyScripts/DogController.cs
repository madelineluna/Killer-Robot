using UnityEngine;

public class DogMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;

    void Update()
    {
        // Get input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Rotate dog toward movement direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            // Move the dog
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            // Play running animation
            if(animator != null)
                animator.SetBool("isRunning", true);
        }
        else
        {
            // Stop animation
            if(animator != null)
                animator.SetBool("isRunning", false);
        }
    }
}