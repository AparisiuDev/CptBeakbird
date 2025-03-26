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
        LevelCol = LevelCheck(LevelCol, other);
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset de todo
        canEnter = false;
        LevelCol = null;
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
}
