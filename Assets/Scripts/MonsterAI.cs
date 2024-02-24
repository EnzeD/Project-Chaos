using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class MonsterAI : MonoBehaviour
{
    public Transform playerTransform;
    public Transform spawnCenterTransform;
    public float attackRange = 2.0f; // Range within which monster will attack

    private NavMeshAgent agent;
    private Animator animator;
    private Transform currentTarget;
    private SpawnManager spawnManager;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

    private bool isDead;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spawnManager = FindObjectOfType<SpawnManager>();

        // Find and assign the player and spawn center transforms correctly
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null) playerTransform = playerObject.transform;

        GameObject spawnCenterObject = GameObject.Find("MainBuilding");
        if (spawnCenterObject != null) spawnCenterTransform = spawnCenterObject.transform;
    }

    void Update()
    {
        if (!isDead)
        {
            ChooseTarget();
            MoveToTarget();
        }

    }

    void ChooseTarget()
    {
        // Determine the closest target
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        float distanceToSpawnCenter = Vector3.Distance(spawnCenterTransform.position, transform.position);

        currentTarget = distanceToPlayer < distanceToSpawnCenter ? playerTransform : spawnCenterTransform;
    }

    void MoveToTarget()
    {
        if (currentTarget == null) return;

        // Assuming the target has a collider component attached
        Collider targetCollider = currentTarget.GetComponent<Collider>();
        if (targetCollider == null)
        {
            Debug.LogError("Target does not have a collider.", currentTarget.gameObject);
            return;
        }

        Vector3 closestPoint = targetCollider.ClosestPoint(transform.position); // Find the closest point on the target's collider to this monster
        float distanceToTarget = Vector3.Distance(closestPoint, transform.position); // Use the closest point for distance calculation

        if (distanceToTarget <= attackRange)
        {
            // Stop moving and attack
            agent.isStopped = true;
            animator.SetBool(IsWalking, false);
            animator.SetBool(IsAttacking, true);
        }
        else
        {
            // Move towards the closest point on the target's collider
            agent.isStopped = false;
            agent.SetDestination(closestPoint);
            animator.SetBool(IsAttacking, false);
            animator.SetBool(IsWalking, true);
        }

        // Face the target direction
        Vector3 direction = (closestPoint - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void Die()
    {
        isDead = true;

        // Disable the NavMeshAgent to stop the monster from moving
        agent.enabled = false;

        // Play the Die animation
        animator.SetTrigger("Die");

        // Share with the spawn manager
        spawnManager.MonsterDied();

        // Play the spell sound effect
        AudioSource monsterAudioSource = gameObject.GetComponent<AudioSource>();
        if (monsterAudioSource != null)
        {
            monsterAudioSource.volume = AudioManager.Instance.sfxVolume; // Set the volume
            monsterAudioSource.Play(); // Play the sound attached
        }

        // Destroy the monster after the length of the Die animation
        // Assumes you have an animation clip named "Die" in your Animator
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, animationLength);
    }
}