using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public static MenuHandler Instance;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject victoryMenu;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public bool getPause()
    {
        return isPaused;
    }

    public void HandlePauseInput()
    {
        if (victoryMenu.activeInHierarchy) return;

        print("HandlePauseInput");

        if (isPaused)
        {
            HidePauseScreen();
        }
        else
        {
            ShowPauseScreen();
        }
    }

    private void ShowPauseScreen()
    {
        pauseMenu.SetActive(true);
        PauseGame();
        print("ShowPauseScreen");
    }

    public void HidePauseScreen()
    {
        pauseMenu.SetActive(false);
        print("HidePauseScreen");
        ResumeGame();
    }

    private void ResumeGame()
    {
        isPaused = false;
        print("ResumeGame");
        Time.timeScale = 1;
    }

    private void PauseGame()
    {
        isPaused = true;
        print("PauseGame");
        Time.timeScale = 0;
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void ShowVictoryScreen()
    {
        victoryMenu.SetActive(true);
        PauseGame();
    }
}
