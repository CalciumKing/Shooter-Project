using UnityEngine;

public class Screens : MonoBehaviour {
    private PlayerStats ps;
    public GameObject loseScreen;

    [Header("Player")]
    public Transform player;
    public Transform playerSpawnPos;
    public GunStats[] weapons;

    private void Start() {
        ps = GameManager.i.ps;
    }
    public void Death() {
        loseScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Restart() {
        player.position = playerSpawnPos.position;
        ps.currentHealth = ps.maxHealth;

        foreach (GunStats weapon in weapons)
            weapon.currentAmmo = weapon.maxAmmo;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        loseScreen.SetActive(false);
        Time.timeScale = 1;
    }
}