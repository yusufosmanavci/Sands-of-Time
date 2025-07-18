using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public GameObject roomToActivate; // Reference to the room GameObject that will be activated when the player enters the trigger
    public GameObject roomToDeactivate;
    public CinemachineConfiner2D cameraConfiner; // Reference to the CinemachineConfiner2D component for camera confinement
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

            if(roomToDeactivate != null)
                roomToDeactivate.SetActive(false); // Deactivate the old room if it exists

            // Kamera sýnýrýný güncelle
            Collider2D newBounds = roomToActivate.GetComponentInChildren<BoxCollider2D>();
            if (newBounds != null)
                cameraConfiner.BoundingShape2D = newBounds;

            // Oyuncuyu hedef noktaya yerleþtir
            if (entryTarget != null)
                other.transform.position = entryTarget.position;
        }
    }

}
