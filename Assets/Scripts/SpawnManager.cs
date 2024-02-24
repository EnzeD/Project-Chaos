using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnWave
{
    public GameObject monsterPrefab;
    public int numberOfMonsters;
}

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<SpawnWave> spawnWaves = new List<SpawnWave>();
    public float spawnRadiusMax = 100f; // Distance from the spawn point
    public float spawnRadiusMin = 80f; // Distance from the spawn point
    public Transform spawnCenter;
    public LightingManager lightingManager;

    private bool spawnedForTonight = false;
    private int currentWaveIndex = 0; // Track the current wave index
    public int totalMonstersAlive = 0;

    void Update()
    {
        if (lightingManager.IsNight && !spawnedForTonight)
        {
            // Check if we still have waves to spawn
            if (currentWaveIndex < spawnWaves.Count)
            {
                StartCoroutine(SpawnWave(spawnWaves[currentWaveIndex]));
                currentWaveIndex++; // Prepare the index for the next wave
            }
            spawnedForTonight = true; // Ensure we only spawn once per night
            // Start playing night music
            AudioManager.Instance.PlayNightMusic();
        }
        else if (!lightingManager.IsNight)
        {
            spawnedForTonight = false; // Reset for the next night
            // Start playing daylight music as all monsters are dead
        }
    }

    IEnumerator SpawnWave(SpawnWave wave)
    {
        totalMonstersAlive += wave.numberOfMonsters; // Increase total monsters count
        for (int i = 0; i < wave.numberOfMonsters; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere.normalized;
            float randomDistance = Random.Range(spawnRadiusMin, spawnRadiusMax);
            Vector3 spawnPosition = spawnCenter.position + randomDirection * randomDistance;
            spawnPosition.y = spawnCenter.position.y;

            Instantiate(wave.monsterPrefab, spawnPosition, Quaternion.identity);

            yield return null;
        }
    }
    public void MonsterDied()
    {
        totalMonstersAlive--;
        if (totalMonstersAlive == 0)
        {
            AudioManager.Instance.PlayDayMusic();
        }

    }
}