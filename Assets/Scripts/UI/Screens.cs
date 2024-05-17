using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class Screens : MonoBehaviour {
    private PlayerStats ps;
    private OtherKeys ok;
    private PlayerMovement pm;

    [Header("Sounds")]
    private AudioSource click;
    private bool soundPlayed;
    private AudioSource gameOver;

    [Header("Screens")]
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject winScreen;

    [Header("Player")]
    [SerializeField] Transform player;
    [SerializeField] Transform playerSpawnPos;
    [SerializeField] GunStats[] weapons;
    public bool stopped;

    [Header("Timer")]
    public float timer;
    [SerializeField] TMP_Text timerText, bestText;

    private void Start() {
        ps = GameManager.i.ps;
        click = SoundManager.i.click;
        gameOver = SoundManager.i.gameOver;
        ok = player.GetComponent<OtherKeys>();
        pm = FindObjectOfType<PlayerMovement>();
        loseScreen.SetActive(false);
        pauseScreen.SetActive(false);
        winScreen.SetActive(false);
    }
    private void Update() {
        timer += Time.deltaTime;
        if (timer < 0)
            timer = 0;

        timerText.text = String.Format("{0:0.00}", timer);
        bestText.text = "Best: " + String.Format("{0:0.00}", ps.bestTime);
    }

    public void MenuSettings(bool screenOn) {
        click.Play();
        if (!screenOn) {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            loseScreen.SetActive(false);
            pauseScreen.SetActive(false);
            winScreen.SetActive(false);
            stopped = false;
        } else {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            stopped = true;
        }
    }
    public void Quit() {
        click.Play();
        Application.Quit();
    }
    public void ChangeLevel() {
        SceneManager.LoadScene("SampleScene");
    }
    public void Restart() {
        SoundManager.i.victory.Stop();
        click.Play();
        timer = 0;
        playerSpawnPos.position = Vector3.zero;
        ps.spawnPos = Vector3.zero;
        pm.hasDoubleJumped = false;
        Respawn();
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
        if (!soundPlayed) {
            loseScreen.SetActive(true);
            MenuSettings(true);
            click.Stop();
            gameOver.Play();
            soundPlayed = true;
        }
    }
    public void Respawn() {
        click.Play();
        player.position = playerSpawnPos.position;
        ps.currentHealth = ps.maxHealth;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        soundPlayed = false;
        ok.Respawn();

        Camera mc = Camera.main;
        
        if (mc.fieldOfView != 60) {
            mc.fieldOfView = 60;
            Keys k = GameManager.i.k;
            PlayerCam pc = FindObjectOfType<PlayerCam>();
            pc.xLookSense = k.xSense;
            pc.yLookSense = k.ySense;
            pc.weaponHolder.transform.position = new Vector3(0.4f, -0.2f, 0.5f);
        }
        

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
            tank.soundPlayed = true;
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