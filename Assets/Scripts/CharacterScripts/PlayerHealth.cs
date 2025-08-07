using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the player
    public float currentHealth; // Current health of the player
    public int healPotions = 2; // Number of healing potions collected
    public GameObject hurtOverlay;


    private void Start()
    {
        // Initialize current health to maximum health at the start
        currentHealth = maxHealth;
        PlayerManager.Instance.playerHealthBar.SetMaxHealth(maxHealth); // Set the maximum health in the health bar UI
    }

    public void TakePLayerDamage(float damage, Vector2 attackerPosition)
    {
        // Reduce current health by the damage amount
        currentHealth -= damage;

        AudioManager.instance.PlaySFX("Hurt");
        

        PlayerManager.Instance.playerHealthBar.SetHealth(currentHealth); // Update the health bar UI

        Vector2 knockbackDir = (transform.position - (Vector3)attackerPosition).normalized;
        Vector2 knockbackForce = new(knockbackDir.x * 10f, 5f); // X kuvveti yüksek, Y daha az
        StartCoroutine(PlayerManager.Instance.playerController.KnockbackRoutine(knockbackForce));
        if (PlayerManager.Instance.playerValues.rb != null)
        {
            PlayerManager.Instance.playerValues.rb.AddForce(knockbackForce, ForceMode2D.Impulse); // Apply knockback force to the player
        }
        // Ensure current health does not drop below zero
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        // Optionally, you can add logic to handle player death here
        if (currentHealth == 0)
        {
            StartCoroutine(Die());
        }

        StartCoroutine(ActivateHurtOverlay());
    }

    public void HealPlayer(float healAmount)
    {
        PlayerManager.Instance.playerValues.rb.linearVelocity = Vector2.zero; // Stop player movement when healing
        if (healPotions > 0)
        {
            healPotions--; // Decrease the number of healing potions
            currentHealth += healAmount; // Increase current health by the heal amount
            PlayerManager.Instance.playerHealthBar.SetHealth(currentHealth); // Update the health bar UI
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth; // Ensure current health does not exceed maximum health
            }
        }
        else if (healPotions <= 0)
        {
            Debug.Log("No healing potions left!"); // Optional: Log message if no healing potions are available
        }

    }

    private IEnumerator Die()
    {
        InputManager.DeactivatePlayerControls();
        PlayerManager.Instance.playerValues.rb.linearVelocity = Vector2.zero; // Stop player movement
        yield return new WaitForSeconds(0.6f); // Wait for the animation to finish (adjust time as needed)
        PlayerManager.Instance.playerValues.IsDead = true; // Set the player state to dead
        //transform.position = PlayerValues.lastCheckpointPosition;
        if (PlayerManager.Instance.playerValues.IsDead)
        {
            if (PlayerManager.Instance.playerValues.lastCheckPoint != null)
            {
                StartCoroutine(RoomTransition.Instance.LoadRoom(PlayerManager.Instance.playerValues.lastCheckPoint));
                PlayerManager.Instance.playerValues.checkPointRoom.SetActive(true);
                PlayerManager.Instance.playerValues.IsDead = false;
                PlayerManager.Instance.playerHealth.currentHealth = PlayerManager.Instance.playerHealth.maxHealth;
                PlayerManager.Instance.playerHealthBar.SetHealth(PlayerManager.Instance.playerHealth.currentHealth);
                healPotions = 2;
                yield return new WaitForSeconds(0.3f);
                PlayerManager.Instance.playerValues.deathRoom.SetActive(false);
                InputManager.ActivatePlayerControls();
                EnemySpawner enemySpawner = PlayerManager.Instance.playerValues.deathRoom.GetComponentInChildren<EnemySpawner>();
                if(enemySpawner != null)
                {
                    enemySpawner.ClearEnemies();
                }
                BossSpawner bossSpawner = PlayerManager.Instance.playerValues.deathRoom.GetComponentInChildren<BossSpawner>();
                if(bossSpawner != null)
                {
                    bossSpawner.ClearBosses();
                }
            }
        }
    }

    public IEnumerator ActivateHurtOverlay()
    {
        hurtOverlay.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hurtOverlay.SetActive(false);
    }
}
