using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using LevelLocker;
//using MaskTransitions;
using EasyTransition;
public class LevelEnterer : MonoBehaviour
{
    // Checks del trigger y variable del nombre del level
    private bool inRange;
    private bool canEnter;
    private string LevelCol;
    public float scaleFactor = 1.2f; // Factor de escala al entrar en el trigger
    public float duration = 0.5f; // Duraci�n del escalado
    public TransitionSettings transition;

    private Vector3 originalScale;

    private void Update()
    {

       if (inRange && canEnter)
         if(Input.GetKeyDown(KeyCode.E))
            TransitionManager.Instance().Transition(LevelCol, transition, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Setear el tp point
        LevelCol = LevelCheck(LevelCol, other);


        if (!canEnter) return;
        //Activar la posibilidad de entrar
        inRange = true;

        //Hacergrande
        originalScale = other.transform.localScale;
        MakeBigger(other);

    }

    private void OnTriggerExit(Collider other)
    {
        // Reset de todo
        inRange = false;
        LevelCol = null;
        if(!canEnter) return;
        // Hacer normal again
        MakeSmaller(other);
        canEnter = false;
    }

    private string LevelCheck(string level, Collider other)
    {
        //Check de quin level es

        if (other.CompareTag("Tutorial"))
        {
            level = "Intro_Tuto";
            canEnter = true;
        }
        if (other.CompareTag("Level1"))
        {
            level = "Tropi_Tuto";
            canEnter = LevelLocker.VariablesGlobales._lvl1;
        }

        if (other.CompareTag("Level2"))
        {
            level = "Disco_Tuto";
            canEnter = LevelLocker.VariablesGlobales._lvl2;
        }

        if (other.CompareTag("Tapon"))
        {
            level = "Ending";
            canEnter = LevelLocker.VariablesGlobales._tapon;
        }

        return level;
    }

    private void MakeBigger(Collider other)
    {
        LeanTween.scale(other.gameObject, originalScale * scaleFactor, duration).setEase(LeanTweenType.easeOutElastic);
    }

    private void MakeSmaller(Collider other)
    {
        LeanTween.scale(other.gameObject, originalScale, duration).setEase(LeanTweenType.easeOutElastic);
    }
}
