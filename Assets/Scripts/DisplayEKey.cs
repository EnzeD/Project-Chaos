using UnityEngine;
using TMPro;

public class DisplayEKey : MonoBehaviour
{
    public GameObject pressEText;
    public bool isPlayerNear = false;
    private float detectionRadius = 5f;
    private ResourceGathering resourceGathering;
    private GameObject player;


    private void Start()
    {
        resourceGathering = GetComponent<ResourceGathering>();
        player = GameObject.FindGameObjectWithTag("Player");
        pressEText.SetActive(false);
    }

    private void Update()
    {
        CheckIfPlayerIsNear(player);

        // Check if the resource is depleted
        bool isResourceDepleted = resourceGathering != null && resourceGathering.IsDepleted;
        
        // Display "Press E" text if the player is near and the resource is not depleted
        if (isPlayerNear && !isResourceDepleted)
        {
            Debug.Log("activate E");
            pressEText.SetActive(true);
        }
        else if (isResourceDepleted || !isPlayerNear) // Hide if the resource is depleted or player is not near
        {
            pressEText.SetActive(false);
        }

    }

    private bool CheckIfPlayerIsNear(GameObject player)
    {
        // Check the distance between the player and this collectible
        if (Vector3.Distance(player.transform.position, transform.position) <= detectionRadius)
        {
            // If within range
            isPlayerNear = true;
        }
        else
        {
            // If not in range
            isPlayerNear = false;
        }
        return isPlayerNear;
    }
}