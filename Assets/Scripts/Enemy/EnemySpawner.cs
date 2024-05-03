using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [Header("References")]
    public GameObject enemy;
    public Transform enemySpawnPos;
    private Enemy e;
    private Pathfinding pf;

    [Header("Timer")]
    public float timer;
    [SerializeField] float coolDown;

    private void Start() {
        e = enemy.GetComponent<Enemy>();
        pf = enemy.GetComponent<Pathfinding>();
    }
    private void Update() {
        if (!enemy.activeInHierarchy) {
            if (timer > 0)
                timer -= Time.deltaTime;
            else {
                timer = coolDown;
                enemy.transform.position = enemySpawnPos.position;
                enemy.transform.rotation = enemySpawnPos.rotation;
                e.health = e.maxHealth;
                pf.currentPatrolPoint = pf.patrolPoints[0];
                enemy.SetActive(true);
            }
        }
    }
}