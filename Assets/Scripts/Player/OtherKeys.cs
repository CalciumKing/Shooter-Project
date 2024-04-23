using UnityEngine;

public class OtherKeys : MonoBehaviour {
    private PlayerStats ps;
    private Keys k;
    public Cooldowns cd;

    [Header("Throwing")]
    public GameObject throwItem;
    public int throwForce;
    public bool canThrow;
    public float throwTimer;
    public float throwCooldown;

    [Header("Healing")]
    public bool canHeal;
    public float healTimer;
    public float healCooldown;
    public int healAmount;

    private void Start() {
        ps = GameManager.i.ps;
        k = GameManager.i.k;
    }
    private void Update() {
        if (canThrow) {
            if (Input.GetKeyDown(k.throwable)) {
                Instantiate(throwItem, transform.position, Quaternion.identity);
                throwItem.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce, ForceMode.Impulse);
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
    }
}