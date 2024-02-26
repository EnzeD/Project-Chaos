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

    public ResourceData resourceData;
    private float lastChaosIncrementTime = 0f;
    private readonly float attackAnimationDuration = 0.833f;

    public AudioClip attackAudioClip;
    private bool attackAudioHasBeenPlayed = false;

    public PlayerController playerController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spawnManager = FindObjectOfType<SpawnManager>();

        // Find and assign the player and spawn center transforms correctly
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null) playerTransform = playerObject.transform;
        if (playerObject != null) playerController = playerObject.GetComponent<PlayerController>();

        GameObject spawnCenterObject = GameObject.Find("MainBuilding");
        if (spawnCenterObject != null) spawnCenterTransform = spawnCenterObject.transform;
    }

    void Update()
    {
        if (!isDead)
        {
            ChooseTarget();
            MoveToTarget();
            CheckAttackCompletion();
        }

    }

    void ChooseTarget()
    {
        // Initialize closest distance with a high value
        float closestDistance = Mathf.Infinity;

        // Initialize currentTarget as null
        Transform closestTarget = null;

        // Check distance to player
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        if (distanceToPlayer < closestDistance)
        {
            closestDistance = distanceToPlayer;
            closestTarget = playerTransform;
        }

        // Check distance to Main Building
        float distanceToSpawnCenter = Vector3.Distance(spawnCenterTransform.position, transform.position);
        if (distanceToSpawnCenter < closestDistance)
        {
            closestDistance = distanceToSpawnCenter;
            closestTarget = spawnCenterTransform;
        }

        // Check distance to each defense
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Defenses");
        foreach (GameObject building in buildings)
        {
            float distanceToBuilding = Vector3.Distance(building.transform.position, transform.position);
            if (distanceToBuilding < closestDistance)
            {
                closestDistance = distanceToBuilding;
                closestTarget = building.transform;
            }
        }

        // Set the closest target
        currentTarget = closestTarget;
    }

    void MoveToTarget()
    {
        if (currentTarget == null) return;

        // Assuming the target has a collider component attached
        if (!currentTarget.TryGetComponent<Collider>(out var targetCollider))
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
            if (!attackAudioHasBeenPlayed)
            {
                AudioManager.Instance.PlaySFX(attackAudioClip);
                attackAudioHasBeenPlayed = true;
                playerController.ShowDamageText(-1, playerTransform.transform.position + Vector3.up * 4);
            }

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
        spawnManager.MonsterDied(gameObject.transform);

        // Play the spell sound effect
        if (gameObject.TryGetComponent<AudioSource>(out var monsterAudioSource))
        {
            AudioManager.Instance.PlaySFX(monsterAudioSource.clip); // Play the sound attached
        }

        // Destroy the monster after the length of the Die animation
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, animationLength);
    }
    public void OnAttackComplete()
    {
        // Check if enough time has passed since the last chaos increment
        if (Time.time - lastChaosIncrementTime >= attackAnimationDuration)
        {
            if (resourceData != null)
            {
                resourceData.IncrementChaos(); // Increment chaos
                lastChaosIncrementTime = Time.time; // Update the time of the last increment
                attackAudioHasBeenPlayed = false;
            }
        }
    }
    void CheckAttackCompletion()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isAttacking = stateInfo.IsName("Attack01"); 

        // Check if the animation is playing and nearing its end
        if (isAttacking && stateInfo.normalizedTime >= 0.95f) // 0.95 is near the end
        {
            OnAttackComplete();
        }
    }
}