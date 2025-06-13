using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using MaskTransitions;
using EasyTransition;

public class MainMenuController : MonoBehaviour
{
    public GameObject optionPanel;
    public TransitionSettings transition;

    //Boton ir hacia juego
    public void StartButton()
    {
        //Debug.Log("YAHOO");
        TransitionManager.Instance().Transition("Cine_Tuto", transition, 0f);
        //TransitionManager.Instance.LoadLevel("LevelSelect", 0.5f);
       //SceneManager.LoadScene("LevelSelect");
    }

    //Boton volver hacia menu
    public void MainMenuButton()
    {
        TransitionManager.Instance().Transition("Main_Menu", transition, 0f);
    }
    public void LevelSelect()
    {
        TransitionManager.Instance().Transition("LevelSelect", transition, 0f);
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
