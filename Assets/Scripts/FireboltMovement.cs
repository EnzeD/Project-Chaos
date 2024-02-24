using UnityEngine;

public class FireboltMovement : MonoBehaviour
{
    public float FireboltDuration = 0.5f;
    public float SpellDamage = 50f;
    void Start()
    {
        Destroy(gameObject, FireboltDuration); // Destroy the firebolt after 5 seconds to clean up
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy the firebolt on collision
        }

        // Check if the firebolt hits a monster
        if (collision.gameObject.CompareTag("Monster"))
        {
            // Update health and trigger death
            MonsterAI monsterAI = collision.gameObject.GetComponent<MonsterAI>();
            HealthBar monsterHealth = collision.gameObject.GetComponent<HealthBar>();
            if (monsterAI != null)
            {
                monsterHealth.UpdateHealth(-SpellDamage);
                if (monsterHealth.currentHealth <= 0) 
                {
                    monsterAI.Die();
                }
            }
            Destroy(gameObject); // Destroy the firebolt on collision with a monster
        }
    }
}