using UnityEngine;

public class Grenade : GasTank {
    public float timer;
    [SerializeField] float cooldown = 5f;
    [SerializeField] ParticleSystem explosionInstance;
    private AudioSource explosionSound;
    private bool soundPlayed;

    private void Start() {
        ps = GameManager.i.ps;
        explosionSound = SoundManager.i.explosion;
        timer = cooldown;
    }
    private void Update() {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            Explode(true);

        if (timer > 0 && timer <= .1f) {
            if (!soundPlayed) {
                explosionSound.Play();
                soundPlayed = true;
            }
            ExplodeEffect();
        }
    }

    public void ExplodeEffect() {
        explosionInstance = Instantiate(base.explosionPrefab, transform.position, Quaternion.identity, transform);
        explosionInstance.Play();
    }
}