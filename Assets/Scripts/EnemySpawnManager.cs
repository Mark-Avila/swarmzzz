using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject swarm;
    [Tooltip("Initial No. of Enemies per SP"), SerializeField] private int perWave = 1;

    private Camera mainCamera;
    private int numberOfEnemies;
    private int waveNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        SetNumberOfEnemies(perWave);
        Debug.Log(numberOfEnemies);

        SpawnEnemies(perWave);
    }

    private void Update()
    {
        if (numberOfEnemies <= 0)
        {
            waveNumber++;
            perWave += waveNumber;
            SetNumberOfEnemies(perWave);
            SpawnEnemies(perWave);
        }
    }

    public void SetNumberOfEnemies(int count)
    {
        int totalSpawnPoints = transform.childCount;
        numberOfEnemies = count <= 0 ? totalSpawnPoints : count * totalSpawnPoints;
    }

    public void DecreaseEnemy()
    {
        numberOfEnemies--;
        Debug.Log(numberOfEnemies);
    }

    public int GetNumberOfEnemies()
    {
        return numberOfEnemies;
    }
    private void SpawnEnemies(int count)
    {
        foreach (Transform spawnPoint in transform)
        {
            Vector2 spawnPointPosition = spawnPoint.position;
            Vector2 screenPoint = mainCamera.WorldToViewportPoint(spawnPointPosition);

            if (screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
                continue;

            GameObject newSwarm = Instantiate(swarm, spawnPointPosition, Quaternion.identity);
            newSwarm.GetComponent<EnemySwarm>().SetNumberOfEnemies(count);
        }
    }
}
