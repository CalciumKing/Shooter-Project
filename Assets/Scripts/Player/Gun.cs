using UnityEngine;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour {
    [Header("References")]
    private Keys k;
    private Screens s;
    [SerializeField] GunStats gs;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform muzzlePos;
    [SerializeField] ParticleSystem muzzleFlash;
    private AudioSource gunShot;

    [Header("Shooting")]
    [SerializeField] float cooldown;
    [SerializeField] float timer;
    [SerializeField] bool canShoot = true;
    [SerializeField] bool usingPistol = false, usingShotgun = false;
    [SerializeField] float spread = .1f;
    [SerializeField] int numPellets;

    private void Start() {
        k = GameManager.i.k;
        timer = cooldown;
        s = FindObjectOfType<Screens>();
        gunShot = GetComponent<AudioSource>();
    }
    private void Update() {
        if (!s.stopped) {
            if (!usingPistol) {
                if (canShoot) {
                    if (Input.GetKey(k.fire) && gs.currentAmmo > 0) {
                        if (!usingShotgun) {
                            bullet.GetComponent<Bullet>().damage = 10;
                            Instantiate(bullet, muzzlePos.position, transform.rotation * bullet.transform.localRotation);
                        } else {
                            if (Input.GetKey(k.aim))
                                spread /= 2;

                            bullet.GetComponent<Bullet>().damage = 6;
                            for (int i = 0; i < numPellets; i++) {
                                Quaternion pelletRot = transform.rotation;
                                pelletRot.x += Random.Range(-spread, spread);
                                pelletRot.y += Random.Range(-spread, spread);

                                Instantiate(bullet, muzzlePos.position, pelletRot * bullet.transform.localRotation);
                            }
                        }
                        gunShot.Play();
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
                    if (bullet.GetComponent<Bullet>().damage != 10)
                        bullet.GetComponent<Bullet>().damage = 10;
                    Instantiate(bullet, muzzlePos.position, transform.rotation * bullet.transform.localRotation);
                    gunShot.Play();
                    muzzleFlash.Play();
                    gs.currentAmmo--;
                }
            }

            if (Input.GetKeyDown(k.reload) && gs.currentAmmo != gs.maxAmmo)
                gs.currentAmmo = gs.maxAmmo;
        }
    }
}