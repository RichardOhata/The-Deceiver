using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NPCWander : MonoBehaviour
{
    [Header("Wander Settings")]
    public float wanderRadius = 10f;

    [Tooltip("Minimum time to stand still before picking a new path")]
    public float minIdleTime = 2f;

    [Tooltip("Maximum time to stand still before picking a new path")]
    public float maxIdleTime = 5f;

    [Tooltip("How fast the NPC rotates before walking")]
    public float turnSpeed = 5f;

    private NavMeshAgent agent;
    private Animator animator;
    public bool isInteracting = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        StartCoroutine(WanderSequence());
    }

    void Update()
    {
        if (isInteracting) return;
        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);
    }

    private IEnumerator WanderSequence()
    {

        while (true)
        {
            yield return new WaitUntil(() => !isInteracting);
            agent.ResetPath(); 
            float waitTime = Random.Range(minIdleTime, maxIdleTime);
            yield return new WaitForSeconds(waitTime);

      
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);

         
            Vector3 directionToPoint = (newPos - transform.position).normalized;
            directionToPoint.y = 0; 

            if (directionToPoint != Vector3.zero)
            {
           
                agent.updateRotation = false;

                Quaternion targetRotation = Quaternion.LookRotation(directionToPoint);

            
                while (Quaternion.Angle(transform.rotation, targetRotation) > 5f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                    yield return null; 
                }

          
                agent.updateRotation = true;
            }

          
            agent.SetDestination(newPos);

         
            yield return null;

           
            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

          
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    public void FreezeAndInteract()
    {
        if (isInteracting) return; // Prevent spamming if already frozen
        isInteracting = true;

        // 1. Hard stop the NavMeshAgent
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // 2. Force the idle animation
        animator.SetBool("isWalking", false);
    }

    public void Unfreeze()
    {
        if (!isInteracting) return;
        isInteracting = false;

        // Give the agent permission to move again
        if (agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }
    }
}
