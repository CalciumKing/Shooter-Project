using UnityEngine;

public class Gun : MonoBehaviour {
    [Header("References")]
    private Keys k;
    public GunStats gs;
    public GameObject bullet;
    public Camera mc;

    [Header("Shooting")]
    public float cooldown, timer;
    public bool canShoot = true;
    public bool usingPistol = false;

    private void Start() {
        timer = cooldown;
        mc = Camera.main;
        k = GameManager.i.k;
    }
    private void Update() {
        if (!usingPistol) {
            if (canShoot) {
                if (Input.GetKey(k.fire) && gs.currentAmmo > 0) {
                    Instantiate(bullet, transform.position, transform.rotation * bullet.transform.localRotation);
                    gs.currentAmmo--;
                    canShoot = false;
                }
            } else {
                if (timer > 0)
                    timer -= Time.deltaTime;
                else {
                    canShoot = true;
                    timer = cooldown;
                }
            }
        } else {
            if(Input.GetKeyDown(k.fire) && gs.currentAmmo > 0) {
                Instantiate(bullet, transform.position, transform.rotation * bullet.transform.localRotation);
                gs.currentAmmo--;
            }
        }

        if (Input.GetKeyDown(k.reload) && gs.currentAmmo != gs.maxAmmo)
            gs.currentAmmo = gs.maxAmmo;
    }
}