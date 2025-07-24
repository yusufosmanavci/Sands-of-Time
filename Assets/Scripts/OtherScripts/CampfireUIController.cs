using Assets.Scripts.CharacterScripts;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.OtherScripts
{
    public class CampfireUIController : MonoBehaviour
    {
        
        public GameObject campfireCanvas;
        public GameObject characterUpgradeCanvas;
        public GameObject healthBar;
        public GameObject dashSkill;

        public bool IsPaused;

        public Button upgradeButton;
        public Button levelUpButton;

        public void Pause()
        {
            IsPaused = true;
            Time.timeScale = 0f; // Stop time

            OpenCampfireMenu();
        }
        

        public void Unpause()
        {
            IsPaused = false;
            Time.timeScale = 1f; // Resume time
            CloseCampfireMenu();
        }

        public void OpenCampfireMenu()
        {
            campfireCanvas.SetActive(true);
            characterUpgradeCanvas.SetActive(false); // Ensure the upgrade canvas is hidden initially
            healthBar.SetActive(false); // Hide the health bar
            dashSkill.SetActive(false); // Hide the dash skill
            EventSystem.current.SetSelectedGameObject(null); // Deselect any UI element
            EventSystem.current.SetSelectedGameObject(upgradeButton.gameObject); // Set focus on the campfire text
        }
        public void OpenCharacterUpgradeMenu()
        {
            campfireCanvas.SetActive(false);
            characterUpgradeCanvas.SetActive(true); // Show the character upgrade canvas
            healthBar.SetActive(false); // Hide the health bar
            dashSkill.SetActive(false); // Hide the dash skill
            EventSystem.current.SetSelectedGameObject(null); // Deselect any UI element
            EventSystem.current.SetSelectedGameObject(levelUpButton.gameObject); // Set focus on the level up button
        }

        public void CloseCharacterUpgradeMenu()
        {
            characterUpgradeCanvas.SetActive(false);
            campfireCanvas.SetActive(true); // Show the campfire canvas when closing the upgrade menu
            healthBar.SetActive(false); // Hide the health bar
            dashSkill.SetActive(false); // Hide the dash skill
            EventSystem.current.SetSelectedGameObject(null); // Deselect any UI element
            EventSystem.current.SetSelectedGameObject(upgradeButton.gameObject); // Set focus on the resume button
        }

        public void CloseCampfireMenu()
        {
            campfireCanvas.SetActive(false);
            characterUpgradeCanvas.SetActive(false); // Hide the upgrade canvas when closing the campfire menu
            healthBar.SetActive(true); // Hide the health bar
            dashSkill.SetActive(true); // Hide the dash skill
            EventSystem.current.SetSelectedGameObject(null); // Deselect any UI element
        }

        public void OnResumePressed()
        {
            Unpause();
        }

        public void OnUpgradePressed()
        {
            OpenCharacterUpgradeMenu(); // Open the character upgrade menu
        }

        public void OnLevelUpPressed()
        {
            //Level up logic here
        }

        public void OnBackPressed()
        {
            CloseCharacterUpgradeMenu();
        }
    }
}