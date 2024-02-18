using UnityEngine;
using TMPro;

public class DisplayEKey : MonoBehaviour
{
    public GameObject pressEText;
    private GameObject textObj;
    public bool isPlayerNear = false;
    private ResourceGathering resourceGathering;


    private void Start()
    {
        resourceGathering = GetComponent<ResourceGathering>(); // Assume this script is on the same GameObject
    }

    private void Update()
    {
        // Check if the resource is depleted
        bool isResourceDepleted = resourceGathering != null && resourceGathering.IsDepleted;
        
        // Display "Press E" text if the player is near and the resource is not depleted
        if (isPlayerNear && !isResourceDepleted && !pressEText.activeInHierarchy)
        {
            Debug.Log("activate E text");
            pressEText.SetActive(true);
        }
        else if (isResourceDepleted || !isPlayerNear) // Hide if the resource is depleted or player is not near
        {
            pressEText.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            pressEText.SetActive(false); // Hide "Press E" text when the player leaves
            Debug.Log("player left");
        }
    }
}