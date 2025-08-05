using Assets.Scripts.OtherScripts;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [Header("UI Elements")]
    public GameObject pauseMenu;
    public GameObject healthUI;
    public GameObject dashUI;
    public GameObject sandsOfTimeText;
    public GameObject healPotionText;
    public GameObject options;
    public GameObject resumeBut;
    public GameObject mainMenuBut;
    public GameObject optionsBut;

    [Header("UI")]
    public Button resumeButton;
    public Slider musicSlider;

    public static bool isPaused = false;



    private void Update()
    {
        if (CampfireUIController.Instance.IsPaused) return;
        else if (InputManager.pauseInput && !isPaused)
        {
            Pause();
        }
        else if (InputManager.pauseInput && isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
            pauseMenu.SetActive(false);

            healthUI.SetActive(true);
            dashUI.SetActive(true);
            sandsOfTimeText.SetActive(true);
            healPotionText.SetActive(true);

            InputManager.ActivatePlayerControls();
        }
    }

    private void SetUIElement(GameObject element)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(element);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;

        OpenPauseMenu();
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);

        healthUI.SetActive(false);
        dashUI.SetActive(false);
        sandsOfTimeText.SetActive(false);
        healPotionText.SetActive(false);

        InputManager.DeactivatePlayerControls();

        SetUIElement(resumeButton.gameObject);
    }


    public void OnResumePressed()
    {
        AudioManager.instance.PlaySFX("ButtonClick");
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);

        healthUI.SetActive(true);
        dashUI.SetActive(true);
        sandsOfTimeText.SetActive(true);
        healPotionText.SetActive(true);

        InputManager.ActivatePlayerControls();
    }

    public void OnOptionsPressed()
    {
        AudioManager.instance.PlaySFX("ButtonClick");
        resumeBut.SetActive(false);
        optionsBut.SetActive(false);
        mainMenuBut.SetActive(false);
        options.SetActive(true);
        SetUIElement(musicSlider.gameObject);
    }

    public void OnBackPressed()
    {
        AudioManager.instance.PlaySFX("ButtonClick");
        options.SetActive(false);
        resumeBut.SetActive(true);
        mainMenuBut.SetActive(true);
        optionsBut.SetActive(true);
        SetUIElement(resumeButton.gameObject);
    }

    public void OnMainMenuPressed()
    {
        AudioManager.instance.PlaySFX("ButtonClick");
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Main Menu");
    }
}
