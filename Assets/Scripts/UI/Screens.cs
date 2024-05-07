using UnityEngine;

public class Screens : MonoBehaviour {
    private PlayerStats ps;
    private OtherKeys ok;

    [Header("Screens")]
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject pauseScreen;

    [Header("Player")]
    [SerializeField] Transform player;
    [SerializeField] Transform playerSpawnPos;
    [SerializeField] GunStats[] weapons;
    public bool stopped;

    private void Start() {
        ps = GameManager.i.ps;
        ok = player.GetComponent<OtherKeys>();
    }
    private void MenuSettings(bool screenOn) {
        if (!screenOn) {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            loseScreen.SetActive(false);
            pauseScreen.SetActive(false);
            stopped = false;
        } else {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            stopped = true;
        }
    }
    public void Pause() {
        pauseScreen.SetActive(true);
        MenuSettings(true);
    }
    public void UnPause() {
        pauseScreen.SetActive(false);
        MenuSettings(false);
    }
    public void Death() {
        loseScreen.SetActive(true);
        MenuSettings(true);
    }
    public void Restart() {
        player.position = playerSpawnPos.position;
        ps.currentHealth = ps.maxHealth;

        ok.Respawn();

        foreach (GunStats weapon in weapons)
            weapon.currentAmmo = weapon.maxAmmo;

        EnemySpawner[] enemySpawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner enemy in enemySpawners) {
            enemy.enemy.SetActive(false);
            enemy.timer = 0;
        }

        GasTankSpawner[] gasTankSpawners = FindObjectsOfType<GasTankSpawner>();
        foreach (GasTankSpawner tank in gasTankSpawners) {
            tank.gasTank.SetActive(false);
            tank.timer = 0;
        }

        Grenade[] grenade = FindObjectsOfType<Grenade>();
        foreach (Grenade g in grenade)
            Destroy(g.gameObject);

        Bullet[] bullet = FindObjectsOfType<Bullet>();
        foreach (Bullet b in bullet)
            Destroy(b.gameObject);

        MenuSettings(false);
    }
}