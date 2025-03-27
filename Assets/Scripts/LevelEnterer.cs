using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnterer : MonoBehaviour
{
    // Checks del trigger y variable del nombre del level
    private bool canEnter;
    private string LevelCol;
    public float scaleFactor = 1.2f; // Factor de escala al entrar en el trigger
    public float duration = 0.5f; // Duración del escalado

    private Vector3 originalScale;

    private void Update()
    {
       if (canEnter)
         if(Input.GetKeyDown(KeyCode.E))
          SceneManager.LoadScene(LevelCol);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Activar la posibilidad de entrar
        canEnter = true;

        //Hacergrande
        originalScale = other.transform.localScale;
        MakeBigger(other);

        //Setear el tp point
        LevelCol = LevelCheck(LevelCol, other);
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset de todo
        canEnter = false;
        LevelCol = null;

        // Hacer normal again
        MakeSmaller(other);
    }

    private string LevelCheck(string level, Collider other)
    {
        //Check de quin level es

        if (other.CompareTag("Level1"))
            level = "Level1Test";

        if (other.CompareTag("Level2"))
            level = "Level2Test";

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
