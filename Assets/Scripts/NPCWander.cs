using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCWander : MonoBehaviour
{
    [Header("Wander Settings")]
    public float wanderRadius = 10f; // How far they can pick a new point
    public float wanderTimer = 5f;   // How long they wait before picking a new point

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // When the timer hits the limit, find a new random destination
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0; // Reset the timer
        }
    }

    // This function finds a random valid point on the NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        // Get a random point inside a sphere
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;

        // SamplePosition ensures the point is actually on the walkable NavMesh
        // If the random point lands inside a wall, it finds the closest valid edge
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}
