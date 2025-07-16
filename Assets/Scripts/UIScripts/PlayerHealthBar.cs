using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(float health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }




    //Code learn
    private Coroutine fadeCoroutine;

    public void StartFade()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null; // �nceki coroutine'i durdur ve referans� s�f�rla
        }
        if(fadeCoroutine == null)
        fadeCoroutine = StartCoroutine(Fade());
    }
    private IEnumerator Fade(Action complete = null)
    {
        yield return new WaitForSeconds(0f);
        yield return new WaitUntil(() => slider.maxValue > 1);
        float duration = 1f; // Fade s�resi
        float journey = 0f;
        while (journey <= duration)
        {
            journey += Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);
            float t = percent * percent;
            float x = Mathf.Lerp(0,10,percent);
            // Burada x de�erini istedi�iniz �ekilde uygulayabilirisin
            yield return null;
        }
        fadeCoroutine = null; // Coroutine tamamland���nda referans� s�f�rla
        complete?.Invoke();
    }
    private void Wafsdf(int a=-1)
    {
        
    }
}
