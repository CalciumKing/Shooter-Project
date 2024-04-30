using UnityEngine;
using UnityEngine.AI;

public class Shooting : MonoBehaviour {
    private Pathfinding pf;
    [SerializeField] GameObject bullet;

    [Header("Shooting")]
    public bool canShoot;
    [SerializeField] float timer, maxTime;
    private void Awake() { pf = GetComponentInParent<Pathfinding>(); }
    void Update() {
        if (!canShoot) {
            if (timer > 0)
                timer -= Time.deltaTime;
            else {
                canShoot = true;
                timer = maxTime;
            }
        }

        if (pf.distanceBetweenPlayer <= pf.GetComponent<NavMeshAgent>().stoppingDistance) {
            transform.parent.LookAt(pf.playerTarget);
            if (canShoot) {
                Instantiate(bullet, transform.position, transform.rotation * bullet.transform.localRotation);
                canShoot = false;
            }
        }
    }
}