// Worked on by 
//Joshua Newsome 
//Madeline Luna
//Audrey Brainerd

using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    private const KeyCode AimKey = KeyCode.C;
    public Transform cameraPivot;
    public float lookSpeed = 1f;
    public float deadzone = 0.01f;
    public float minVerticalAngle = -60f;
    public float maxVerticalAngle = 70f;
    public float aimRecoilMultiplier = 0.5f;
    public float aimZoomFov = 55f;
    public float aimBlendSpeed = 10f;
    public float aimNoiseAmplitudeMultiplier = 0.35f;
    public float aimNoiseFrequencyMultiplier = 0.7f;
    private Rigidbody body;
    private float horizontalDegrees;
    private float verticalDegrees;
    private float queuedHorizontal;
    private bool needsHorizontalUpdate;

    private PauseGameEvent pauseGameEvent;
    private PlayerController playercontroller;
    private CinemachineCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineNoise;
    private float defaultFieldOfView;
    private float defaultNoiseAmplitude = 1f;
    private float defaultNoiseFrequency = 1f;

    public InputActionReference controllerLook;
    public bool IsAiming { get; private set; }

    void Start()
    {
        body = GetComponent<Rigidbody>();
        pauseGameEvent = FindFirstObjectByType<PauseGameEvent>();
        playercontroller = FindFirstObjectByType<PlayerController>();
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        horizontalDegrees = transform.eulerAngles.y;
        verticalDegrees = cameraPivot.localEulerAngles.x;
        needsHorizontalUpdate = true;
        if (verticalDegrees > 180f)
        {
            verticalDegrees -= 360f;
        }

        if (cinemachineCamera != null)
        {
            defaultFieldOfView = cinemachineCamera.Lens.FieldOfView;
            cinemachineNoise = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            if (cinemachineNoise != null)
            {
                defaultNoiseAmplitude = cinemachineNoise.AmplitudeGain;
                defaultNoiseFrequency = cinemachineNoise.FrequencyGain;
            }
        }
    }

    void Update()
    {
        IsAiming = Input.GetKey(AimKey);

        if ((pauseGameEvent != null && pauseGameEvent.isPaused) || (playercontroller != null && playercontroller.isAlive == false))
        {
            return;
        }

        Vector2 controllerInput = controllerLook.action.ReadValue<Vector2>();

        float lookX = Input.GetAxisRaw("Mouse X");
        float lookY = Input.GetAxisRaw("Mouse Y");

        float controllerX = controllerInput.x;
        float controllerY = controllerInput.y;

        if (Mathf.Abs(lookX) < Mathf.Abs(controllerX))
        {
            lookX = controllerX;
        }

        if (Mathf.Abs(lookY) < Mathf.Abs(controllerY))
        {
            lookY = controllerY;
        }

        // // Debug.Log($"lookX: {lookX}, lookY: {lookY}");

        if (Mathf.Abs(lookX) < deadzone)
        {
            lookX = 0f;
        }

        if (Mathf.Abs(lookY) < deadzone)
        {
            lookY = 0f;
        }

        float sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1.5f);
        // // Debug.Log("Sensitivity: " + sensitivity);
        bool invertY = PlayerPrefs.GetInt("InvertY", 0) == 1;

        if (invertY)
        {
            lookY = -lookY;
        }

        float finalLookSpeed = lookSpeed * sensitivity * .1f;

        queuedHorizontal += lookX * finalLookSpeed;
        verticalDegrees -= lookY * finalLookSpeed;
        verticalDegrees = Mathf.Clamp(verticalDegrees, minVerticalAngle, maxVerticalAngle);

        cameraPivot.localRotation = Quaternion.Euler(verticalDegrees, 0f, 0f);
        UpdateAimCamera();
    }

    void FixedUpdate()
    {
        if (body != null)
        {
            body.angularVelocity = Vector3.zero;
        }

        if (Mathf.Approximately(queuedHorizontal, 0f) && !needsHorizontalUpdate)
        {
            return;
        }

        horizontalDegrees += queuedHorizontal;
        Quaternion targetHorizontal = Quaternion.Euler(0f, horizontalDegrees, 0f);

        if (body != null)
        {
            body.MoveRotation(targetHorizontal);
        }
        else
        {
            transform.rotation = targetHorizontal;
        }

        queuedHorizontal = 0f;
        needsHorizontalUpdate = false;
    }

    public void ApplyRecoil(float verticalRecoil, float horizontalRecoil)
    {
        float recoilMultiplier = IsAiming ? aimRecoilMultiplier : 1f;
        float verticalKick = Mathf.Abs(verticalRecoil) * recoilMultiplier;
        float horizontalKick = -Mathf.Abs(horizontalRecoil) * recoilMultiplier;

        verticalDegrees = Mathf.Clamp(verticalDegrees - verticalKick, minVerticalAngle, maxVerticalAngle);
        horizontalDegrees += horizontalKick;
        needsHorizontalUpdate = true;
    }

    private void UpdateAimCamera()
    {
        if (cinemachineCamera != null)
        {
            float targetFieldOfView = IsAiming ? aimZoomFov : defaultFieldOfView;
            LensSettings lens = cinemachineCamera.Lens;
            lens.FieldOfView = Mathf.Lerp(lens.FieldOfView, targetFieldOfView, aimBlendSpeed * Time.deltaTime);
            cinemachineCamera.Lens = lens;
        }

        if (cinemachineNoise != null)
        {
            float targetAmplitude = IsAiming ? defaultNoiseAmplitude * aimNoiseAmplitudeMultiplier : defaultNoiseAmplitude;
            float targetFrequency = IsAiming ? defaultNoiseFrequency * aimNoiseFrequencyMultiplier : defaultNoiseFrequency;
            cinemachineNoise.AmplitudeGain = Mathf.Lerp(cinemachineNoise.AmplitudeGain, targetAmplitude, aimBlendSpeed * Time.deltaTime);
            cinemachineNoise.FrequencyGain = Mathf.Lerp(cinemachineNoise.FrequencyGain, targetFrequency, aimBlendSpeed * Time.deltaTime);
        }
    }
}
