using UnityEngine;

public class Bullet : MonoBehaviour {
    private PlayerStats ps;
    [SerializeField] int speed = 40;
    public int damage;
    [SerializeField] Grenade grenade;
    [SerializeField] DamagePopup damagePopupPrefab;

    private float timer = 10f;

    private void Start() { ps = GameManager.i.ps; }
    void Update() {
        transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Enemy" && gameObject.tag == "Player Bullet") {
            DamagePopup localPopup = Instantiate(damagePopupPrefab, transform.position, other.transform.rotation * Quaternion.Euler(0, 180, 0), other.transform);
            localPopup.SetDamageText(damage);
            other.gameObject.GetComponent<Enemy>().takeDamage(damage);
        } else if (other.gameObject.tag == "Player" && gameObject.tag == "Enemy Bullet")
            ps.currentHealth -= damage;

        else if (other.gameObject.tag == "Gas Tank")
            other.gameObject.GetComponent<GasTank>().Explode(false);
        else if (other.gameObject.tag == "Grenade") {
            grenade = other.gameObject.GetComponent<Grenade>();
            grenade.timer = .1f;
        }

        if (!(other.gameObject.tag == "Player Bullet" && gameObject.tag == "Player Bullet") &&
            !(other.gameObject.tag == "Enemy Bullet" && gameObject.tag == "Enemy Bullet"))
            Destroy(gameObject);
    }
}