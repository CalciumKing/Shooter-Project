using UnityEngine;

public class Grenade : GasTank {
    public float timer;
    [SerializeField] float cooldown = 5f;
    [SerializeField] ParticleSystem explosionPrefab, explosionInstance;

    private void Start() {
        ps = GameManager.i.ps;
        timer = cooldown;
    }
    private void Update() {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            Explode(true);

        if (timer > 0 && timer <= .1f)
            ExplodeEffect();
    }

    public void ExplodeEffect() {
        explosionInstance = Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
        explosionInstance.Play();
    }
}