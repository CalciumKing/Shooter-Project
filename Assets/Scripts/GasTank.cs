using UnityEngine;

public class GasTank : MonoBehaviour {
    private PlayerStats ps;
    [SerializeField] int damageAmount;
    [SerializeField] int damageDistance;

    private void Start() { ps = GameManager.i.ps; }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Enemy Bullet" || other.gameObject.tag == "Player Bullet") {

            // EXPLOSION EFFECT

            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (Vector3.Distance(player.position, transform.position) <= damageDistance)
                ps.currentHealth -= damageAmount;
            foreach (GameObject enemy in enemies)
                if (Vector3.Distance(enemy.transform.position, transform.position) <= damageDistance)
                    enemy.GetComponent<Enemy>().takeDamage(damageAmount);
                        gameObject.SetActive(false);
        }
    }
}