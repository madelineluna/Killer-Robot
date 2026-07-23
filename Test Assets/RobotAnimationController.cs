using UnityEngine;

public class RobotAnimationController : MonoBehaviour
{
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            anim.SetBool("playerInsideZone", true);
            Debug.Log("Entered robot trigger");
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
        anim.SetBool("playerInsideZone", false);
        Debug.Log("Exited robot trigger");
        }   
    }
}
