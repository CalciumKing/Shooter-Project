using UnityEngine;

public class GasTankSpawner : MonoBehaviour {
    public GameObject gasTank;
    [SerializeField] Transform gasSpawnPos;
    protected AudioSource explosionSound;
    public bool soundPlayed;

    [Header("Timer")]
    public float timer;
    [SerializeField] float coolDown;

    private void Update() {
        if (!gasTank.activeInHierarchy) {
            if (!soundPlayed) {
                SoundManager.i.explosion.Play();
                soundPlayed = true;
            }

            if (timer > 0)
                timer -= Time.deltaTime;
            else {
                timer = coolDown;
                gasTank.transform.position = gasSpawnPos.position;
                gasTank.transform.rotation = gasSpawnPos.rotation;
                gasTank.SetActive(true);
                gasTank.GetComponent<GasTank>().exploded = false;
                soundPlayed = false;
            }
        }
    }
}