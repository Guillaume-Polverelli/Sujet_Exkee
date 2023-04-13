using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{ 
    [SerializeField] private GameObject _gameChoiceWindow;
    [SerializeField] private GameObject _helpWindow;

    private void Awake()
    {
        Debug.Assert(_gameChoiceWindow != null);
        Debug.Assert(_helpWindow != null);
    }

    public void StartGamePvp()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartGameAI()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void NewGameButton()
    {
        _gameChoiceWindow.SetActive(true);
    }

    public void CloseNewGameWindow()
    {
        _gameChoiceWindow.SetActive(false);
    }

    public void HelpButton()
    {
        _helpWindow.SetActive(true);
    }

    public void CloseHelpWindow()
    {
        _helpWindow.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
