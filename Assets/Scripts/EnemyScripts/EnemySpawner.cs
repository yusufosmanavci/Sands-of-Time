using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab of the enemy to spawn
    public Transform[] spawnPoints; // Array of spawn points where enemies can be spawned
    public Transform[] patrolZones; // Array of patrol points for the enemies

    private List<GameObject> currentEnemies = new List<GameObject>(); // List to keep track of active enemies

  

    public void SpawnEnemies()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if(enemyController != null)
            {
                Transform[] closestTwoPoints = GetClosestPatrolPoints(spawnPoint.position, 2);
                enemyController.SetPatrolZone(closestTwoPoints[0], closestTwoPoints[1]);
            }
            currentEnemies.Add(enemy); // Add the spawned enemy to the list
        }
    }

    private Transform[] GetClosestPatrolPoints(Vector3 position, int count)
    {
        Transform[] closestPoints = patrolZones
            .OrderBy(p => Vector3.Distance(position, p.position))
            .Take(count)
            .ToArray();
        return closestPoints;
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
