using UnityEngine;

public class Medkit : MonoBehaviour {
    private PlayerStats ps;
    public GameObject medKit;

    [Header("Rotation")]
    private Vector3 currentAngle;
    [SerializeField] float rotationSpeed;

    [Header("Timer")]
    public float timer;
    [SerializeField] float coolDown;

    private void Start() { ps = GameManager.i.ps; }
    private void Update() {
        currentAngle += Vector3.one * Time.deltaTime * rotationSpeed;
        medKit.transform.localEulerAngles = currentAngle;

        if (!medKit.activeInHierarchy) {
            if (timer > 0)
                timer -= Time.deltaTime;
            else {
                medKit.SetActive(true);
                timer = coolDown;
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player" && ps.currentHealth < ps.maxHealth && medKit.activeInHierarchy) {
            ps.currentHealth += 25;
            medKit.SetActive(false);
        }
    }
}