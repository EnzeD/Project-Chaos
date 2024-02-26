using System.Collections;
using UnityEngine;

public class ChaosManager : MonoBehaviour
{
    public ResourceData resourceData;
    public LightingManager lightingManager;
    private Coroutine chaosCoroutine = null;

    // Monster waves
    public GameObject monsterPrefab;
    public int minMonstersPerWave = 2;
    public int maxMonstersPerWave = 10;
    public float spawnRadius = 50f;

    public GameObject player;
    private SpawnManager spawnManager;

    private OffScreenIndicatorManager indicatorManager; // For offscreen indicators

    private void Start()
    {
        maxMonstersPerWave += minMonstersPerWave + resourceData.chaosLevel / 10;
        spawnManager = FindObjectOfType<SpawnManager>();
        indicatorManager = FindObjectOfType<OffScreenIndicatorManager>();
    }

    private void Update()
    {
        // Ensure references are not null
        if (resourceData == null || lightingManager == null) return;

        // Start or stop the chaos increment coroutine based on the time of day
        if (lightingManager.IsNight && chaosCoroutine == null)
        {
            chaosCoroutine = StartCoroutine(IncrementChaos());
        }
        else if (!lightingManager.IsNight && chaosCoroutine != null)
        {
            StopCoroutine(chaosCoroutine);
            chaosCoroutine = null;
        }
    }

    private IEnumerator IncrementChaos()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            resourceData.IncrementChaos();
        }
    }

    public void SpawnWave()
    {
        int monstersToSpawn = Random.Range(minMonstersPerWave, maxMonstersPerWave + 1);
        for (int i = 0; i < monstersToSpawn; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere.normalized * spawnRadius;
            randomDirection.y = 0; // Ensure they spawn on the ground level
            Vector3 spawnPosition = player.transform.position + randomDirection;

            GameObject newMonster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            indicatorManager.AddMonster(newMonster.transform);
            spawnManager.totalMonstersAlive++;
        }
    }
}