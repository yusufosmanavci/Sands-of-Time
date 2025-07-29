using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    public Image dashAbility;
    public TextMeshProUGUI dashCooldownText;

    PlayerValues playerValues;

    private void Start()
    {
        playerValues = GetComponent<PlayerValues>();

        dashAbility.fillAmount = 0;

        dashCooldownText.text = "";
    }

    private void Update()
    {
        DashAbilityCooldown(dashAbility, dashCooldownText);
    }

    private void DashAbilityCooldown(Image skillImage, TextMeshProUGUI skillText)
    {
        if(playerValues.currentDashCooldown <= 0)
        {
            if(skillImage != null)
            {
                skillImage.fillAmount = 0;
            }
            if(skillText != null)
            {
                skillText.text = "";
            }
        }
        else
        {
            if (skillImage != null)
            {
                skillImage.fillAmount = playerValues.currentDashCooldown / playerValues.dashCooldown;
            }
            if (skillText != null)
            {
                skillText.text = Mathf.Ceil(playerValues.currentDashCooldown).ToString();
            }
        }
    }
}
