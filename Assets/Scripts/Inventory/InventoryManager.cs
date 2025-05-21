using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    [Header("NO TOCAR / PREGUNTAR AL BIEL")]
    //public float SlowDownScaleFactor = 0.3f;
    public float SlowDown;
    private bool menuActivated;

    public ItemSlot[] ItemSlot;
    // Start is called before the first frame update
    void Start()
    {
        SlowDown = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Inventory.instance.backpack.Count);
        ToggleMenu();
        AddItem();
    }
    
    void ToggleMenu()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && menuActivated)
        {
            SlowDown = 1f;
            Time.timeScale = SlowDown;
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }

        else if (Input.GetKeyDown(KeyCode.Tab) && !menuActivated)
        {
            SlowDown = 0.3f;
            Time.timeScale = SlowDown;
            InventoryMenu.SetActive(true);
            menuActivated = true;

        }
    }

    void AddItem()
    {
        for (int i = 0 ; i < Inventory.instance.backpack.Count; i++)
        {
            if (ItemSlot[i].isFull == false)
            {
                ItemSlot[i].AddItem(Inventory.instance.backpack[i].Name,Inventory.instance.backpack[i].Description, Inventory.instance.backpack[i].Model);
                //Inventory.instance.backpack[i].isAdded = true;
                    return;
            }
        }
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < ItemSlot.Length; i++)
        {
            ItemSlot[i].selectedShader.SetActive(false);
            ItemSlot[i].thisItemSelected = false;
        }
    }
}
