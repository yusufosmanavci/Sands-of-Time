using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UIScripts
{
    public class BossHealthBar : MonoBehaviour
    {
        public Slider bossSlider;
        public Gradient bossGradient;
        public Image bossFill;

        public void SetMaxHealth(float health)
        {
            bossSlider.maxValue = health;
            bossSlider.value = health;

            bossFill.color = bossGradient.Evaluate(1f);
        }
        public void SetHealth(float health)
        {
            bossSlider.value = health;

            bossFill.color = bossGradient.Evaluate(bossSlider.normalizedValue);
        }
    }
}