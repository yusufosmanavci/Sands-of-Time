using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager instance;
    [SerializeField] private Image _fadeOutImage;
    [Range(0f,10f), SerializeField] private float _fadeOutSpeed = 5f;
    [Range(0f, 10f), SerializeField] private float _fadeInSpeed = 5f;

    [SerializeField] private Color fadeOutStartColor;

    public bool IsFadingOut { get; set; }
    public bool IsFadingIn { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        fadeOutStartColor.a = 0f;
    }

    private void Update()
    {
        if (IsFadingOut)
        {
            if(_fadeOutImage.color.a < 1f)
            {
                fadeOutStartColor.a += Time.deltaTime * _fadeOutSpeed;
                _fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                IsFadingOut = false;
            }
        }

        if(IsFadingIn)
        {
            if(_fadeOutImage.color.a > 0f)
            {
                fadeOutStartColor.a -= Time.deltaTime * _fadeInSpeed;
                _fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                IsFadingIn = false;
            }
        }
    }

    public void StartFadeOut()
    {
        _fadeOutImage.color = fadeOutStartColor;
        IsFadingOut = true;
    }

    public void StartFadeIn()
    {
        if(_fadeOutImage.color.a >= 1f)
        {
            _fadeOutImage.color = fadeOutStartColor;
            IsFadingIn = true;
        }
    }
}
