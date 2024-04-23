using UnityEngine;

public class GasTank : MonoBehaviour {
    protected PlayerStats ps;
    [SerializeField] private int damageAmount;
    [SerializeField] private int damageDistance;
    public bool exploded = false;

    private void Start() { ps = GameManager.i.ps; }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Enemy Bullet" || other.gameObject.tag == "Player Bullet")
            Explode(false);
    }
    protected void Explode(bool destroyAfter) {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] gasTanks = GameObject.FindGameObjectsWithTag("Gas Tank");
        GameObject[] grenades = GameObject.FindGameObjectsWithTag("Grenade");

        exploded = true;

        // EXPLOSION EFFECT

        if (Vector3.Distance(player.position, transform.position) <= damageDistance)
            ps.currentHealth -= damageAmount;
        foreach (GameObject enemy in enemies)
            if (Vector3.Distance(enemy.transform.position, transform.position) <= damageDistance)
                enemy.GetComponent<Enemy>().takeDamage(damageAmount);

        foreach (GameObject tank in gasTanks)
            if (Vector3.Distance(tank.transform.position, transform.position) <= damageDistance)
                if (!tank.GetComponent<GasTank>().exploded)
                    tank.GetComponent<GasTank>().Explode(false);
        foreach (GameObject grenade in grenades)
            if (Vector3.Distance(grenade.transform.position, transform.position) <= damageDistance)
                if(!grenade.GetComponent<Grenade>().exploded)
                    grenade.GetComponent<Grenade>().Explode(true);

        if (!destroyAfter)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }
}