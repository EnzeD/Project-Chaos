using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class SpellShooter : MonoBehaviour
{
    public GameObject fireboltPrefab;
    public GameObject waterSplashPrefab;
    public GameObject earthQuakePrefab;

    public AudioClip fireboltSound;
    public AudioClip waterSplashSound;
    public AudioClip earthQuakeSound;
    private AudioSource audioSource;

    public GameObject UICannotAttack;

    public float spellSpeed = 20f; // Speed at which all spells move
    public float shootingCooldown = 0.5f; // Cooldown between shots for all spells

    private float lastShotTime = float.MinValue; // Time since last shot was fired

    private Animator animator;
    private NavMeshAgent agent;
    private Rigidbody rb;

    // Enum to keep track of the current power
    public enum PowerType { Fire, Water, Earth }
    public PowerType currentPower;

    public void SetCurrentPower(PowerType newPower)
    {
        currentPower = newPower;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        
        // Check if the left mouse button is clicked, cooldown has elapsed, and no UI element is blocking the event
        if (Input.GetMouseButton(0) && Time.time - lastShotTime >= shootingCooldown && !EventSystem.current.IsPointerOverGameObject() && !ToggleConstructionMenu.isOpen)
        {
            ShootCurrentSpell();
            lastShotTime = Time.time; // Update the last shot time
            
        }
        /*
        if (Input.GetMouseButton(0) && Time.time - lastShotTime >= shootingCooldown && !EventSystem.current.IsPointerOverGameObject() && ToggleConstructionMenu.isOpen)
        {
            UICannotAttack.SetActive(true);
        }*/

    }

    private void ShootCurrentSpell()
    {
        GameObject spellPrefab = null;
        AudioClip spellSound = null;

        // Determine which spell prefab to use based on the currently selected power
        switch (currentPower)
        {
            case PowerType.Fire:
                spellPrefab = fireboltPrefab;
                spellSound = fireboltSound;
                break;
            case PowerType.Water:
                spellPrefab = waterSplashPrefab;
                spellSound = waterSplashSound;
                break;
            case PowerType.Earth:
                spellPrefab = earthQuakePrefab;
                spellSound = earthQuakeSound;
                break;
        }

        if (spellPrefab)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                // Calculate the direction from the player to the mouse cursor's raycast hit point
                Vector3 direction = (hit.point - transform.position).normalized;

                // Face the player in the direction of the shot
                transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

                // Set the spawn position to be 1 unit in front of the player in the direction of the raycast hit
                Vector3 spawnPosition = transform.position + direction;
                spawnPosition.y = transform.position.y + 1; // Keep Y position constant

                // Instantiate the spell prefab at the spawn position
                GameObject spell = Instantiate(spellPrefab, spawnPosition, Quaternion.LookRotation(direction));

                // Set the velocity to move the spell, ensuring Y component is 0 for constant height
                Rigidbody spellRb = spell.GetComponent<Rigidbody>();
                if (spellRb != null)
                {
                    spellRb.velocity = new Vector3(direction.x, 0, direction.z) * spellSpeed;
                }
                else
                {
                    Debug.LogError("Spell prefab does not have a Rigidbody component.");
                }

                // Play the spell sound effect
                if (spellSound != null)
                {
                    audioSource.PlayOneShot(spellSound);
                }
                agent.isStopped = true;
                animator.SetBool("IsCasting", true);

                StartCoroutine(ResetIsCastingAndMovement());
            }
        }
        IEnumerator ResetIsCastingAndMovement()
        {
            rb.constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ);
            // Wait for the duration of the casting animation or a fixed time
            yield return new WaitForSeconds(0.1f); // Adjust according to your animation's length

            animator.SetBool("IsCasting", false);

            // Resume player movement
            if (agent != null)
            {
                //agent.isStopped = false;
            }

            // Unfreeze the player's position on the X and Z axes
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
    }
}