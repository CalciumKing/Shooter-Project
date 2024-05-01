using UnityEngine;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour {
    [Header("References")]
    private Keys k;
    [SerializeField] GunStats gs;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform muzzlePos;
    [SerializeField] ParticleSystem muzzleFlash;

    [Header("Shooting")]
    [SerializeField] float cooldown;
    [SerializeField] float timer;
    [SerializeField] bool canShoot = true, usingPistol = false, usingShotgun = false;
    public float spread = .1f;
    [SerializeField] int numPellets;

    private void Start() {
        k = GameManager.i.k;
        timer = cooldown;
    }
    private void Update() {
        if (!usingPistol) {
            if (canShoot) {
                if (Input.GetKey(k.fire) && gs.currentAmmo > 0) {
                    if (!usingShotgun) {
                        bullet.GetComponent<Bullet>().damage = 10;
                        Instantiate(bullet, muzzlePos.position, transform.rotation * bullet.transform.localRotation);
                    } else {
                        bullet.GetComponent<Bullet>().damage = 6;
                        for (int i = 0; i < numPellets; i++) {
                            Quaternion pelletRot = transform.rotation;
                            pelletRot.x += Random.Range(-spread, spread);
                            pelletRot.y += Random.Range(-spread, spread);

                            Instantiate(bullet, muzzlePos.position, pelletRot * bullet.transform.localRotation);
                        }
                    }
                    muzzleFlash.Play();
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
            if (Input.GetKeyDown(k.fire) && gs.currentAmmo > 0) {
                Instantiate(bullet, muzzlePos.position, transform.rotation * bullet.transform.localRotation);
                muzzleFlash.Play();
                gs.currentAmmo--;
            }
        }

        if (Input.GetKeyDown(k.reload) && gs.currentAmmo != gs.maxAmmo)
            gs.currentAmmo = gs.maxAmmo;
    }
}