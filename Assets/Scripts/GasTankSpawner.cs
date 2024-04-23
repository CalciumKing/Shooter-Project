using UnityEngine;

public class GasTankSpawner : MonoBehaviour {
    public GameObject gasTank;
    public Transform gasSpawnPos;

    [Header("Timer")]
    public float timer;
    [SerializeField] float coolDown;

    private void Update() {
        if (!gasTank.activeInHierarchy) {
            if (timer > 0)
                timer -= Time.deltaTime;
            else {
                timer = coolDown;
                gasTank.transform.position = gasSpawnPos.position;
                gasTank.transform.rotation = gasSpawnPos.rotation;
                gasTank.SetActive(true);
                gasTank.GetComponent<GasTank>().exploded = false;
            }
        }
    }
}