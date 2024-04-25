using UnityEngine;

public class Grenade : GasTank {
    [SerializeField] float timer, cooldown = 5f;

    private void Start() {
        ps = GameManager.i.ps;
        timer = cooldown;
    }
    private void Update() {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            Explode(true);
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Enemy Bullet" || other.gameObject.tag == "Player Bullet")
            Explode(true);
    }
}