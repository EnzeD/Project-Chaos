using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGathering : MonoBehaviour
{
    public int resourceAmount = 20; // Total amount of resource in the object
    public bool IsDepleted = false;
    private Collectible collectible;

    public AudioClip extractionSound;
    private AudioSource audioSource;

    private void Start()
    {
        collectible = GetComponent<Collectible>();
        if (!TryGetComponent<AudioSource>(out audioSource)) // If there's no AudioSource component, add one.
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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
        resourceAmount -= amountToExtract; // Subtract the amount of resources extracted
        resourceAmount = Mathf.Max(resourceAmount, 0); // Ensure resourceAmount doesn't go below 0
        Debug.Log("Extracted resource. Remaining: " + resourceAmount);
        GetComponent<ResourceCollector>().DisplayFloatingText();

        // Play the extraction sound effect
        if (extractionSound != null && audioSource != null)
        {
            AudioManager.Instance.PlaySFX(extractionSound);
        }

        if (resourceAmount <= 0)
        {
            IsDepleted = true;
        }
        return true; // Indicate that resource was successfully extracted
    }
}
