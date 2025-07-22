using Assets.Scripts.OtherScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CharacterScripts
{
    public class CheckPointController : MonoBehaviour
    {
        public TextMeshPro campfireText;
        public string checkpointID; // Unique identifier for the checkpoint
        private bool IsInCheckpointArea = false; // Flag to check if the player is in the checkpoint area

        public GameObject roomToActivate;
        public List<GameObject> roomsToDeactivate; // List of rooms to activate when the player reaches a checkpoint
        public CinemachineConfiner2D cameraConfiner; // Reference to the CinemachineConfiner2D component for camera confinement
        public CampfireUIController campfire;
        private void Start()
        {
            campfireText.alpha = 0f;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                IsInCheckpointArea = true; // Set the flag to true when the player enters the checkpoint area
                campfireText.alpha = 1f; // Show the campfire text when the player enters the checkpoint area
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                IsInCheckpointArea = false; // Set the flag to false when the player exits the checkpoint area
                campfireText.alpha = 0f; // Hide the campfire text when the player exits the checkpoint area
            }
        }

        private void Update()
        {
            if (IsInCheckpointArea)
            {
                if (InputManager.Instance.MenuOpenCloseInput)
                {
                    if (campfire != null)
                    {
                        campfire.IsPaused = true;
                        if (campfire.IsPaused)
                        {
                            campfire.Pause(); // Pause the game and open the campfire menu
                        }
                    }
                }
            }
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

                PlayerValues.lastCheckpointPosition = transform.position;
                PlayerManager.Instance.playerData.CheckPointSave(); // Save the player's data when they reach a checkpoint
                CheckPointController checkpointController = GetComponentInParent<CheckPointController>();
                PlayerPrefs.SetString("LastCheckpointRoomID", checkpointController.checkpointID); // oda kaydı
                PlayerPrefs.Save();
            }
        }
    }
}