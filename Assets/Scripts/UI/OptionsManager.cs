using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OptionsManager : MonoBehaviour
{
    public GameObject menuPanel; // Assign your UI GameObject here in the Inspector
    public GameObject optionsPanel; // Assign your UI GameObject here in the Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPanel != null)
            {
                menuPanel.SetActive(!menuPanel.activeSelf);
            }
        }
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }
}
