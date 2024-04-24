using UnityEngine;

public class Screens : MonoBehaviour {
    private PlayerStats ps;
    private OtherKeys ok;
    [SerializeField] GameObject loseScreen;

    [Header("Player")]
    [SerializeField] Transform player;
    [SerializeField] Transform playerSpawnPos;
    [SerializeField] GunStats[] weapons;

    private void Start() {
        ps = GameManager.i.ps;
        ok = player.GetComponent<OtherKeys>();
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

        ok.Respawn();

        foreach (GunStats weapon in weapons)
            weapon.currentAmmo = weapon.maxAmmo;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        loseScreen.SetActive(false);
        Time.timeScale = 1;
    }
}