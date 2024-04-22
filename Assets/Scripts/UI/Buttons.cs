using UnityEngine;

public class Buttons : MonoBehaviour {
    public PlayerStats ps;

    public void Restart() {
        ps.currentHealth = ps.maxHealth;
    }
}