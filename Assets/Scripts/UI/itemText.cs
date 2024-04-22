using TMPro;
using UnityEngine;

public class itemText : MonoBehaviour {
    public PlayerStats ps;
    private TextMeshProUGUI text;
    private Transform player;
    public Transform spawnPos;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start() {
        text.gameObject.SetActive(false);
    }
    private void Update() {
        if (text.gameObject.activeInHierarchy) {
            text.transform.LookAt(player);
            if (Input.GetKeyDown(KeyCode.E)) {
                Vector3 pos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z + 1);
                spawnPos.position = pos;
                ps.spawnPos = pos;
                text.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
            text.gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player")
            text.gameObject.SetActive(false);
    }
}