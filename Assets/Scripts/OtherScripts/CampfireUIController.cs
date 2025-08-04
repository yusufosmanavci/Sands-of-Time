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

        [Header("Texts")]
        public TextMeshProUGUI currMaxHealth;
        public TextMeshProUGUI lvlUpMaxHealth;
        public TextMeshProUGUI currDamage;
        public TextMeshProUGUI lvlUpDamage;
        public TextMeshProUGUI currDashDamage;
        public TextMeshProUGUI lvlUpDashDamage;
        public TextMeshProUGUI sandsOfTime;
        public TextMeshProUGUI levelText;

        [Header("GameObjects")]
        public GameObject campfireCanvas;
        public GameObject characterUpgradeCanvas;
        public GameObject healthBar;
        public GameObject dashSkill;
        public GameObject SandsOfTimeBackground;
        public GameObject SandsOfTimeText;
        public GameObject maxLvlText;
        public GameObject healPotion;


        [Header("Buttons")]
        public Button upgradeButton;
        public Button levelUpButton;

        public bool IsPaused;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

           // GetComponent<Button>().onClick.AddListener(() => { });
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
            healPotion.SetActive(false);
            InputManager.DeactivatePlayerControls(); // Disable player controls when the campfire menu is open
            SetUIElement(upgradeButton.gameObject);
        }
        public void OpenCharacterUpgradeMenu()
        {
            campfireCanvas.SetActive(false);
            characterUpgradeCanvas.SetActive(true); // Show the character upgrade canvas
            healthBar.SetActive(false); // Hide the health bar
            dashSkill.SetActive(false); // Hide the dash skill
            healPotion.SetActive(false);
            SandsOfTimeBackground.SetActive(false);
            SandsOfTimeText.SetActive(false);

            levelText.text = PlayerManager.Instance.playerValues.PlayerLevel + "/10";

            sandsOfTime.text = PlayerManager.Instance.playerValues.sandsOfTimeUpgrade.ToString();
            currMaxHealth.text = PlayerManager.Instance.playerHealth.maxHealth.ToString();
            lvlUpMaxHealth.text = (PlayerManager.Instance.playerHealth.maxHealth * 0.2f + PlayerManager.Instance.playerHealth.maxHealth).ToString();
            currDamage.text = PlayerManager.Instance.playerValues.playerDamage.ToString();
            lvlUpDamage.text = (PlayerManager.Instance.playerValues.playerDamage * 0.2f + PlayerManager.Instance.playerValues.playerDamage).ToString();
            currDashDamage.text = PlayerManager.Instance.playerValues.playerDashDamage.ToString();
            lvlUpDashDamage.text = (PlayerManager.Instance.playerValues.playerDashDamage * 0.2f + PlayerManager.Instance.playerValues.playerDashDamage).ToString();

            SetUIElement(levelUpButton.gameObject);
        }

        public void CloseCharacterUpgradeMenu()
        {
            characterUpgradeCanvas.SetActive(false);
            campfireCanvas.SetActive(true); // Show the campfire canvas when closing the upgrade menu
            healthBar.SetActive(false); // Hide the health bar
            dashSkill.SetActive(false); // Hide the dash skill
            SetUIElement(upgradeButton.gameObject);
        }

        public void CloseCampfireMenu()
        {
            campfireCanvas.SetActive(false);
            characterUpgradeCanvas.SetActive(false); // Hide the upgrade canvas when closing the campfire menu
            healthBar.SetActive(true); // Hide the health bar
            dashSkill.SetActive(true); // Hide the dash skill
            healPotion.SetActive(true);
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
            if (PlayerManager.Instance.playerValues.sandsOfTimeUpgrade <= PlayerManager.Instance.playerValues.sandsOfTime && PlayerManager.Instance.playerValues.PlayerLevel < 10)
            {
                PlayerManager.Instance.playerValues.PlayerLevel++;
                levelText.text = PlayerManager.Instance.playerValues.PlayerLevel + "/10";

                PlayerManager.Instance.playerValues.sandsOfTime -= PlayerManager.Instance.playerValues.sandsOfTimeUpgrade;
                int newSandsOfTimeAmount = PlayerManager.Instance.playerValues.sandsOfTimeUpgrade * 2 + PlayerManager.Instance.playerValues.sandsOfTimeUpgrade;
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
                lvlUpDashDamage.text = newLvlUpDashDamage.ToString();
                PlayerManager.Instance.playerValues.playerDashDamage = newDashDamage;
            }
            else if (PlayerManager.Instance.playerValues.sandsOfTimeUpgrade >= PlayerManager.Instance.playerValues.sandsOfTime)
            {
                SandsOfTimeBackground.SetActive(true);
                SandsOfTimeText.SetActive(true);
                Debug.Log("SandsOfTime Amount is not enough");
            }
            else if (PlayerManager.Instance.playerValues.PlayerLevel >= 10)
            {
                SandsOfTimeBackground.SetActive(true);
                maxLvlText.SetActive(true);
            }
        }

        public void OnBackPressed()
        {
            CloseCharacterUpgradeMenu();
        }

        private void SetUIElement(GameObject element)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(element);
        }
    }
}
