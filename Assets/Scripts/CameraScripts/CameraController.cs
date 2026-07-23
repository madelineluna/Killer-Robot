// Worked on by:
// Josh Newsome
// Audrey Brainerd
// Madeline Luna

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 2f;
    float horizontalRotation = 0f;
    float verticalRotation = 0f;
    private Transform cameraTransform;
    public Vector3 cameraPosition = Vector3.zero;
    public float maxCameraVert = 50f;
    public float minCameraVert = -35f;
    public GameObject player;
    private Vector3 offset;
    public GameObject lookpoint;
    public PlayerController playerController;
    public PauseGameEvent pauseGameEvent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        offset = transform.position - player.transform.position; 
        cameraTransform = Camera.main.transform;
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
    
    }

    // Update is called once per frame
    void LateUpdate() {
        if (pauseGameEvent.isPaused) {
            return;
        }
        RotateCamera();
    }

    void RotateCamera()
    {
        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        bool invertY = PlayerPrefs.GetInt("InvertY", 0) == 1;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        if (invertY)
            mouseY = -mouseY;

        horizontalRotation += mouseX;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minCameraVert, maxCameraVert);
        cameraPosition = Quaternion.Euler(verticalRotation , horizontalRotation, 0) * offset;
        // freeze camera on death (to stop player rotation)
        // to-do, look at animating camera on death or allowing player to control it?
        if (playerController.isAlive)
        {
            // Apply the same rotation directly to the player
            player.transform.rotation = Quaternion.Euler(0, horizontalRotation, 0);
            transform.position = player.transform.position + cameraPosition;
            transform.LookAt(lookpoint.transform);
        } else
        {
            transform.LookAt(player.transform);
        }
    }

/* Commented out by Madeline Luna
    void RotateCamera()
    {
        horizontalRotation += Input.GetAxis("Mouse X") * mouseSensitivity;
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, minCameraVert, maxCameraVert);
        cameraPosition = Quaternion.Euler(verticalRotation , horizontalRotation, 0) * offset;
        // freeze camera on death (to stop player rotation)
        // to-do, look at animating camera on death or allowing player to control it?
        if (playerController.isAlive)
        {
            // Apply the same rotation directly to the player
            player.transform.rotation = Quaternion.Euler(0, horizontalRotation, 0);
            transform.position = player.transform.position + cameraPosition;
            transform.LookAt(lookpoint.transform);
        } else
        {
            transform.LookAt(player.transform);
        }
    }
*/
}