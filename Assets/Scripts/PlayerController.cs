using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject clickEffectPrefab;
    NavMeshAgent agent;
    Animator animator;

    private float startTime;
    private float estimatedTimeOfTravel;
    private bool isTraveling;

    // Ressource extraction variables
    public float harvestRate = 0.5f; // Rate at which player can harvest ressources
    private float lastHarvestTime = 0f; // When the player last harvested ressources
    private ResourceGathering targetedObject; // Currently targeted objetd
    public ResourceData resourceData;
    public ChaosManager chaosManager;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        chaosManager = FindObjectOfType<ChaosManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Moving the player
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !ToggleConstructionMenu.isOpen)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                agent.isStopped = false; // Ensure the agent is allowed to move towards the new destination
                Instantiate(clickEffectPrefab, hitInfo.point + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0));
                agent.SetDestination(hitInfo.point);

                // Calculate estimated time of travel
                float distance = Vector3.Distance(transform.position, hitInfo.point);
                estimatedTimeOfTravel = distance / agent.speed;

                // Mark the start time and position
                startTime = Time.time;
                isTraveling = true;
            }
        }

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && !ToggleConstructionMenu.isOpen)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                agent.SetDestination(hit.point);
                agent.isStopped = false; // Ensure the agent is allowed to move
            }
        }

        animator.SetBool("IsRunning", agent.velocity.magnitude > 5f);

        // Track real time of travel and compare with the estimated time
        if (isTraveling)
        {
            float realTimeOfTravel = Time.time - startTime;
            if (realTimeOfTravel > 1.2f*estimatedTimeOfTravel)
            {
                // Stop the agent if real time exceeds the estimated time
                agent.isStopped = true;
                animator.SetBool("IsRunning", false);
                isTraveling = false; // Reset travel flag
            }
            else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                // Mark travel as complete if the destination is reached
                isTraveling = false;
            }
        }


        //Debug.Log("velocity:" + agent.velocity.magnitude);

        // Gathering ressources
        if (Input.GetKey(KeyCode.E) && targetedObject != null && Time.time - lastHarvestTime >= harvestRate)
        {
            if (targetedObject.CompareTag("FireRock"))
            {
                if (targetedObject.ExtractRessource(1))
                {
                    lastHarvestTime = Time.time;
                    resourceData.AddFire(1); // Update total fire
                }
            }
            if (targetedObject.CompareTag("ChaosSeed"))
            {
                if (targetedObject.ExtractRessource(10))
                {
                    lastHarvestTime = Time.time;
                    resourceData.ReduceChaos(10); // Update total Chaos

                    // Trigger wave spawn from ChaosManager
                    if (chaosManager != null)
                    {
                        chaosManager.SpawnWave(); // Assuming SpawnWave is the method you wish to call
                    }

                }
            }
        }
    }
    // Detect if the player is within range of a collectible
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FireRock") || other.gameObject.CompareTag("ChaosSeed"))
        {
            targetedObject = other.gameObject.GetComponent<ResourceGathering>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FireRock") || other.gameObject.CompareTag("ChaosSeed"))
        {
            targetedObject = null; // Player is no longer near the rock
        }
    }
}
