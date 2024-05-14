using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject {
    [Header("Weapons")]
    public GameObject currentWeapon;
    public GameObject currentInsta;
    public KeyCode currentGunKey;

    [Header("XP")]
    public int xp;
    public int levelInterval;
    public int level;

    [Header("Health")]
    public int currentHealth;
    public int maxHealth;

    [Header("Spawn Info")]
    public Vector3 spawnPos;

    [Header("Timer")]
    public float bestTime;
}