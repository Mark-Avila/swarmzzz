using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] items;
    private Camera mainCamera;
    private PlayerWeapon playerWeapon;
    private int maxItemCount = 3;

    private void Start()
    {
        mainCamera = Camera.main;
        playerWeapon = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerWeapon>();
        SpawnItems();
    }

    public void SetMaxItemsCount(int newMaxItemCount)
    {
        if (maxItemCount >= 0)
            maxItemCount = newMaxItemCount;
    }

    public int GetMaxItemsCount()
    {
        return maxItemCount;
    }

    public void SpawnItems()
    {
        Transform[] spawnPoints = GetRandomizedSpawnPoints();

        int itemCount = Mathf.Min(maxItemCount, spawnPoints.Length);
        for (int i = 0; i < itemCount; i++)
        {
            Transform spawnPoint = spawnPoints[i];

            Vector2 spawnPointPosition = spawnPoint.position;
            Vector2 screenPoint = mainCamera.WorldToViewportPoint(spawnPointPosition);

            if (screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
                continue;

            // Check if an item already exists at the spawn point
            bool weaponExists = spawnPoint.GetComponentInChildren<WeaponPickup>() != null;
            if (weaponExists)
                continue;

            bool itemExists = spawnPoint.GetComponentInChildren<HealthPickup>() != null;
            if (itemExists)
                continue;

            int itemIndex = Random.Range(0, items.Length);
            GameObject newItem = Instantiate(items[itemIndex], spawnPointPosition, Quaternion.identity, spawnPoint);

            WeaponPickup isWeapon = newItem.GetComponent<WeaponPickup>();

            if (isWeapon != null)
                newItem.GetComponent<WeaponPickup>().SetPlayerWeapon(playerWeapon);
        }
    }

    private Transform[] GetRandomizedSpawnPoints()
    {
        List<Transform> spawnPointList = new List<Transform>();
        foreach (Transform spawnPoint in transform)
        {
            spawnPointList.Add(spawnPoint);
        }

        // Randomize the order of spawn points using Fisher-Yates shuffle algorithm
        int n = spawnPointList.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Transform temp = spawnPointList[k];
            spawnPointList[k] = spawnPointList[n];
            spawnPointList[n] = temp;
        }

        return spawnPointList.ToArray();
    }
}

