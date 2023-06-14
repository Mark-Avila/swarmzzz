using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] swarms;
    [Tooltip("Initial No. of Enemies per SP"), SerializeField] private int perWave = 1;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private ItemSpawnManager itemSpawn;

    private Camera mainCamera;
    private int numberOfEnemies;
    private int waveNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SetNumberOfEnemies(perWave);
        enemiesText.SetText($"Enemies: {numberOfEnemies}");
        waveText.SetText($"Wave: {waveNumber}");

        SpawnEnemies(perWave);
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
        perWave += waveNumber;
        itemSpawn.SetMaxItemsCount(itemSpawn.GetMaxItemsCount() + 1);
        itemSpawn.SpawnItems();
        SetNumberOfEnemies(perWave);
        SpawnEnemies(perWave);

        Debug.Log($"Enemies in wave {waveNumber}: {numberOfEnemies}");
    }

    public void SetNumberOfEnemies(int count)
    {
        int totalSpawnPoints = transform.childCount;
        numberOfEnemies = count <= 0 ? totalSpawnPoints : count * totalSpawnPoints;
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
        if (waveNumber < 2)
        {
            // Only allow index 0 if waveNumber is less than 2
            return 0;
        }
        else if (waveNumber < 4)
        {
            // Allow indices 0 and 1 if waveNumber is less than 4
            return Random.Range(0, 1);
        }
        else
        {
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

    private void SpawnEnemies(int count)
    {
        foreach (Transform spawnPoint in transform)
        {
            Vector2 spawnPointPosition = spawnPoint.position;
            Vector2 screenPoint = mainCamera.WorldToViewportPoint(spawnPointPosition);

            if (screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
                continue;

            int swarmIndex = GetSwarmIndex();
            GameObject newSwarm = Instantiate(swarms[waveNumber >= 2 ? swarmIndex : 0], spawnPointPosition, Quaternion.identity);
            newSwarm.GetComponent<EnemySwarm>().SetNumberOfEnemies(count);
        }
    }
}
