using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour {
    [Header("References")]
    private Keys k;
    public Transform cameraPosition;
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;
    [SerializeField] AudioSource slidingSound;

    [Header("Sliding")]
    public bool sliding;
    [SerializeField] float slideForce, slideTimer, slideMaxTime;
    [SerializeField] Vector3 lastPos;
    
    private float horizontalInput, verticalInput;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }
    private void Start() {
        k = GameManager.i.k;
    }
    private void Update() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(k.crouch) && (horizontalInput != 0 || verticalInput != 0) && pm.moveSpeed >= pm.runSpeed && !sliding)
            StartSlide();
        if (Input.GetKeyUp(k.crouch))
            StopSlide();
    }
    private void FixedUpdate() {
        if (sliding)
            SlidingMovement();
    }

    private void StartSlide() {
        sliding = true;
        slideTimer = slideMaxTime;
        lastPos = transform.position;
        slidingSound.Play();
    }
    private void SlidingMovement() {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

        if (lastPos.y > transform.position.y) {
            slideTimer = .5f;
            lastPos = transform.position;
        } else if (lastPos.y < transform.position.y) {
            slideTimer -= .1f;
            lastPos = transform.position;
        }

        slideTimer -= Time.deltaTime;

        if (slideTimer <= 0)
            StopSlide();
    }
    private void StopSlide() { sliding = false; }
}