using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;  // Array for patrol points
    private int currentPatrolIndex = 0;  // Index for current patrol point
    public float patrolSpeed = 2f;  // Speed while patrolling
    public float chaseSpeed = 5f;  // Speed while chasing the player
    public float detectionRange = 10f;  // Detection range to trigger chase
    public Transform player;  // Reference to the player

    private bool isChasing = false;  // Flag to determine if enemy is chasing

    private Animator animator;  // Reference to the enemy's animator (if using animations)

    void Start()
    {
        // Get the animator component (if your 3D prefab has one)
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player is in range or visible
        DetectPlayer();

        if (isChasing)
        {
            // Chase the player
            ChasePlayer();
            if (animator != null)
            {
                animator.SetBool("Running", true);  // Trigger a "run" animation (optional)
            }
        }
        else
        {
            // Patrol behavior
            Patrol();
            if (animator != null)
            {
                animator.SetBool("Running", false);  // Trigger a "walk" or idle animation (optional)
            }
        }
    }

    void Patrol()
    {
        Transform targetPatrolPoint = patrolPoints[currentPatrolIndex];
        MoveTowards(targetPatrolPoint.position, patrolSpeed);

        // If the enemy is close to the patrol point, move to the next one
        if (Vector3.Distance(transform.position, targetPatrolPoint.position) < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            RaycastHit hit;
            Vector3 direction = player.position - transform.position;

            // Cast a ray to check if there are no obstacles between the enemy and the player
            if (Physics.Raycast(transform.position, direction, out hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    isChasing = true;  // The player is detected, start chasing
                }
            }
        }
        else
        {
            isChasing = false;  // The player is out of range, stop chasing
        }
    }

    void ChasePlayer()
    {
        // Move towards the player's position at chase speed
        MoveTowards(player.position, chaseSpeed);
    }

    void MoveTowards(Vector3 target, float speed)
    {
        // Calculate direction and move towards the target position
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Optionally, rotate the enemy to face the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);  // Smooth rotation
    }
}
