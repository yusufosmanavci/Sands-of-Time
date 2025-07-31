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
        public GameObject checkpointRoom; // Unique identifier for the checkpoint
        private bool IsInCheckpointArea = false; // Flag to check if the player is in the checkpoint area
        public static bool hasButtonPressed = false; // Flag to check if the button has been pressed
        public RoomData roomData;
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

        public void SaveProgress()
        {
            PlayerManager.Instance.playerValues.lastCheckPoint = roomData;
            PlayerManager.Instance.playerValues.checkPointRoom = checkpointRoom;
            //PlayerValues.lastCheckpointPosition = transform.position;
            //PlayerManager.Instance.playerData.CheckPointSave(); // Save the player's data when they reach a checkpoint
            PlayerPrefs.SetInt("LastCheckpointRoomID", roomData.roomID); // oda kaydı
            PlayerPrefs.Save();
        }

        private void Update()
        {
            if (IsInCheckpointArea && PlayerManager.Instance.playerValues.IsGrounded)
            {
                if (InputManager.Instance.MenuOpenCloseInput && !hasButtonPressed)
                {
                    hasButtonPressed = true; // Set the flag to true to prevent multiple presses
                    CampfireUIController.Instance.IsPaused = true;
                    if (CampfireUIController.Instance.IsPaused)
                    {
                        CampfireUIController.Instance.Pause(); // Pause the game and open the campfire menu
                        PlayerManager.Instance.playerHealth.healPotions = 2; //potion ları yeniliyor.
                        PlayerManager.Instance.playerHealth.currentHealth = PlayerManager.Instance.playerHealth.maxHealth;
                        PlayerManager.Instance.playerHealthBar.SetHealth(PlayerManager.Instance.playerHealth.currentHealth);
                        SaveProgress(); // Save the player's progress when they pause

                    }
                }
                else if(!CampfireUIController.Instance.IsPaused)
                {
                    InputManager.ActivatePlayerControls(); // Enable player controls when the campfire menu is closed
                }
            }

            
        }
    }
}