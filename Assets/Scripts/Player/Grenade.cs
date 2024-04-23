using UnityEngine;

public class Grenade : GasTank {
    public float cooldown = 5f;
    public float timer;

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