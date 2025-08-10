using Assets.Scripts.CharacterScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{
    public static Collectibles instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public TextMeshProUGUI sandsOfTimeText; // Reference to the TextMeshProUGUI component for displaying "sands of time"
    public TextMeshProUGUI sandsOfTimeText1;
    public TextMeshProUGUI healPotionText;

    private void Start()
    {
        healPotionText.text = "Heal Potions: " + PlayerManager.Instance.playerHealth.healPotions;
        //sandsOfTimeText.text = "Sands Of Time " + PlayerManager.Instance.playerValues.sandsOfTime; // Initialize the player's "sands of time" value with the text from the UI
    }

    private void Update()
    {
    }
}
