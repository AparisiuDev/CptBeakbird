using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private ItemStats ItemStats;
    public List<ItemStats> backpack = new List<ItemStats>();

    public static Inventory instance; // Singleton

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    private void Update()
    {

        DEB_ItemShow();
    }

    private void DEB_ItemShow()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Debug.Log(Inventory.instance.backpack[Inventory.instance.backpack.Count - 1].Name);
        if (Input.GetKeyDown(KeyCode.T))
            Debug.Log(Inventory.instance.backpack[Inventory.instance.backpack.Count - 1].Description);
    }
}
