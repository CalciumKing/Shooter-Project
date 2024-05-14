using UnityEngine;

public class WallRunning : MonoBehaviour {
    private Keys k;
    private PlayerMovement pm;
    private Rigidbody rb;
    private PlayerCam mc;
    [SerializeField] Transform cameraHolder;

    [Header("Wallrunning")]
    [SerializeField] float wallRunForce;
    [SerializeField] LayerMask whatIsWall, whatIsGround;
    [SerializeField] float timer, cooldown;

    [Header("Detection")]
    public bool playerFlipped = false;
    [SerializeField] float wallCheckDistance, minJumpHeight;
    private RaycastHit leftWallHit, rightWallHit;
    private bool wallLeft, wallRight, wallForward, ceilingAbove;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        mc = Camera.main.GetComponent<PlayerCam>();
    }
    private void Start() {
        k = GameManager.i.k;
        ResetPlayerCeilingRunning();
    }
    private void Update() {
        CheckForWall();
        StateMachine();

        if (timer > 0)
            timer -= Time.deltaTime;
    }
    private void FixedUpdate() {
        if (pm.wallRunning) {
            WallRunningMovement();
            pm.hasDoubleJumped = false;
            if (Input.GetKey(k.jump))
                pm.Jump();
        } else if (playerFlipped)
            rb.AddForce(-Physics.gravity * rb.mass, ForceMode.Force);
    }

    private void CheckForWall() {
        wallRight = Physics.Raycast(transform.position, pm.orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -pm.orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
        wallForward = Physics.Raycast(transform.position, pm.orientation.forward, wallCheckDistance, whatIsWall);
        ceilingAbove = Physics.Raycast(transform.position, pm.orientation.up, wallCheckDistance * 2, whatIsGround);
    }
    private bool AboveGround() { return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround); }

    private void StateMachine() {
        if ((wallLeft || wallRight) && AboveGround() || wallForward) {
            if (playerFlipped)
                ResetPlayerCeilingRunning();

            if (wallRight)
                mc.Tilt(15);
            else if (wallLeft)
                mc.Tilt(-15);
            else if (wallForward)
                mc.Tilt(0);

            StartWallRun();
        } else
            if (pm.wallRunning)
                StopWallRun();

        if (ceilingAbove && !playerFlipped && (timer <= 0 || wallForward)) {
            cameraHolder.localRotation = Quaternion.Euler(cameraHolder.rotation.x, cameraHolder.rotation.y, 180);
            playerFlipped = true;
        } else if (!ceilingAbove && playerFlipped && !pm.grounded)
            ResetPlayerCeilingRunning();
    }

    private void StartWallRun() {
        if (!pm.wallRunning)
            pm.wallRunning = true;
    }
    private void WallRunningMovement() {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((pm.orientation.forward - wallForward).magnitude > (pm.orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        if (pm.verticalInput > 0) {
            rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
            if (!(wallLeft && pm.horizontalInput > 0) && !(wallRight && pm.horizontalInput < 0))
                rb.AddForce(-wallNormal * 100, ForceMode.Force);
        } else
            rb.velocity = Vector3.zero;

    }
    private void StopWallRun() {
        pm.wallRunning = false;
        rb.useGravity = true;
        mc.Tilt(0);
    }

    public void ResetPlayerCeilingRunning() {
        timer = cooldown;
        playerFlipped = false;
        cameraHolder.localRotation = Quaternion.Euler(cameraHolder.rotation.x, cameraHolder.rotation.y, 0);
        rb.AddForce(Physics.gravity * rb.mass);
    }
}