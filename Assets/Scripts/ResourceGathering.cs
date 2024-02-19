using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGathering : MonoBehaviour
{
    public int resourceAmount = 20; // Total amount of resource in the object
    public bool IsDepleted = false;
    private Collectible collectible;

    private void Start()
    {
        collectible = GetComponent<Collectible>();
    }

    // Method to extract ressource from the object
    public void Update()
    {
        if (IsDepleted)
        {
            CollectibleManager.Instance.UnregisterCollectible(collectible);
            Destroy(gameObject); // Destroy the object since it's depleted
        }
            
    }
    public bool ExtractRessource(int amountToExtract)
    {
        if (resourceAmount <= 0)
        {
            // Optionally, destroy the rock or change its appearance when depleted
            Debug.Log("Rock is depleted of fire.");
            return false; // Indicate that no fire was extracted
        }

        resourceAmount -= amountToExtract; // Subtract the amount of fire extracted
        resourceAmount = Mathf.Max(resourceAmount, 0); // Ensure fireAmount doesn't go below 0
        Debug.Log("Extracted fire. Remaining: " + resourceAmount);
        GetComponent<ResourceCollector>().DisplayFloatingText();

        if (resourceAmount <= 0)
        {
            Debug.Log("Object is depleted of resource. Destroying object.");
            IsDepleted = true;
        }
        return true; // Indicate that fire was successfully extracted
    }
}
