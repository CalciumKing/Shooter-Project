using UnityEngine;

public class Grenade : GasTank {
    [SerializeField] float timer, cooldown = 5f;
    [SerializeField] ParticleSystem explosionPrefab, explosionInstance;
    [SerializeField] bool explosionFinished = false;

    private void Start() {
        ps = GameManager.i.ps;
        timer = cooldown;
    }
    private void Update() {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            Explode(true);

        if (timer > 0 && timer < 0.1f && !explosionFinished)
        {
            ExplodeEffect();
        }
    }

    public void ExplodeEffect()
    {
        explosionInstance = Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
        explosionInstance.Play();
        explosionFinished = true;
    }

    /*private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Enemy Bullet" || other.gameObject.tag == "Player Bullet")
            Explode(true);
    }*/
}