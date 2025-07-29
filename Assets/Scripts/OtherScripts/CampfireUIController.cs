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
        public TextMeshProUGUI currDashDamage;
        public TextMeshProUGUI lvlUpDashDamage;
        public TextMeshProUGUI sandsOfTime;
        public GameObject campfireCanvas;
        public GameObject characterUpgradeCanvas;
        public GameObject healthBar;
        public GameObject dashSkill;
        public GameObject SandsOfTimeBackground;
        public GameObject SandsOfTimeText;

        public bool IsPaused;
        public float showTime;

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
            SandsOfTimeBackground.SetActive(false);
            SandsOfTimeText.SetActive(false);

            sandsOfTime.text = PlayerManager.Instance.playerValues.sandsOfTimeUpgrade.ToString();
            currMaxHealth.text = PlayerManager.Instance.playerHealth.maxHealth.ToString();
            lvlUpMaxHealth.text = (PlayerManager.Instance.playerHealth.maxHealth * 0.2f + PlayerManager.Instance.playerHealth.maxHealth).ToString();
            currDamage.text = PlayerManager.Instance.playerValues.playerDamage.ToString();
            lvlUpDamage.text = (PlayerManager.Instance.playerValues.playerDamage * 0.2f + PlayerManager.Instance.playerValues.playerDamage).ToString();
            currDashDamage.text = PlayerManager.Instance.playerValues.playerDashDamage.ToString();
            lvlUpDashDamage.text = (PlayerManager.Instance.playerValues.playerDashDamage * 0.2f + PlayerManager.Instance.playerValues.playerDashDamage).ToString();

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
            if (PlayerManager.Instance.playerValues.sandsOfTimeUpgrade <= PlayerManager.Instance.playerValues.sandsOfTime)
            {
                PlayerManager.Instance.playerValues.sandsOfTime -= PlayerManager.Instance.playerValues.sandsOfTimeUpgrade;
                int newSandsOfTimeAmount = PlayerManager.Instance.playerValues.sandsOfTimeUpgrade * 5 + PlayerManager.Instance.playerValues.sandsOfTimeUpgrade;
                PlayerManager.Instance.playerValues.sandsOfTimeUpgrade = newSandsOfTimeAmount;
                sandsOfTime.text = newSandsOfTimeAmount.ToString();

                float newMaxHealth = PlayerManager.Instance.playerHealth.maxHealth * 0.2f + PlayerManager.Instance.playerHealth.maxHealth;
                currMaxHealth.text = newMaxHealth.ToString();
                float newLvlUpHealth = newMaxHealth * 0.2f + newMaxHealth;
                lvlUpMaxHealth.text = newLvlUpHealth.ToString();
                PlayerManager.Instance.playerHealth.maxHealth = newMaxHealth;
                PlayerManager.Instance.playerHealth.currentHealth = newMaxHealth;
                PlayerManager.Instance.playerHealthBar.SetMaxHealth(PlayerManager.Instance.playerHealth.maxHealth);

                float newPlayerDamage = PlayerManager.Instance.playerValues.playerDamage * 0.2f + PlayerManager.Instance.playerValues.playerDamage;
                currDamage.text = newPlayerDamage.ToString();
                float newLvlUpDamage = newPlayerDamage * 0.2f + newPlayerDamage;
                lvlUpDamage.text = newLvlUpDamage.ToString();
                PlayerManager.Instance.playerValues.playerDamage = newPlayerDamage;

                float newDashDamage = PlayerManager.Instance.playerValues.playerDashDamage * 0.2f + PlayerManager.Instance.playerValues.playerDashDamage;
                currDashDamage.text = newDashDamage.ToString();
                float newLvlUpDashDamage = newDashDamage * 0.2f + newDashDamage;
                lvlUpDashDamage.text= newLvlUpDashDamage.ToString();
                PlayerManager.Instance.playerValues.playerDashDamage = newDashDamage;
            }
            else
            {
                SandsOfTimeBackground.SetActive(true);
                SandsOfTimeText.SetActive(true);
                Debug.Log("SandsOfTime Amount is not enough");
            }
        }

        public void OnBackPressed()
        {
            CloseCharacterUpgradeMenu();
        }
    }
}
