using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour {
    private PlayerStats ps;
    [SerializeField] int speed = 40;
    public int damage;
    [SerializeField] DamagePopup damagePopupPrefab;

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
        switch (other.gameObject.tag)
        {
            case "Enemy":
                if (gameObject.tag == "Player Bullet")
                {
                    DamagePopup localPopup = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity, other.transform);
                    localPopup.SetDamageText(damage);
                    localPopup.transform.rotation = Quaternion.LookRotation(transform.position - GameObject.FindGameObjectWithTag("Player").transform.position);
                    other.gameObject.GetComponent<Enemy>().takeDamage(damage);
                }
                break;
            case "Player":
                if (gameObject.tag == "Enemy Bullet")
                    ps.currentHealth -= damage;
                break;

            case "Gas Tank":
                other.gameObject.GetComponent<GasTank>().Explode(false);
                break;
            case "Grenade":
                Grenade grenade = other.gameObject.GetComponent<Grenade>();
                grenade.timer = .1f;
                break;
        }

        if(!(other.gameObject.tag == "Player Bullet" && gameObject.tag == "Player Bullet") &&
            !(other.gameObject.tag == "Enemy Bullet" && gameObject.tag == "Enemy Bullet"))
            Destroy(gameObject);
    }
}