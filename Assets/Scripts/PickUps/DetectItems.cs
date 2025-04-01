using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectItems : MonoBehaviour
{
    private bool grabInRange = false;
    private bool waitForE = false;
    private ItemStats ItemStats;
    GameObject itemObj;
    // Lista para guardar toda la info de los items
    public List<ItemStats> Inventory = new List<ItemStats>();

    private void Start()
    {
        ItemStats = GetComponent<ItemStats>();
    }

    private void Update()
    {
        // Al inputear E, checkea que este dentro y todo bien, y destruye el item y guarda su valor
        if (waitForE)
        {
            StoreItem(ItemStats);
        }

        DEB_ItemShow();

    }

    private void OnTriggerEnter(Collider other)
    {
        // Checkea grab, y se pone en modo espera al input de E
        if (CheckGrab(other))
        {
            SeeItemStats(other);
            waitForE = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Seguridad de resetear todo en falso y null
        grabInRange = false;
        waitForE = false;
        itemObj = null;
        ItemStats = null;
    }

    // Guardar Item en array y destruir al pulsar la E
    private void StoreItem(ItemStats NextItem)
    {
        if (ItemStats != null && itemObj != null && Input.GetKeyDown(KeyCode.E))
        {
            Inventory.Add(NextItem);
            itemObj.SetActive(false);
        }
    }

    // Checkear si te el tag "Items" quan esta en rango
    private bool CheckGrab(Collider other)
    {
        grabInRange = other.CompareTag("Items");
        return grabInRange;
    }

    // Guardar en una variable la info del item
    private void SeeItemStats(Collider other)
    {
        itemObj = other.gameObject;
        ItemStats = itemObj.GetComponent<ItemStats>();
    }

    //DEBUG: Ensenar el ultimo nombre y descripcion
    private void DEB_ItemShow()
    {
        if(Input.GetKeyDown(KeyCode.R))
            Debug.Log(Inventory[Inventory.Count - 1].Name);
        if (Input.GetKeyDown(KeyCode.T))
            Debug.Log(Inventory[Inventory.Count - 1].Description);
    }
}
