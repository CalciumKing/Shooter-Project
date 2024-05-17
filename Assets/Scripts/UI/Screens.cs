using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class Screens : MonoBehaviour {
    private PlayerStats ps;
    private OtherKeys ok;
    private PlayerMovement pm;
    [SerializeField] AudioSource gameOver;
    private bool soundPlayed;

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
        ok = player.GetComponent<OtherKeys>();
        pm = FindObjectOfType<PlayerMovement>();
        loseScreen.SetActive(false);
        pauseScreen.SetActive(false);
        winScreen.SetActive(false);
    }
    private void Update() {
        timer += Time.deltaTime;

        timerText.text = String.Format("{0:0.00}", timer);
        bestText.text = "Best: " + String.Format("{0:0.00}", ps.bestTime);
    }

    public void MenuSettings(bool screenOn) {
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
        Application.Quit();
    }
    public void ChangeLevel() {
        SceneManager.LoadScene("SampleScene");
    }
    public void Restart() {
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
        loseScreen.SetActive(true);
        MenuSettings(true);
        if(!soundPlayed)
        {
            gameOver.Play();
            soundPlayed = true;
        }
    }
    public void Respawn() {
        player.position = playerSpawnPos.position;
        ps.currentHealth = ps.maxHealth;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        soundPlayed = false;
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