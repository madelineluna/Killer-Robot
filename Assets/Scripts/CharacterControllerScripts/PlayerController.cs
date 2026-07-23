// Worked on by:
// Josh Newsome
// Madeline Luna
// Audrey Brainerd

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// This script provides player jumping and movement 

public class PlayerController : MonoBehaviour
{
    // Animation
    // public Animator WeaponSwingController;
    // Ground Movement
    private Rigidbody rb;
    public float MoveSpeed = 5f;
    public float crouchSpeed = 2.5f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float aimingSpeedMultiplier = 0.55f;
    public float shootingSpeedMultiplier = 0.6f;

    private float moveHorizontal;
    private float moveForward;

    private bool invertY = false;

    // Alive or Dead
    public bool isAlive = true;
    public bool debugMode = false;

    // Jumping
    public bool canJump = false;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f; // Multiplies gravity when falling down
    public float ascendMultiplier = 2f; // Multiplies gravity for ascending to peak of jump
    private bool isGrounded = true;
    private float jumpDelay = 0f;
        public float holdJumpDelay = 0.15f;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    public float groundedRaycastDistance = .25f;
    public Animator animator;
    private PlayerCameraController playerCameraController;

    // Shooting
    public GunController gunController;
    public float fallbackFireInterval = 0.5f;
    private float nextFireTime = 0f;

    // Utility
    public int partsCollected = 0;
    public int partsToCollect = 3;

    // Controls
    [Header("Control Setup")]
    public InputActionReference sprintAction;
    public InputActionReference jumpAction;
    public InputActionReference fireAction;
    public InputActionReference moveAction;
    public InputActionReference meleeAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerCameraController = GetComponent<PlayerCameraController>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        invertY = PlayerPrefs.GetInt("InvertY", 0) == 1;
    }

    void Update()
    {
            if (isAlive)
            {
            Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
            moveHorizontal = moveInput.x;
            moveForward = moveInput.y;

            bool jumpPressed = jumpAction.action.IsPressed();

            if (debugMode){
                if (InputSystem.actions["Teleport Boss"].WasPressedThisFrame())
                {
                    Debug.Log("DEBUG: Boss Teleport pressed!");
                    Transform bossRoomTransform = GameObject.Find("Boss Room Transform").transform;
                    if (bossRoomTransform == null) {
                        Debug.Log("Boss Room Transform not found!");
                        return;
                    }
                    rb.isKinematic = true;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    transform.position = bossRoomTransform.position;
                    rb.isKinematic = false;
                }

                if (InputSystem.actions["Heal"].WasPressedThisFrame())
                {
                    PlayerHealth1 playerHealth = GetComponent<PlayerHealth1>();
                    Debug.Log("DEBUG: Heal pressed!");
                    playerHealth.Heal(100);
                }

                if (Input.GetKeyDown(KeyCode.K))
                {
                    if (isAlive)
                    {
                        Debug.Log("DEBUG: Kill Player pressed!");
                        Die();
                    }
                }

                if (Input.GetKeyDown(KeyCode.I))
                {
                    bool current = PlayerPrefs.GetInt("InvertY", 0) == 1;
                    invertY = !current;
                    PlayerPrefs.SetInt("InvertY", invertY ? 1 : 0);
                    PlayerPrefs.Save();
                }
                        

/*              Disable this as falling glitch was fixed
                if (InputSystem.actions["Teleport Fall"].WasPressedThisFrame())
                {
                    Transform fallingTransform = GameObject.Find("Falling Transform").transform;
                    rb.linearVelocity = Vector3.zero;
                    rb.MovePosition(fallingTransform.position);
                }
*/
            }


            if (jumpPressed && isGrounded)
            {
                Jump();
            }

            // Checking when we're on the ground and keeping track of our ground check delay
            if (!isGrounded && groundCheckTimer <= 0f)
            {
                Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
                isGrounded = Physics.Raycast(rayOrigin, Vector3.down, groundedRaycastDistance, groundLayer);
            }
            else
            {
                groundCheckTimer -= Time.deltaTime;
            }

            if (isGrounded)
            {
                jumpDelay += Time.deltaTime;
            }
            else
            {
                jumpDelay = 0f;
            }

            // Sprint command
            if (sprintAction.action.IsPressed()) // can only sprint while grounded
            {
                MoveSpeed = sprintSpeed;
            }
            else
            {
                MoveSpeed = walkSpeed;
            }

            bool isAiming = playerCameraController != null && playerCameraController.IsAiming;
            if (isAiming)
            {
                MoveSpeed *= aimingSpeedMultiplier;
            }

            bool isHoldingFire = fireAction.action.IsPressed();
            if (isHoldingFire)
            {
                MoveSpeed *= shootingSpeedMultiplier;
            }

            if (isHoldingFire && Time.time >= nextFireTime)
            {
                animator.SetBool("isFiring", true);

                float fireInterval = gunController != null ? gunController.fireRate : fallbackFireInterval;
                nextFireTime = Time.time + fireInterval;
            }
            else if (!isHoldingFire)
            {
                animator.SetBool("isFiring", false);
            }

            bool isHoldingMelee = (Mouse.current != null && Mouse.current.rightButton.isPressed) || meleeAction.action.IsPressed();
            if (isHoldingMelee)
            {
                animator.SetBool("isMelee", true);
            }
            else
            {
                animator.SetBool("isMelee", false);
            }

        } else {
            if (jumpAction.action.IsPressed())
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    void FixedUpdate()
    {
        if (isAlive) {
            MovePlayer();
            ApplyJumpPhysics();
        }
    }

    void MovePlayer()
    {
        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveForward).normalized;
    
        bool onMovingPlatform = transform.parent != null;
        bool hasMoveInput = Mathf.Abs(moveHorizontal) > 0.08f || Mathf.Abs(moveForward) > 0.08f;
    
        // -----------------------------
        // Movement while on a platform
        // -----------------------------
        if (onMovingPlatform)
        {
            // Let parenting handle the carry.
            // Do not use Rigidbody X/Z movement while parented.
    
            if (hasMoveInput)
            {
                float platformMoveMultiplier = 0.8f; // tune 0.5 - 0.8 if needed
                Vector3 moveStep = movement * MoveSpeed * platformMoveMultiplier * Time.fixedDeltaTime;
    
                // Move in WORLD space so prefab rotation does not mess up controls
                rb.MovePosition(rb.position + moveStep);
                animator.SetFloat("MoveX", moveHorizontal * 0.5f);
                animator.SetFloat("MoveZ", moveForward * 0.5f);
            }
            else
            {
                animator.SetFloat("MoveX", 0f);
                animator.SetFloat("MoveZ", 0f);
            }
    
            return;
        }
    
        // -----------------------------
        // Normal movement off platform
        // -----------------------------
        Vector3 targetVelocity = movement * MoveSpeed;
        Vector3 velocity = rb.linearVelocity;
    
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;
    
        if (isGrounded && !hasMoveInput)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
        animator.SetFloat("MoveX", localVel.x / 10f);
        animator.SetFloat("MoveZ", localVel.z / 10f);
    }

    void Jump()
    {
        if (!canJump) return; // Prevent jumping if the player hasn't collected enough parts
        if (transform.parent != null && transform.parent.GetComponent<MovingStep>() != null) transform.SetParent(null);
        groundCheckTimer = groundCheckDelay;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z); // Initial burst for the jump
        isGrounded = false;
        animator.Play("Jump");
        animator.SetBool("isGrounded", false);
    }

    public void Shooting()
    {
        // animator.Play("Shoot");
        gunController.Shoot();
    }

    public void EndFiring()
    {
        animator.SetBool("isFiring", false);
    }

    void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0) 
        {
            // Falling: Apply fall multiplier to make descent faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        } // Rising
        else if (rb.linearVelocity.y > 0)
        {
            // Rising: Change multiplier to make player reach peak of jump faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * ascendMultiplier  * Time.fixedDeltaTime;
        }
    }

    // Pickup Collection
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Jump Part")) {
            other.gameObject.SetActive(false);
            AudioEventManager.Instance.PlayAudio(AudioType.Pickup, transform.position);
            partsCollected++;
        } else { if (other.gameObject.CompareTag("Health Pickup")) {
            PlayerHealth1 playerHealth = GetComponent<PlayerHealth1>();
            other.gameObject.SetActive(false);
            AudioEventManager.Instance.PlayAudio(AudioType.Pickup, transform.position);
            if (playerHealth != null)
            {
                playerHealth.Heal(10);
            }
        }

        }
        if (partsCollected >= partsToCollect) {
            canJump = true;
        }
   }

    public void Die()
    {
        // to do, implement different types of animations based on how player dies
        // to do, consider using a bool to set animator bool for player is dead?
        isAlive = false;
        animator.Play("PlayerDeath");
        DeathScreenController deathScreen = FindFirstObjectByType<DeathScreenController>();
        if (deathScreen != null)
        {
            deathScreen.ShowDeathScreen();
        }
    }
}
