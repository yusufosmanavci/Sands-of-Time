using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab of the enemy to spawn
    public Transform[] spawnPoints; // Array of spawn points where enemies can be spawned

    private List<GameObject> currentEnemies = new List<GameObject>(); // List to keep track of active enemies

    private void Start()
    {
        SpawnEnemies(); // Spawn enemies at the start
    }

    public void SpawnEnemies()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            currentEnemies.Add(enemy); // Add the spawned enemy to the list
        }
    }

    public void ClearEnemies()
    {
        foreach (GameObject enemy in currentEnemies)
        {
            if (enemy != null)
                Destroy(enemy); // Destroy each enemy in the list
        }
        currentEnemies.Clear(); // Clear the list after destroying enemies
    }
}
