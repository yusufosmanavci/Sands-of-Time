using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the player
    public float currentHealth; // Current health of the player
    PlayerValues playerValues;
    PlayerController playerController;

    private void Start()
    {
        // Initialize current health to maximum health at the start
        currentHealth = maxHealth;
        playerValues = GetComponent<PlayerValues>();
        playerController = GetComponent<PlayerController>();
    }

    public void TakePLayerDamage(float damage, Vector2 attackerPosition)
    {
        // Reduce current health by the damage amount
        currentHealth -= damage;

        Vector2 knockbackDir = (transform.position - (Vector3)attackerPosition).normalized;
        Vector2 knockbackForce = new(knockbackDir.x * 10f, 5f); // X kuvveti yüksek, Y daha az
        StartCoroutine(playerController.KnockbackRoutine(knockbackForce));
        if (playerValues.rb != null)
        {
            playerValues.rb.AddForce(knockbackForce, ForceMode2D.Impulse); // Apply knockback force to the player
        }
        // Ensure current health does not drop below zero
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        // Optionally, you can add logic to handle player death here
        if (currentHealth == 0)
        {
            Destroy(gameObject); // Destroy the player object when health reaches zero
        }
    }
}
