using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject clickEffectPrefab;
    NavMeshAgent agent;

    // Ressource extraction variables
    public float harvestRate = 0.5f; // Rate at which player can harvest ressources
    private float lastHarvestTime = 0f; // When the player last harvested ressources
    private ResourceGathering targetedObject; // Currently targeted objetd
    public ResourceData resourceData;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // Moving the player
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Instantiate(clickEffectPrefab, hitInfo.point + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0));
                agent.SetDestination(hitInfo.point);
            }
        }
        bool isMoving = !agent.pathPending && agent.remainingDistance > agent.stoppingDistance;
        
        // Gathering ressources
        if (Input.GetKey(KeyCode.E) && targetedObject != null && Time.time - lastHarvestTime >= harvestRate)
        {
            bool success = targetedObject.ExtractRessource(1); // Attempt to extract 1 unit of ressource
            if (success)
            {
                lastHarvestTime = Time.time;
                if (targetedObject.CompareTag("FireRock"))
                {
                    resourceData.AddFire(1); // Update total fire
                }
            }
        }
    }
    // Detect if the player is within range of a fire rock
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FireRock")) // Make sure your rock prefab has the "FireRock" tag
        {
            Debug.Log("collide");
            targetedObject = other.gameObject.GetComponent<ResourceGathering>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FireRock"))
        {
            targetedObject = null; // Player is no longer near the rock
        }
    }
}
