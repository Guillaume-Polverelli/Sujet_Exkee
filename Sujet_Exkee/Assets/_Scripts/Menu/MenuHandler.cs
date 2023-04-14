using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Assertions;

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

    private void Start()
    {
        Assert.IsNotNull(pauseMenu);
        Assert.IsNotNull(victoryMenu);
    }

    public bool getPause()
    {
        return isPaused;
    }

    public void HandlePauseInput()
    {
        if (victoryMenu.activeInHierarchy) return;

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
    }

    public void HidePauseScreen()
    {
        pauseMenu.SetActive(false);
        ResumeGame();
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }

    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowVictoryScreen(int player)
    {
        victoryMenu.SetActive(true);
        if(player == 0)
        {
            victoryMenu.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("C'est un draw ! ");
        }
        else
        {
            victoryMenu.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Joueur " + player + " emporte la partie !");
        }  
    }
}
