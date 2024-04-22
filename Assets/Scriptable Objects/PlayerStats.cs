using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject {
    [Header("Weapons")]
    public GameObject currentWeapon;
    public GameObject currentInsta;
    public KeyCode currentGunKey;

    [Header("XP")]
    [SerializeField] public int xp;
    [SerializeField] public int levelInterval;
    [SerializeField] public int level;

    [Header("Health")]
    [SerializeField] public int currentHealth;
    [SerializeField] public int maxHealth;

    [Header("Spawn Info")]
    public Vector3 spawnPos;
}