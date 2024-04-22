using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("References")]
    private PlayerStats ps;
    private Keys k;
    private Rigidbody rb;
    private Sliding s;
    public Transform orientation, playerSpawn;
    public float horizontalInput, verticalInput;
    private Vector3 moveDirection;

    [Header("Jumping")]
    [SerializeField] float airMultiplier;
    public float jumpForce;
    public bool grounded, readyToJump, hasDoubleJumped, crouchInAir;
    public float groundDrag, playerHeight;
    public LayerMask whatIsGround;

    [Header("Falling")]
    public float fallTime;
    public int fallDamageMultiplier;
    public Vector3 startJump, endJump;

    [Header("Speed")]
    public bool crouched;
    public bool wallRunning;
    public float wallRunSpeed, moveSpeed, crouchSpeed, walkSpeed, runSpeed;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        s = GetComponent<Sliding>();
    }
    private void Start() {
        ps = GameManager.i.ps;
        k = GameManager.i.k;
        moveSpeed = walkSpeed;
        readyToJump = true;
        transform.position = ps.spawnPos;
        playerSpawn.position = ps.spawnPos;
    }
    private void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(k.jump)) {
            if (readyToJump && grounded) {
                startJump = transform.position;
                Jump();
                readyToJump = false;
                Invoke(nameof(ResetJump), 0);
            } else if (!grounded && !hasDoubleJumped) {
                hasDoubleJumped = true;
                Jump();
            }
        } else if (Input.GetKeyDown(k.crouch))
            crouched = true;
        else if (Input.GetKeyUp(k.crouch))
            crouched = false;

        if (crouched && grounded && !s.sliding)
            moveSpeed = crouchSpeed;
        else if (!crouched && grounded && !s.sliding) {
            if (Input.GetKey(KeyCode.LeftShift))
                moveSpeed = runSpeed;
            else
                moveSpeed = walkSpeed;
        } else if (wallRunning) {
            moveSpeed = wallRunSpeed;
            fallTime = 0;
        }

        DragController();
    }

    private void DragController() {
        if (grounded) {
            rb.drag = groundDrag;
        } else {
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

            hasDoubleJumped = false;
            crouchInAir = false;

            endJump = transform.position;
            if (fallTime >= 2.5f)
                if (startJump.y - endJump.y >= 7)
                    ps.currentHealth -= (int)fallTime * fallDamageMultiplier;
            fallTime = 0;
        } else if (!grounded) {
            fallTime += Time.deltaTime;
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    public void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump() { readyToJump = true; }
}