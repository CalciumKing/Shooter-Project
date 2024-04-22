using UnityEngine;

public class Enemy : MonoBehaviour {
    private PlayerStats ps;

    [Header("Enemy Info")]
    [SerializeField] public int maxHealth;
    [SerializeField] public int health;
    [SerializeField] int xp;

    private void Start() {
        ps = GameManager.i.ps;
        health = maxHealth;
    }
    public void takeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            ps.xp += xp;
            gameObject.SetActive(false);
        }
    }
}