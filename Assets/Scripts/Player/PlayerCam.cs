using UnityEngine;

public class PlayerCam : MonoBehaviour {
    private Keys k;
    private Camera mc;
    private Screens s;
    private WallRunning wr;

    [Header("Transforms")]
    public Transform orientation;
    public Transform cameraHolder;
    public Transform weaponHolder;

    private float xRotation, yRotation;
    public int xLookSense, yLookSense;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mc = Camera.main;
        k = GameManager.i.k;
        xLookSense = k.xSense;
        yLookSense = k.ySense;
        wr = FindObjectOfType<WallRunning>();
        s = FindObjectOfType<Screens>();
    }

    private void Update() {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xLookSense * 100;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * yLookSense * 100;

        if (wr.playerFlipped) {
            mouseX = -mouseX;
            mouseY = -mouseY;
        }

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (!wr.playerFlipped) {
            cameraHolder.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        } else {
            cameraHolder.localRotation = Quaternion.Euler(xRotation, yRotation, 180);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        if (!s.stopped) {
            if (Input.GetKeyDown(k.aim)) {
                weaponHolder.transform.Translate(new Vector3(-.4f, 0, .5f), transform);
                mc.fieldOfView = 20;
                xLookSense = k.scopeXSense;
                yLookSense = k.scopeYSense;
            } else if (Input.GetKeyUp(k.aim)) {
                weaponHolder.transform.Translate(new Vector3(.4f, 0, -.5f), transform);
                mc.fieldOfView = 60;
                xLookSense = k.xSense;
                yLookSense = k.ySense;
            }
        }
    }
    public void Tilt(float zTilt) { transform.localRotation = Quaternion.Euler(0, 0, zTilt); }
}