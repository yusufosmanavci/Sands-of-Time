using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

namespace Assets.Scripts.CharacterScripts
{
    public class CheckPointController : MonoBehaviour
    {
        public string checkpointID; // Unique identifier for the checkpoint

        public GameObject roomToActivate;
        public List<GameObject> roomsToDeactivate; // List of rooms to activate when the player reaches a checkpoint
        public CinemachineConfiner2D cameraConfiner; // Reference to the CinemachineConfiner2D component for camera confinement
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerValues.lastCheckpointPosition = transform.position;
                PlayerManager.Instance.playerData.CheckPointSave(); // Save the player's data when they reach a checkpoint
                PlayerPrefs.SetString("LastCheckpointRoomID", checkpointID); // oda kaydı
                PlayerPrefs.Save();
            }
        }
        private void Update()
        {
            if (PlayerManager.Instance.playerValues.IsDead)
            {
                PlayerManager.Instance.playerValues.rb.linearVelocity = Vector2.zero; // Stop player movement when dead
                roomToActivate.SetActive(true); // Activate the new room when the player reaches a checkpoint
                if (roomsToDeactivate != null)
                {
                    foreach (GameObject room in roomsToDeactivate)
                    {
                        if (room != null)
                        {
                            EnemySpawner enemySpawner = room.GetComponentInChildren<EnemySpawner>();
                            if (enemySpawner != null)
                            {
                                enemySpawner.ClearEnemies(); // Clear any existing enemies in the old room
                            }
                            room.SetActive(false); // Deactivate the old room
                        }
                    }
                }
                cameraConfiner.BoundingShape2D = roomToActivate.GetComponentInChildren<BoxCollider2D>(); // Update camera confinement to the new room
                PlayerManager.Instance.playerValues.IsDead = false; // Reset the dead state when reaching a checkpoint
                PlayerManager.Instance.playerHealth.currentHealth = PlayerManager.Instance.playerHealth.maxHealth; // Restore health to maximum
                PlayerManager.Instance.playerHealthBar.SetMaxHealth(PlayerManager.Instance.playerHealth.maxHealth); // Update health bar UI

            }
        }
    }
}