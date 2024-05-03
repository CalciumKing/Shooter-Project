using UnityEngine;

public class OtherKeys : MonoBehaviour {
    private PlayerStats ps;
    private Keys k;
    [SerializeField] Cooldowns cd;

    [Header("Pausing")]
    [SerializeField] Screens s;
    private bool paused;

    [Header("Throwing")]
    [SerializeField] GameObject throwItem;
    [SerializeField] Transform throwPos;
    [SerializeField] int throwForce;
    [SerializeField] float throwTimer, throwCooldown;
    private Camera cam;
    private bool canThrow;

    [Header("Healing")]
    [SerializeField] int healAmount;
    [SerializeField] float healTimer, healCooldown;
    private bool canHeal;

    [Header("Melee")]
    [SerializeField] DamagePopup damagePopupPrefab;
    [SerializeField] Transform gunHolder;
    [SerializeField] bool hitting = false;
    [SerializeField] int meleeDamage, meleeRange;
    [SerializeField] float meleeTimer, meleeCooldown;

    private void Start() {
        ps = GameManager.i.ps;
        k = GameManager.i.k;
        cam = Camera.main;
    }
    private void Update() {
        if (meleeTimer > 0)
        {
            meleeTimer -= Time.deltaTime;
        }
        else
        {
            if (hitting)
            {
                Vector3 gh = gunHolder.transform.position;
                gunHolder.transform.Translate(new Vector3(0, 0, -.3f));
                hitting = false;
            }
            if (Input.GetKeyDown(k.melee))
            {
                Vector3 gh = gunHolder.transform.position;
                gunHolder.transform.Translate(new Vector3(0, 0, .3f));
                meleeTimer = meleeCooldown;
                hitting = true;

                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                GameObject[] gasTanks = GameObject.FindGameObjectsWithTag("Gas Tank");
                
                foreach (GameObject enemy in enemies)
                {
                    if (Vector3.Distance(gunHolder.position, enemy.transform.position) <= meleeRange)
                    {
                        DamagePopup localPopup = Instantiate(damagePopupPrefab, transform.position, enemy.transform.rotation * Quaternion.Euler(0, 180, 0), enemy.transform);
                        localPopup.SetDamageText(meleeDamage);
                        enemy.GetComponent<Enemy>().takeDamage(meleeDamage);
                    }
                }

                foreach (GameObject tank in gasTanks)
                    if (Vector3.Distance(tank.transform.position, transform.position) <= meleeRange)
                        if (!tank.GetComponent<GasTank>().exploded)
                            tank.GetComponent<GasTank>().Explode(false);
            }
        }

        if (canThrow) {
            if (Input.GetKeyDown(k.throwable)) {
                GameObject proj = Instantiate(throwItem, throwPos.position, Quaternion.identity);
                proj.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
                canThrow = false;
            }
        } else {
            if (throwTimer > 0) {
                throwTimer -= Time.deltaTime;
                cd.UpdateThrowCooldown(throwCooldown, throwTimer);
            } else {
                canThrow = true;
                throwTimer = throwCooldown;
            }
        }

        if (canHeal) {
            if (Input.GetKeyDown(k.heal) && ps.currentHealth != ps.maxHealth) {
                ps.currentHealth += healAmount;
                canHeal = false;
            }
        } else {
            if (healTimer > 0) {
                healTimer -= Time.deltaTime;
                cd.UpdateHealCooldown(healCooldown, healTimer);
            } else {
                canHeal = true;
                healTimer = healCooldown;
            }
        }

        if (Input.GetKeyDown(k.pause))
        {
            if (paused == false)
                s.Pause();
            else
                s.UnPause();

            paused = !paused;
        }
    }
    public void Respawn() {
        cd.UpdateHealCooldown(healCooldown, 0);
        canHeal = true;
        healTimer = healCooldown;
        cd.healImage.fillAmount = 0;
        cd.UpdateThrowCooldown(throwCooldown, 0);
        canThrow = true;
        throwTimer = throwCooldown;
        cd.throwableImage.fillAmount = 0;
    }
}