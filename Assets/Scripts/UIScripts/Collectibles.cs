using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{
    public TextMeshProUGUI sandsOfTimeText; // Reference to the TextMeshProUGUI component for displaying "sands of time"
    public PlayerValues playerValues; // Reference to the PlayerValues script to access the player's "sands of time" value

    private void Start()
    {
        sandsOfTimeText.text = "Sands Of Time " + playerValues.sandsOfTime.ToString(); // Initialize the player's "sands of time" value with the text from the UI
    }

    private void Update()
    {
        sandsOfTimeText.text = "Sands Of Time " + playerValues.sandsOfTime.ToString(); // Continuously update the text to reflect the current "sands of time" value
    }
}
