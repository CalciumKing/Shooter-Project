using UnityEngine;

public class KillOnTouch : MonoBehaviour {
    private PlayerStats ps;
    private void Start() { ps = GameManager.i.ps; }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player")
            ps.currentHealth = 0;
    }
}