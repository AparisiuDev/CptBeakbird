using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    private bool grabInRange = false;
    private ItemStats ItemStats;
    GameObject itemObj;

    private void Start()
    {
        ItemStats = GetComponent<ItemStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckGrab(other))
        {
            SeeItemStats(other);
            Debug.Log(ItemStats.Description);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        grabInRange = false;
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
}
