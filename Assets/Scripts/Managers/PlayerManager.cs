using Assets.Scripts.CharacterScripts;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script to manage player health
    public PlayerController playerController; // Reference to the PlayerController script for player movement and actions
    public PlayerValues playerValues; // Reference to the PlayerValues script for player attributes
    public PlayerHealthBar playerHealthBar; // Reference to the PlayerHealthBar script for health bar UI
    public PlayerAnimations playerAnimations; // Reference to the PlayerAnimations script for player animations
    public PlayerData playerData; // Reference to the PlayerData script for saving and loading player data

    public static PlayerManager Instance { get; private set; } // Singleton instance of PlayerManager

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject); // Ensure only one instance of PlayerManager exists
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scene loads
        }
    }
}
