using UnityEngine;

public class Screens : MonoBehaviour {
    private PlayerStats ps;
    private OtherKeys ok;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject pauseScreen;

    [Header("Player")]
    [SerializeField] Transform player;
    [SerializeField] Transform playerSpawnPos;
    [SerializeField] GunStats[] weapons;

    private void Start() {
        ps = GameManager.i.ps;
        ok = player.GetComponent<OtherKeys>();
    }
    private void MenuSettings()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            loseScreen.SetActive(false);
            pauseScreen.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void Pause()
    {
        pauseScreen.SetActive(true);
        MenuSettings();
    }
    public void UnPause()
    {
        pauseScreen.SetActive(true);
        MenuSettings();
    }
    public void Death() {
        loseScreen.SetActive(true);
        MenuSettings();
    }
    public void Restart() {
        player.position = playerSpawnPos.position;
        ps.currentHealth = ps.maxHealth;

        ok.Respawn();

        foreach (GunStats weapon in weapons)
            weapon.currentAmmo = weapon.maxAmmo;

        EnemySpawner[] enemySpawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner enemy in enemySpawners)
        {
            enemy.enemy.SetActive(false);
            enemy.timer = 0;
        }

        GasTankSpawner[] gasTankSpawners = FindObjectsOfType<GasTankSpawner>();
        foreach (GasTankSpawner tank in gasTankSpawners)
        {
            tank.gasTank.SetActive(false);
            tank.timer = 0;
        }

        Grenade[] grenade = FindObjectsOfType<Grenade>();
        foreach (Grenade g in grenade)
            Destroy(g.gameObject);

        MenuSettings();
    }
}