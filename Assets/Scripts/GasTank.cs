using UnityEngine;

public class GasTank : MonoBehaviour {
    protected PlayerStats ps;
    [SerializeField] int damageAmount, damageDistance;
    public bool exploded = false;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] DamagePopup damagePopupPrefab;

    private void Start() { ps = GameManager.i.ps; }
    public void Explode(bool destroyAfter) {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        GasTank[] gasTanks = FindObjectsOfType<GasTank>();
        Grenade[] grenades = FindObjectsOfType<Grenade>();

        exploded = true;

        if (Vector3.Distance(player.position, transform.position) <= damageDistance)
            ps.currentHealth -= damageAmount;
        foreach (Enemy enemy in enemies) {
            if (enemy != null && Vector3.Distance(enemy.transform.position, transform.position) <= damageDistance) {
                try {
                    DamagePopup localPopup = Instantiate(damagePopupPrefab, transform.position, enemy.transform.rotation * Quaternion.Euler(0, 180, 0), enemy.transform);
                    localPopup.SetDamageText(damageAmount);
                    enemy.takeDamage(damageAmount);
                } catch { }
            }
        }

        foreach (GasTank tank in gasTanks)
            if (Vector3.Distance(tank.transform.position, transform.position) <= damageDistance)
                if (!tank.exploded)
                    tank.Explode(false);
        foreach (Grenade grenade in grenades) {
            if (Vector3.Distance(grenade.transform.position, transform.position) <= damageDistance) {
                if (!grenade.exploded) {
                    grenade.timer = .1f;
                    grenade.Explode(true);
                }
            }
        }

        if (!destroyAfter) {
            explosion.Play();
            gameObject.SetActive(false);
        } else
            Destroy(gameObject);
    }
}