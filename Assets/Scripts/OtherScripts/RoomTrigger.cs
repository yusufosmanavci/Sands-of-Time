using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public GameObject roomToActivate; // Reference to the room GameObject that will be activated when the player enters the trigger
    public GameObject roomToDeactivate;
    public Transform entryTarget;
    EnemySpawner enemySpawner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            roomToActivate.SetActive(true); // Activate the new room

            enemySpawner = roomToActivate.GetComponentInChildren<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.ClearEnemies(); // Clear any existing enemies in the new room
                enemySpawner.SpawnEnemies(); // Spawn new enemies in the new room
            }

            if (roomToDeactivate != null)
            {
                enemySpawner = roomToDeactivate.GetComponentInChildren<EnemySpawner>();
                if (enemySpawner != null)
                {
                    enemySpawner.ClearEnemies(); // Clear any existing enemies in the old room
                }
                roomToDeactivate.SetActive(false); // Deactivate the old room if it exists
            }
            // Kamera sýnýrýný güncelle
            BoxCollider2D box = roomToActivate.GetComponentInChildren<BoxCollider2D>();
            Vector2 roomMin = box.bounds.min;
            Vector2 roomMax = box.bounds.max;

            CameraController cam = Camera.main.GetComponent<CameraController>();
            cam.SetBounds(roomMin, roomMax); // Set camera bounds to the new room

            // Oyuncuyu hedef noktaya yerleþtir
            if (entryTarget != null)
                other.transform.position = entryTarget.position;
        }
    }

}
