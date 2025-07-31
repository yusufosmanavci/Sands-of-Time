using Assets.Scripts.UIScripts;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.BossScripts
{
    public class BossHealth : MonoBehaviour
    {
        public float bossMaxHealth = 1000f; // Maximum health of the boss
        public float bossCurrentHealth; // Current health of the boss
        BossValues bossValues;
        BossController bossController;
        public PlayerValues playerValues;
        BossSpellCasting bossSpellCasting;

        public BossHealthBar bossHealthBar;
        public GameObject BossCanvas;
        public GameObject[] doors;

        // Use this for initialization
        void Start()
        {
            bossCurrentHealth = bossMaxHealth; // Initialize current health to maximum health at the start
            bossValues = GetComponent<BossValues>();
            bossController = GetComponent<BossController>();
            playerValues = FindFirstObjectByType<PlayerValues>();
            bossHealthBar.SetMaxHealth(bossMaxHealth); // Set the maximum health in the health bar UI
            bossSpellCasting = GetComponentInChildren<BossSpellCasting>();
        }

        private void Update()
        {
            if (!bossValues.IsDead)
            {
                foreach (var door in doors)
                    door.SetActive(false);
            }
            else
            {
                foreach (var door in doors)
                    door.SetActive(true);
            }
        }

        public void TakeBossDamage(float damage)
        {
            // Reduce current health by the damage amount
            bossCurrentHealth -= damage;
            bossHealthBar.SetHealth(bossCurrentHealth); // Update the health bar UI

            // Ensure current health does not drop below zero
            if (bossCurrentHealth < 0)
            {
                bossCurrentHealth = 0;
            }
            // Optionally, you can add logic to handle player death here
            if (bossCurrentHealth == 0)
            {
                StartCoroutine(Die()); // Call the Die method to handle enemy death
            }
        }

        public void TakeBossDashDamage(float damage)
        {
            // Reduce current health by the damage amount
            bossCurrentHealth -= damage;
            bossHealthBar.SetHealth(bossCurrentHealth); // Update the health bar UI

            // Ensure current health does not drop below zero
            if (bossCurrentHealth < 0)
            {
                bossCurrentHealth = 0;
            }
            // Optionally, you can add logic to handle player death here
            if (bossCurrentHealth == 0)
            {
                StartCoroutine(Die()); // Call the Die method to handle enemy death
            }
        }

        public IEnumerator Die()
        {
            bossValues.IsDead = true;
            foreach (var spell in bossSpellCasting.spellList)
            {
                if (spell != null)
                    Destroy(spell);
            }
            bossSpellCasting.spellList.Clear();
            bossValues.bossAnimator.SetBool("IsDead", true);
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
            Destroy(BossCanvas);

        }
    }
}