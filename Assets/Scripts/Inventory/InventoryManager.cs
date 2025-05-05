using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    public float SlowDownScaleFactor = 0.3f;
    public float SlowDown;
    private bool menuActivated;
    // Start is called before the first frame update
    void Start()
    {
        SlowDown = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        ToggleMenu();
    }
    
    void ToggleMenu()
    {
        if (Input.GetKeyDown(KeyCode.I) && menuActivated)
        {
            SlowDown = 1f;
            Time.timeScale = SlowDown;
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }

        else if (Input.GetKeyDown(KeyCode.I) && !menuActivated)
        {
            SlowDown = SlowDownScaleFactor;
            Time.timeScale = SlowDown;
            InventoryMenu.SetActive(true);
            menuActivated = true;

        }
    }
}
