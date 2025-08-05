using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public List<GameObject> boss = new List<GameObject> ();
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;


    private void Update()
    {
        if(boss.Count != 0)
        {
            return;
        }
        else
        {
            Instantiate(bossPrefab,bossSpawnPoint.position, Quaternion.identity);
            boss.Add(bossPrefab);
        }
    }

    public void ClearBosses()
    {
        foreach (GameObject enemy in boss)
        {
            if (enemy != null)
                Destroy(enemy); // Destroy each enemy in the list
        }
        boss.Clear(); // Clear the list after destroying enemies
    }
}
