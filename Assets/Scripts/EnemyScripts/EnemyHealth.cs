using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyMaxHealth = 100f; // Maximum health of the player
    public float enemyCurrentHealth; // Current health of the player
    EnemyValues enemyValues;
    EnemyController enemyController;
    public PlayerValues playerValues;

    private void Start()
    {
        // Initialize current health to maximum health at the start
        enemyCurrentHealth = enemyMaxHealth;
        enemyValues = GetComponent<EnemyValues>();
        enemyController = GetComponent<EnemyController>();
        playerValues = FindFirstObjectByType<PlayerValues>();
    }

    public void TakeEnemyDamage(float damage, Vector2 attackerPosition)
    {
        // Reduce current health by the damage amount
        enemyCurrentHealth -= damage;

        float xDirection = transform.position.x - attackerPosition.x > 0 ? 1 : -1;

        float knockbackPower = 3f;
        float knockbackY = 3f;

        Vector2 knockbackForce = new(knockbackPower * xDirection, knockbackY);

        enemyValues.enemyRb.linearVelocity = Vector2.zero;
        enemyValues.enemyRb.AddForce(knockbackForce, ForceMode2D.Impulse);
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

    public void TakeEnemyDashDamage(float damage, Vector2 attackerPosition)
    {
        // Reduce current health by the damage amount
        enemyCurrentHealth -= damage;

        float xDirection = transform.position.x - attackerPosition.x > 0 ? 1 : -1;

        float knockbackPower = 4f;  // dash daha güçlü iter
        float knockbackY = 4f;

        Vector2 knockbackForce = new(knockbackPower * xDirection, knockbackY);

        enemyValues.enemyRb.linearVelocity = Vector2.zero;
        enemyValues.enemyRb.AddForce(knockbackForce, ForceMode2D.Impulse);
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
        playerValues.sandsOfTime += RandomSandsOfTimeAmount(); // Increase the player's sand of time count
    }

    public int RandomSandsOfTimeAmount()
    {
       return Random.Range(enemyValues.minSandsOfTime, enemyValues.maxSandsOfTime + 1);
    }
}
