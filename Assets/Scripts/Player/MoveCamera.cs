using UnityEngine;

public class MoveCamera : MonoBehaviour {
    private Keys k;
    [SerializeField] Transform cameraPosition;

    private void Start() {
        k = GameManager.i.k;
    }
    private void Update() {
        transform.position = cameraPosition.position;
        if (Input.GetKeyDown(k.crouch))
            cameraPosition.transform.position = new Vector3(cameraPosition.transform.position.x, cameraPosition.transform.position.y - 0.5f, cameraPosition.transform.position.z);
        else if (Input.GetKeyUp(k.crouch))
            cameraPosition.transform.position = new Vector3(cameraPosition.transform.position.x, cameraPosition.transform.position.y + 0.5f, cameraPosition.transform.position.z);
    }
}