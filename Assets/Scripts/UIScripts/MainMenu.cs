using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button defaultButton;
    public Button settingsButton;
    public GameObject buttons;
    public GameObject settings;
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
    }

    public void SetUIElement(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void OnStartPressed()
    {
        AudioManager.instance.PlaySFX("ButtonClick");
        SceneManager.LoadScene(1);
    }

    public void OnSettingsPressed()
    {
        AudioManager.instance.PlaySFX("ButtonClick");
        buttons.SetActive(false);
        settings.SetActive(true);
        SetUIElement(settingsButton.gameObject);
    }

    public void OnQuitPressed()
    {
        AudioManager.instance.PlaySFX("ButtonClick");
        Application.Quit();
        Debug.Log("Quit!");
    }

    public void OnBackPressed()
    {
        AudioManager.instance.PlaySFX("ButtonClick");
        buttons.SetActive(true);
        settings.SetActive(false);
        SetUIElement(defaultButton.gameObject);
    }

}
