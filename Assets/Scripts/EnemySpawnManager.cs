using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] swarms;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private ItemSpawnManager itemSpawn;
    [Tooltip("Max initial number of Enemies"), SerializeField] private int maxEnemies = 3; 
    [Tooltip("Minimum initial number of Enemies"), SerializeField] private int minEnemies = 1;

    private Camera mainCamera;
    private int numberOfEnemies;
    private int waveNumber = 1;
    void Start()
    {
        mainCamera = Camera.main;
        enemiesText.SetText($"Enemies: {numberOfEnemies}");
        waveText.SetText($"Wave: {waveNumber}");

        SpawnEnemies(minEnemies, maxEnemies);

        Debug.Log($"Enemies in wave {waveNumber}: {numberOfEnemies}");
    }

    private void Update()
    {
        if (numberOfEnemies <= 0)
            NextWave();

        enemiesText.SetText($"Enemies: {numberOfEnemies}");
        waveText.SetText($"Wave: {waveNumber}");
    }

    private void NextWave()
    {
        waveNumber++;
        itemSpawn.SetMaxItemsCount(itemSpawn.GetMaxItemsCount() + 1);
        itemSpawn.SpawnItems();

        maxEnemies++;
        minEnemies++;

        SpawnEnemies(minEnemies, maxEnemies);

        Debug.Log($"Enemies in wave {waveNumber}: {numberOfEnemies}");
    }

    public void DecreaseEnemy()
    {
        numberOfEnemies--;
    }

    public int GetNumberOfEnemies()
    {
        return numberOfEnemies;
    }

    private int GetSwarmIndex()
    {
        if (waveNumber <= 2)
        {
            Debug.Log("First is true");
            // Only allow index 0 if waveNumber is less than 2
            return 0;
        }
        else if (waveNumber > 2 && waveNumber < 6)
        {
            Debug.Log("second is true");
            // Allow indices 0 and 1 if waveNumber is less than 4
            return Random.Range(0, 2);
        }
        else
        {
            Debug.Log("3rd is true");
            // Return the same way as before for waveNumber greater or equal to 4
            float totalProbability = 0f;
            for (int i = 0; i < swarms.Length; i++)
            {
                float probability = (waveNumber - 1) + (i + 1);
                totalProbability += probability;
            }

            float randomValue = Random.Range(0f, totalProbability);

            float cumulativeProbability = 0f;
            for (int i = 0; i < swarms.Length; i++)
            {
                float probability = (waveNumber - 1) + (i + 1);
                cumulativeProbability += probability;

                if (randomValue <= cumulativeProbability)
                    return i;
            }

            return swarms.Length - 1;
        }
    }

    private void SpawnEnemies(int min, int max)
    {
        int newNumberOfEnemies = 0;

        foreach (Transform spawnPoint in transform)
        {
            Vector2 spawnPointPosition = spawnPoint.position;
            Vector2 screenPoint = mainCamera.WorldToViewportPoint(spawnPointPosition);

            if (screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
                continue;

            int swarmIndex = GetSwarmIndex();
            GameObject newSwarm = Instantiate(swarms[waveNumber >= 2 ? swarmIndex : 0], spawnPointPosition, Quaternion.identity);
            int spawnNum = Random.Range(min, max);
            newSwarm.GetComponent<EnemySwarm>().SetNumberOfEnemies(spawnNum);
            newNumberOfEnemies += spawnNum;
        }

        numberOfEnemies = newNumberOfEnemies;
    }
}
