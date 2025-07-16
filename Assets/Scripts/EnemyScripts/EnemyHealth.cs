using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyHealth : MonoBehaviour
{
    public float enemyMaxHealth = 100f; // Maximum health of the player
    public float enemyCurrentHealth; // Current health of the player
    EnemyValues enemyValues;
    EnemyController enemyController;

    private void Start()
    {
        // Initialize current health to maximum health at the start
        enemyCurrentHealth = enemyMaxHealth;
        enemyValues = GetComponent<EnemyValues>();
        enemyController = GetComponent<EnemyController>();
    }


    public void TakeEnemyDashDamage(float damage)
    {
        // Reduce current health by the damage amount

        enemyValues.enemyRb.linearVelocity = Vector2.zero;
        // Ensure current health does not drop below zero
        if (enemyCurrentHealth < 0)
        {
            enemyCurrentHealth = 0;
        }
        // Optionally, you can add logic to handle player death here
        if (enemyCurrentHealth == 0)
        {
            Die(); // Call the Die method to handle enemy death
        }
    }

    public void Die()
    {
        gameObject.SetActive(false); // Destroy the player object when health reaches zero
        PlayerManager.Instance.playerValues.sandsOfTime += RandomSandsOfTimeAmount(); // Increase the player's sand of time count
    }

    public int RandomSandsOfTimeAmount()
    {
       return Random.Range(enemyValues.minSandsOfTime, enemyValues.maxSandsOfTime + 1);
    }

    public IEnumerator TakeEnemyDamage(float damage)
    {
        enemyCurrentHealth -= damage;
        enemyValues.enemyRb.linearVelocity = Vector2.zero; // Stop the enemy's movement when taking damage
        yield return new WaitForSeconds(1f); // Optional delay for damage effect
        if (enemyCurrentHealth <= 0)
        {
            Die(); // Call the Die method to handle enemy death
        }
    }
}
