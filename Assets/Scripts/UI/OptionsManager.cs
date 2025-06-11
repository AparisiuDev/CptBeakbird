using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public GameObject menuPanel; // Assign your UI GameObject here in the Inspector

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
}
