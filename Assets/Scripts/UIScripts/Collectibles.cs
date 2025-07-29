using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{
    public TextMeshProUGUI sandsOfTimeText; // Reference to the TextMeshProUGUI component for displaying "sands of time"
    public TextMeshProUGUI healPotionText;

    private void Start()
    {
        healPotionText.text = "Heal Potions: " + PlayerManager.Instance.playerHealth.healPotions;
        sandsOfTimeText.text = "Sands Of Time " + PlayerManager.Instance.playerValues.sandsOfTime; // Initialize the player's "sands of time" value with the text from the UI
    }

    private void Update()
    {
        healPotionText.text = "Heal Potions: " + PlayerManager.Instance.playerHealth.healPotions;
        sandsOfTimeText.text = "Sands Of Time " + PlayerManager.Instance.playerValues.sandsOfTime; // Continuously update the text to reflect the current "sands of time" value
    }
}
