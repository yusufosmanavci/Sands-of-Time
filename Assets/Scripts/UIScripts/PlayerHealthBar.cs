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
            fadeCoroutine = null; // Önceki coroutine'i durdur ve referansý sýfýrla
        }
        if(fadeCoroutine == null)
        fadeCoroutine = StartCoroutine(Fade());
    }
    private IEnumerator Fade(Action complete = null)
    {
        yield return new WaitForSeconds(0f);
        yield return new WaitUntil(() => slider.maxValue > 1);
        float duration = 1f; // Fade süresi
        float journey = 0f;
        while (journey <= duration)
        {
            journey += Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration);
            float t = percent * percent;
            float x = Mathf.Lerp(0,10,percent);
            // Burada x deðerini istediðiniz þekilde uygulayabilirisin
            yield return null;
        }
        fadeCoroutine = null; // Coroutine tamamlandýðýnda referansý sýfýrla
        complete?.Invoke();
    }
    private void Wafsdf(int a=-1)
    {
        
    }
}
