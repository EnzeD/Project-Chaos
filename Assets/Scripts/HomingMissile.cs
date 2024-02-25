using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public Transform target;
    public float speed = 20f;
    public float turnSpeed = 50f; // How quickly the missile can turn to follow the target

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetDirection = (target.position - transform.position).normalized;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f);
            rb.velocity = newDirection * speed;
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
}