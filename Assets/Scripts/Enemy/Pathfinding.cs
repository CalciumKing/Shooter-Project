using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour {
    private NavMeshAgent agent;

    [Header("Targeting")]
    public Transform playerTarget;
    [SerializeField] float enemyRange = 10f, chaseSpeed, returnSpeed;
    public float distanceBetweenPlayer;

    [Header("Patroling")]
    public Transform[] patrolPoints;
    public Transform currentPatrolPoint;
    public float distanceBetweenPoint;

    private void Awake() { agent = GetComponent<NavMeshAgent>(); }
    private void Start() {
        currentPatrolPoint = patrolPoints[0];
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update() {
        distanceBetweenPlayer = Vector3.Distance(playerTarget.position, transform.position);
        if (distanceBetweenPlayer <= enemyRange) {
            agent.stoppingDistance = 5;
            agent.speed = chaseSpeed;
            agent.SetDestination(playerTarget.position);
        } else {
            agent.stoppingDistance = 0;
            agent.speed = returnSpeed;
            agent.SetDestination(currentPatrolPoint.position);

            distanceBetweenPoint = Vector3.Distance(currentPatrolPoint.position, transform.position);
            if (distanceBetweenPoint <= 2) {
                for (int i = 0; i < patrolPoints.Length; i++) {
                    if (currentPatrolPoint == patrolPoints[i]) {
                        if (i + 1 > patrolPoints.Length - 1)
                            currentPatrolPoint = patrolPoints[0];
                        else
                            currentPatrolPoint = patrolPoints[i + 1];
                        break;
                    }
                }
            }
        }
    }
}