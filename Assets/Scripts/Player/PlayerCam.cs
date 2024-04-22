using UnityEngine;

public class PlayerCam : MonoBehaviour {
    private Keys k;
    public Camera mc;
    public Transform orientation;
    public Transform cameraHolder;
    public Transform weaponHolder;
    private float xRotation, yRotation;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mc = Camera.main;
        k = GameManager.i.k;
    }

    private void Update() {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * k.xSense * 100;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * k.ySense * 100;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        if (Input.GetKeyDown(k.aim)) {
            weaponHolder.transform.Translate(new Vector3(-.4f, 0, .5f), transform);
            mc.fieldOfView = 20;
        } else if (Input.GetKeyUp(k.aim)) {
            weaponHolder.transform.Translate(new Vector3(.4f, 0, -.5f), transform);
            mc.fieldOfView = 60;
        }
    }
    public void Tilt(float zTilt) { transform.localRotation = Quaternion.Euler(0, 0, zTilt); }
}