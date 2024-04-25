using UnityEngine;

public class OtherKeys : MonoBehaviour {
    private PlayerStats ps;
    private Keys k;
    [SerializeField] Cooldowns cd;

    [Header("Throwing")]
    [SerializeField] GameObject throwItem;
    [SerializeField] Transform throwPos;
    [SerializeField] int throwForce;
    [SerializeField] float throwTimer;
    [SerializeField] float throwCooldown;
    private Camera cam;
    private bool canThrow;

    [Header("Healing")]
    [SerializeField] float healTimer;
    [SerializeField] float healCooldown;
    [SerializeField] int healAmount;
    private bool canHeal;

    private void Start() {
        ps = GameManager.i.ps;
        k = GameManager.i.k;
        cam = Camera.main;
    }
    private void Update() {
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