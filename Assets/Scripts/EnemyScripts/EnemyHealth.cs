using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyHealth : MonoBehaviour
{
    public float enemyMaxHealth = 100f; // Maximum health of the player
    public float enemyCurrentHealth; // Current health of the player
    EnemyValues enemyValues;
    EnemyController enemyController;
    public EnemyHealthBar enemyHealthBar;
    public CanvasGroup healthBar;
    public ParticleSystem particles;
    private void Start()
    {
        // Initialize current health to maximum health at the start
        enemyCurrentHealth = enemyMaxHealth;
        enemyValues = GetComponent<EnemyValues>();
        enemyController = GetComponent<EnemyController>();
        enemyHealthBar.SetMaxHealth(enemyMaxHealth); // Set the maximum health in the health bar UI
    }

    private void Update()
    {
        if (enemyCurrentHealth == 100)
        {
            healthBar.alpha = 0; // Hide the health bar if the enemy is at full health
        }
        else
            healthBar.alpha = 1; // Show the health bar if the enemy has taken damage
    }

    public void Die()
    {
        Instantiate(particles, transform.position, Quaternion.identity);
        gameObject.SetActive(false); // Destroy the player object when health reaches zero
        Debug.LogWarning("Enemy has died!"); // Log a message for debugging purposes
        PlayerManager.Instance.playerValues.sandsOfTime += RandomSandsOfTimeAmount(); // Increase the player's sand of time count
        Collectibles.instance.sandsOfTimeText.text = "Sands Of Time " + PlayerManager.Instance.playerValues.sandsOfTime; // Continuously update the text to reflect the current "sands of time" value
        Collectibles.instance.sandsOfTimeText1.text = "Sands Of Time " + PlayerManager.Instance.playerValues.sandsOfTime;
        PlayerManager.Instance.playerData.SandsOfTimeSave(); // Save the player's data after defeating the enemy
    }

    public int RandomSandsOfTimeAmount()
    {
       return Random.Range(enemyValues.minSandsOfTime, enemyValues.maxSandsOfTime + 1);
    }

    public IEnumerator TakeEnemyDamage(float damage)
    {
        enemyCurrentHealth -= damage;
        enemyHealthBar.SetHealth(enemyCurrentHealth); // Update the health bar UI

        enemyValues.enemyRb.linearVelocity = Vector2.zero; // Stop the enemy's movement when taking damage
        StartCoroutine(HitFlash()); // Call the HitFlash method to show damage effect
        if (enemyCurrentHealth <= 0)
        {
            Die(); // Call the Die method to handle enemy death
        }
        yield return new WaitForSeconds(1f); // Optional delay for damage effect

    }
    public IEnumerator TakeEnemyDashDamage(float damage)
    {
        enemyCurrentHealth -= damage;
        enemyHealthBar.SetHealth(enemyCurrentHealth); // Update the health bar UI

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
