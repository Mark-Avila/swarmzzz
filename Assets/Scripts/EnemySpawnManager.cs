using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject swarm;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        InvokeRepeating(nameof(SpawnEnemies), 20f, 20f);
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        foreach (Transform spawnPoint in transform)
        {
            Vector2 spawnPointPosition = spawnPoint.position;
            Vector2 screenPoint = mainCamera.WorldToViewportPoint(spawnPointPosition);

            if (screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
                continue;

            GameObject newSwarm = Instantiate(swarm, spawnPointPosition, Quaternion.identity);
            newSwarm.GetComponent<ZombieSwarm>().SetNumberOfEnemies(2);
        }
    }
}
