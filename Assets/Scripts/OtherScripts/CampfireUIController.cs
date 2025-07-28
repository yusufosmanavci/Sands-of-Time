using Assets.Scripts.CharacterScripts;
using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.OtherScripts
{
    public class CampfireUIController : MonoBehaviour
    {
        public static CampfireUIController Instance;
        public TextMeshProUGUI currMaxHealth;
        public TextMeshProUGUI lvlUpMaxHealth;
        public TextMeshProUGUI currDamage;
        public TextMeshProUGUI lvlUpDamage;
        public TextMeshProUGUI sandsOfTime;
        public GameObject campfireCanvas;
        public GameObject characterUpgradeCanvas;
        public GameObject healthBar;
        public GameObject dashSkill;

        public bool IsPaused;

        public Button upgradeButton;
        public Button levelUpButton;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        private void Start()
        {
            campfireCanvas.SetActive(false);
        }
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
            InputManager.DeactivatePlayerControls(); // Disable player controls when the campfire menu is open
            EventSystem.current.SetSelectedGameObject(null); // Deselect any UI element
            EventSystem.current.SetSelectedGameObject(upgradeButton.gameObject); // Set focus on the campfire text
        }
        public void OpenCharacterUpgradeMenu()
        {
            campfireCanvas.SetActive(false);
            characterUpgradeCanvas.SetActive(true); // Show the character upgrade canvas
            healthBar.SetActive(false); // Hide the health bar
            dashSkill.SetActive(false); // Hide the dash skill

            sandsOfTime.text = PlayerManager.Instance.playerValues.sandsOfTimeUpgrade.ToString();
            currMaxHealth.text = PlayerManager.Instance.playerHealth.maxHealth.ToString();
            lvlUpMaxHealth.text = (PlayerManager.Instance.playerHealth.maxHealth * 0.2f + PlayerManager.Instance.playerHealth.maxHealth).ToString();
            currDamage.text = PlayerManager.Instance.playerValues.playerDamage.ToString();
            lvlUpDamage.text = (PlayerManager.Instance.playerValues.playerDamage * 0.2f + PlayerManager.Instance.playerValues.playerDamage).ToString();

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
            CheckPointController.hasButtonPressed = false; // Reset the flag when the button is released
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
            if(int.TryParse(sandsOfTime.text, out PlayerManager.Instance.playerValues.sandsOfTimeUpgrade) && PlayerManager.Instance.playerValues.sandsOfTimeUpgrade <= PlayerManager.Instance.playerValues.sandsOfTime)
            {
                PlayerManager.Instance.playerValues.sandsOfTime -= PlayerManager.Instance.playerValues.sandsOfTimeUpgrade;
                sandsOfTime.text = (PlayerManager.Instance.playerValues.sandsOfTimeUpgrade * 5f + PlayerManager.Instance.playerValues.sandsOfTimeUpgrade).ToString();
                if (float.TryParse(lvlUpMaxHealth.text, out PlayerManager.Instance.playerHealth.maxHealth))
                {
                    currMaxHealth.text = lvlUpMaxHealth.text;
                    lvlUpMaxHealth.text = (PlayerManager.Instance.playerHealth.maxHealth * 0.2f + PlayerManager.Instance.playerHealth.maxHealth).ToString();
                    PlayerManager.Instance.playerHealth.currentHealth = PlayerManager.Instance.playerHealth.maxHealth;
                    PlayerManager.Instance.playerHealthBar.SetMaxHealth(PlayerManager.Instance.playerHealth.maxHealth);
                }
                if (float.TryParse(lvlUpDamage.text, out PlayerManager.Instance.playerValues.playerDamage))
                {
                    currDamage.text = lvlUpDamage.text;
                    lvlUpDamage.text = (PlayerManager.Instance.playerValues.playerDamage * 0.2f + PlayerManager.Instance.playerValues.playerDamage).ToString();
                }
            }
            else
            {
                //SandsOfTime Amount is not enough
                Debug.Log("SandsOfTime Amount is not enough");
            }
        }

        public void OnBackPressed()
        {
            CloseCharacterUpgradeMenu();
        }
    }
}