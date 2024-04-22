using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager i;
    public PlayerStats ps;
    public Keys k;

    private void Awake() {
        if (i == null)
            i = this;
        else
            Destroy(i);
    }
}