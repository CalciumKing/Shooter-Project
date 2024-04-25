using UnityEngine;

public class Gun : MonoBehaviour {
    [Header("References")]
    private Keys k;
    [SerializeField] GunStats gs;
    [SerializeField] GameObject bullet;

    [Header("Shooting")]
    [SerializeField] float cooldown;
    [SerializeField] float timer;
    [SerializeField] bool canShoot = true;
    [SerializeField] bool usingPistol = false;

    private void Start() {
        timer = cooldown;
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