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

    private float minEnemyVariantChance = 0.5f;
    private float enemyVariantChance = 0.7f;

    void Start()
    {
        mainCamera = Camera.main;
        waveText.SetText($"Wave: {waveNumber}");

        SpawnEnemies(minEnemies, maxEnemies);

        enemiesText.SetText($"Enemies: {numberOfEnemies}");
    }

    private void Update()
    {
        if (numberOfEnemies <= 0)
            NextWave();
    }

    private void NextWave()
    {
        waveNumber++;
        itemSpawn.SetMaxItemsCount(itemSpawn.GetMaxItemsCount() + 1);
        itemSpawn.SpawnItems();

        maxEnemies++;
        minEnemies++;

        if (waveNumber == 5)
            if (enemyVariantChance <= minEnemyVariantChance)
                enemyVariantChance -= 0.1f;

        if (waveNumber == 10)
            if (enemyVariantChance <= minEnemyVariantChance)
                enemyVariantChance -= 0.2f;

        SpawnEnemies(minEnemies, maxEnemies);

        waveText.SetText($"Wave: {waveNumber}");
        enemiesText.SetText($"Enemies: {numberOfEnemies}");
    }

    public void DecreaseEnemy()
    {
        numberOfEnemies--;
        enemiesText.SetText($"Enemies: {numberOfEnemies}");
    }

    public int GetNumberOfEnemies()
    {
        return numberOfEnemies;
    }

    private int GetSwarmIndex()
    {
        if (waveNumber <= 2)
        {
            // Only allow index 0 if waveNumber is less than 2
            return 0;
        }
        else if (waveNumber < 6)
        {
            // Allow indices 0 and 1 if waveNumber is less than 6
            return Random.Range(0, 2);
        }
        else
        {
            float rngSpawn = Random.value;

            if (rngSpawn <= enemyVariantChance)
            {
                Debug.Log("Zero is true");
                return 0;
            }

            Debug.Log("Chance is true");
            // Randomly select from the remaining indices (excluding index 0)
            return Random.Range(1, swarms.Length);
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
