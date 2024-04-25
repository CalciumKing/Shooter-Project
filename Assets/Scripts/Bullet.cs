using UnityEngine;

public class Bullet : MonoBehaviour {
    private PlayerStats ps;
    [SerializeField] int speed = 40;
    [SerializeField] int damage;

    [Header("Timer")]
    public float timer = 10f;

    private void Start() { ps = GameManager.i.ps; }
    void Update() {
        transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Enemy" && gameObject.tag == "Player Bullet")
            other.gameObject.GetComponent<Enemy>().takeDamage(damage);
        else if (other.gameObject.tag == "Player" && gameObject.tag == "Enemy Bullet") {
            ps.currentHealth -= damage;
        }
        Destroy(gameObject);
    }
}