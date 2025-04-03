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
        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (ItemStats itemStats in backpack)
            {
                Debug.Log(itemStats.Name);
            }
        }
    }
}
