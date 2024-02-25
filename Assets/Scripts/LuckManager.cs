using System.Collections;
using UnityEngine;

public class LuckManager : MonoBehaviour
{
    public ResourceData resourceData;
    public LightingManager lightingManager;

    // Monster waves
    public GameObject resourcePrefab;
    public int minPrefab = 1;
    public int maxPrefab = 5;
    public float spawnRadius = 20f;

    public GameObject player;

    public void SpawnResources()
    {
        int resourceToSpawn = Random.Range(minPrefab, maxPrefab + 1);
        for (int i = 0; i < resourceToSpawn; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere.normalized * spawnRadius;
            randomDirection.y = 0; // Ensure they spawn on the ground level
            Vector3 spawnPosition = player.transform.position + randomDirection;

            Instantiate(resourcePrefab, spawnPosition, Quaternion.identity);
            resourceData.IncreaseLuck(1);
        }
    }
}