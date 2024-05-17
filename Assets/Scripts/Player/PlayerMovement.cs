using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("References")]
    private PlayerStats ps;
    private Keys k;
    private Rigidbody rb;
    private Sliding s;
    private WallRunning wr;
    public Transform orientation, playerSpawn;
    public float horizontalInput, verticalInput;
    private Vector3 moveDirection;

    [Header("Sounds")]
    private bool groundBelow;
    private AudioSource walkingSound, landingSound, jumpSound;
    private bool walkingSoundPlaying, landingSoundPlayed;

    [Header("Jumping")]
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float airMultiplier, jumpForce;
    [SerializeField] float groundDrag, playerHeight;
    public bool grounded, readyToJump, hasDoubleJumped, crouchInAir;

    [Header("Falling")]
    [SerializeField] int fallDamageMultiplier;
    [SerializeField] float fallTime;
    private Vector3 startJump, endJump;

    [Header("Speed")]
    [SerializeField] bool crouched;
    public bool wallRunning;
    public float moveSpeed, runSpeed;
    [SerializeField] float wallRunSpeed, crouchSpeed, walkSpeed;

    private void Awake() {
        walkingSound = SoundManager.i.walking;
        landingSound = SoundManager.i.landing;
        jumpSound = SoundManager.i.jump;
        rb = GetComponent<Rigidbody>();
        s = GetComponent<Sliding>();
        wr = GetComponent<WallRunning>();
    }
    private void Start() {
        Physics.gravity = new Vector3(0, -10, 0);
        ps = GameManager.i.ps;
        k = GameManager.i.k;
        moveSpeed = walkSpeed;
        readyToJump = true;
        transform.position = ps.spawnPos;
        playerSpawn.position = ps.spawnPos;
    }
    private void Update() {
        if (!wr.playerFlipped) {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
            horizontalInput = Input.GetAxisRaw("Horizontal");
        } else {
            grounded = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + 0.2f, whatIsGround);
            horizontalInput = -Input.GetAxisRaw("Horizontal");
        }
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(k.jump)) {
            if (readyToJump && grounded) {
                if (wr.playerFlipped)
                    wr.ResetPlayerCeilingRunning();

                startJump = transform.position;
                Jump();
                readyToJump = false;
                Invoke(nameof(ResetJump), 0);
            } else if (!grounded && !hasDoubleJumped) {
                hasDoubleJumped = true;
                Jump();
                fallTime = 0;
            }
        } else if (Input.GetKey(k.crouch)) {
            if (wr.playerFlipped)
                wr.ResetPlayerCeilingRunning();
            else {
                if (wallRunning)
                    rb.AddForce(-transform.up * jumpForce, ForceMode.Impulse);
                else
                    crouched = true;
            }
        } else if (Input.GetKeyUp(k.crouch) && crouched)
            crouched = false;

        if (crouched && grounded && !s.sliding) {
            moveSpeed = crouchSpeed;
            walkingSound.pitch = .25f;
        } else if (!crouched && grounded && !s.sliding) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                moveSpeed = runSpeed;
                walkingSound.pitch = 0.75f;
            } else {
                moveSpeed = walkSpeed;
                walkingSound.pitch = 0.5f;
            }
        } else if (wallRunning) {
            moveSpeed = wallRunSpeed;
            walkingSound.pitch = 1;
            fallTime = 0;
        }

        DragController();
    }

    private void DragController() {
        if (grounded) {
            rb.drag = groundDrag;
            groundBelow = Physics.Raycast(transform.position, -orientation.up, 2, whatIsGround);
            if (groundBelow && landingSoundPlayed == false) {
                landingSound.Play();
                landingSoundPlayed = true;
                walkingSoundPlaying = false;
            }
        } else {
            landingSoundPlayed = false;
            rb.drag = 0;
            if (Input.GetKey(k.crouch) && !crouchInAir) {
                rb.AddForce(-transform.up * jumpForce, ForceMode.Impulse);
                crouchInAir = true;
            }
        }

        Vector3 flatVal = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVal.magnitude > moveSpeed) {
            Vector3 limitedVel = flatVal.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void FixedUpdate() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded) {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

            if (!walkingSoundPlaying && (verticalInput != 0 || horizontalInput != 0)) {
                walkingSound.Play();
                walkingSoundPlaying = true;
            } else if (walkingSoundPlaying && verticalInput == 0 && horizontalInput == 0) {
                walkingSound.Stop();
                walkingSoundPlaying = false;
            }

            hasDoubleJumped = false;
            crouchInAir = false;

            endJump = transform.position;
            if (fallTime >= 2.5f && startJump.y - endJump.y >= 7)
                ps.currentHealth -= (int)fallTime * fallDamageMultiplier;
            fallTime = 0;
        } else if (!grounded) {
            walkingSound.Stop();
            fallTime += Time.deltaTime;
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    public void Jump() {
        jumpSound.Play();
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (!wr.playerFlipped)
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        else
            rb.AddForce(-transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump() { readyToJump = true; }
}