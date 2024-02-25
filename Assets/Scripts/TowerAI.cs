using UnityEngine;

public class TowerAI : MonoBehaviour
{
    public float detectionRadius = 20f;
    private Transform targetEnemy;
    public LayerMask enemyLayer;
    public GameObject fireboltPrefab;
    public float spellSpeed = 20f;
    public float shootingCooldown = 0.5f;
    public float projectileDuration = 1.5f;
    private float lastShotTime = float.MinValue; // Time since last shot was fired

    // Update is called once per frame
    void Update()
    {
        FindClosestEnemy();
        if (targetEnemy != null)
        {
            ShootAt(targetEnemy);
        }
    }

    void FindClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hitCollider in hitColliders)
        {
            HealthBar monsterHealth = hitCollider.GetComponent<HealthBar>();
            if (monsterHealth.currentHealth > 0)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider.transform;
                }
            }
        }
        targetEnemy = closestEnemy;
    }

    void ShootAt(Transform enemy)
    {
        // Check if enough time has passed since the last shot
        if (Time.time - lastShotTime >= shootingCooldown)
        {
            lastShotTime = Time.time; // Update the last shot time

            // Calculate the direction to the enemy
            Vector3 direction = (enemy.position - transform.position).normalized;

            // Calculate the spawn position 1 unit away from the tower towards the target
          
            Vector3 spawnOffset = new(1, 6.5f, -1);
            Vector3 spawnPosition = transform.position + transform.TransformDirection(spawnOffset) + direction * 1.5f;

            // Instantiate the firebolt prefab at the calculated spawn position
            GameObject fireboltInstance = Instantiate(fireboltPrefab, spawnPosition, Quaternion.LookRotation(direction));
            HomingMissile homingComponent = fireboltInstance.GetComponent<HomingMissile>();
            FireboltMovement fireboltMovement = fireboltInstance.GetComponent<FireboltMovement>();
            if (homingComponent != null)
            {
                homingComponent.target = enemy;
            }
            fireboltMovement.FireboltDuration = projectileDuration;
            // Apply velocity to the firebolt to move it towards the enemy
            if (fireboltInstance.TryGetComponent<Rigidbody>(out var fireboltRb))
            {
                fireboltRb.velocity = direction * spellSpeed;
            }
            else
            {
                Debug.LogError("Firebolt prefab does not have a Rigidbody component.");
            }

            // Play the spell sound effect
            if (fireboltRb.TryGetComponent<AudioSource>(out var spellAudioSource))
            {
                spellAudioSource.volume = AudioManager.Instance.sfxVolume; // Set the volume
                spellAudioSource.Play(); // Play the sound attached to the spell prefab
            }
        }
    }
}
