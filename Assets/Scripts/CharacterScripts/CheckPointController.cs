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

        CameraController cam;
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

        public void SaveProgress()
        {
            //PlayerValues.lastCheckpointPosition = transform.position;
            //PlayerManager.Instance.playerData.CheckPointSave(); // Save the player's data when they reach a checkpoint
            PlayerPrefs.SetString("LastCheckpointRoomID", checkpointID); // oda kaydı
            PlayerPrefs.Save();
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
                            SaveProgress(); // Save the player's progress when they pause

                        }
                    }
                }
            }
        }
    }
}