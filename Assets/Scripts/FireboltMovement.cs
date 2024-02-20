using UnityEngine;

public class FireboltMovement : MonoBehaviour
{
    public float FireboltDuration = 0.5f;
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
    }
}