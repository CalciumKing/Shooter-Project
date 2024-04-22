using UnityEngine;

public class WallRunning : MonoBehaviour {
    private Keys k;
    private PlayerMovement pm;
    private Rigidbody rb;
    public Transform mc;

    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit, rightWallHit;
    private bool wallLeft, wallRight;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }
    private void Start() {
        k = GameManager.i.k;
    }
    private void Update() {
        CheckForWall();
        StateMachine();
    }
    private void FixedUpdate() {
        if (pm.wallRunning) {
            WallRunningMovement();
            pm.hasDoubleJumped = false;
            if (Input.GetKey(k.jump))
                pm.Jump();
        }
    }

    private void CheckForWall() {
        wallRight = Physics.Raycast(transform.position, pm.orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -pm.orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }
    private bool AboveGround() { return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround); }

    private void StateMachine() {
        if ((wallLeft || wallRight) && pm.verticalInput > 0 && AboveGround())
            StartWallRun();
        else {
            if (pm.wallRunning) {
                StopWallRun();
            }
        }
    }

    private void StartWallRun() {
        if (!pm.wallRunning){
            pm.wallRunning = true;
            if (wallRight)
                mc.GetComponent<PlayerCam>().Tilt(15);
            else if (wallLeft)
                mc.GetComponent<PlayerCam>().Tilt(-15);
        }
    }
    private void WallRunningMovement() {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((pm.orientation.forward - wallForward).magnitude > (pm.orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (!(wallLeft && pm.horizontalInput > 0) && !(wallRight && pm.horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
    }
    private void StopWallRun() {
        pm.wallRunning = false;
        rb.useGravity = true;
        mc.GetComponent<PlayerCam>().Tilt(0);
    }
}