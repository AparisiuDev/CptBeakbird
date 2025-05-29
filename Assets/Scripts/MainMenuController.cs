using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MaskTransitions;

public class MainMenuController : MonoBehaviour
{
    public GameObject optionPanel;

    //Boton ir hacia juego
    public void StartButton()
    {
        TransitionManager.Instance.LoadLevel("LevelSelect", 0.5f);
       //SceneManager.LoadScene("LevelSelect");
    }

    //Boton volver hacia menu
    public void LeaveButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void CreditsEnter()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OptionButton()
    {
        optionPanel.SetActive(true);
    }

    public void exitOptionsButton()
    {
        optionPanel.SetActive(false);
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }


    public void quitButton()
    {
        Application.Quit();
    }
}
