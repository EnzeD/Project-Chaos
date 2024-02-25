using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Add this for LINQ support

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;

    public GameObject pressECue; // Assign the "Press E" UI element in the inspector
    private readonly HashSet<Collectible> nearbyCollectibles = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Remove any null references from the list of nearby collectibles
        nearbyCollectibles.RemoveWhere(collectible => collectible == null);

        if (nearbyCollectibles.Count > 0)
        {
            pressECue.SetActive(true);
        }
        else
        {
            pressECue.SetActive(false);
        }
    }

    public void RegisterCollectible(Collectible collectible)
    {
        nearbyCollectibles.Add(collectible);
    }

    public void UnregisterCollectible(Collectible collectible)
    {
        if (collectible != null) // Check if the collectible reference is not null before removing
        {
            nearbyCollectibles.Remove(collectible);
        }
    }
}