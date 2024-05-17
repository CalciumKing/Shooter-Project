using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager i;
    private void Awake() {
        if (i == null)
            i = this;
        else
            Destroy(i);
    }

    [Header("Menu")]
    public AudioSource click;
    public AudioSource gameOver;
    public AudioSource saved;
    public AudioSource victory;

    [Header("Movements")]
    public AudioSource sliding;
    public AudioSource jump;
    public AudioSource landing;
    public AudioSource walking;

    [Header("Effects")]
    public AudioSource explosion;
    public AudioSource heal;
    public AudioSource damage;
    public AudioSource reload;
    public AudioSource throwGrenade;
}