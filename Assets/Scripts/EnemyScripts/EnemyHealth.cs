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
        if (enemyCurrentHealth <= 0)
        {
            Die(); // Call the Die method to handle enemy death
        }
    }

    public void Die()
    {
        gameObject.SetActive(false); // Destroy the player object when health reaches zero
        Debug.LogWarning("Enemy has died!"); // Log a message for debugging purposes
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
        StartCoroutine(HitFlash()); // Call the HitFlash method to show damage effect
        if (enemyCurrentHealth <= 0)
        {
            Die(); // Call the Die method to handle enemy death
        }
        yield return new WaitForSeconds(1f); // Optional delay for damage effect

    }

    public IEnumerator HitFlash()
    {
        enemyValues.enemySpriteRenderer.color = new Color(1f, 0.2f, 0.2f, 1f); // Change color to red
        yield return new WaitForSeconds(0.1f); // Wait for a short duration
        enemyValues.enemySpriteRenderer.color = Color.white; // Reset color to white
    }
}
