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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] gasTanks = GameObject.FindGameObjectsWithTag("Gas Tank");
        GameObject[] grenades = GameObject.FindGameObjectsWithTag("Grenade");

        exploded = true;

        if (Vector3.Distance(player.position, transform.position) <= damageDistance)
            ps.currentHealth -= damageAmount;
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) <= damageDistance)
            {
                DamagePopup localPopup = Instantiate(damagePopupPrefab, transform.position, enemy.transform.rotation * Quaternion.Euler(0, 180, 0), enemy.transform);
                localPopup.SetDamageText(damageAmount);
                enemy.GetComponent<Enemy>().takeDamage(damageAmount);
            }
        }

        foreach (GameObject tank in gasTanks)
            if (Vector3.Distance(tank.transform.position, transform.position) <= damageDistance)
                if (!tank.GetComponent<GasTank>().exploded)
                    tank.GetComponent<GasTank>().Explode(false);
        foreach (GameObject grenade in grenades)
            if (Vector3.Distance(grenade.transform.position, transform.position) <= damageDistance)
                if (!grenade.GetComponent<Grenade>().exploded) {
                    Grenade grenadeComponent = grenade.GetComponent<Grenade>();
                    grenadeComponent.timer = .1f;
                    grenadeComponent.Explode(true);
                }

        if (!destroyAfter) {
            explosion.Play();
            gameObject.SetActive(false);
        } else
            Destroy(gameObject);
    }
}