using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectItems : MonoBehaviour
{
    private bool grabInRange = false;
    private bool waitForE = false;
    private ItemStatsContainer ItemStats;
    GameObject itemObj;

    [Header("Grande Pequeño Variables")]
    public float scaleFactor = 1.1f; // Factor de escala al entrar en el trigger
    public float duration = 0.5f; // Duración del escalado
    private Vector3 originalScale;

    private void Start()
    {
    }

    private void Update()
    {
        // Al inputear E, checkea que este dentro y todo bien, y destruye el item y guarda su valor
        if (waitForE)
        {
            StoreItem(ItemStats.ItemStats);
        }

        DEB_ItemShow();

    }

    private void OnTriggerEnter(Collider other)
    {
        // Checkea grab, y se pone en modo espera al input de E
        if (CheckGrab(other))
        {
            originalScale = other.transform.localScale;
            MakeBigger(other);
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
        // Hacer pequeño el item
        MakeSmaller(other);
    }

    // Guardar Item en array y destruir al pulsar la E
    private void StoreItem(ItemStats NextItem)
    {
        if (itemObj != null && Input.GetKeyDown(KeyCode.E))
        {
            Inventory.instance.backpack.Add(NextItem);
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
        ItemStats = itemObj.GetComponent<ItemStatsContainer>();
    }

    //DEBUG: Ensenar el ultimo nombre y descripcion
    private void DEB_ItemShow()
    {
        if(Input.GetKeyDown(KeyCode.R))
            Debug.Log(Inventory.instance.backpack[Inventory.instance.backpack.Count - 1].Name);
        if (Input.GetKeyDown(KeyCode.T))
            Debug.Log(Inventory.instance.backpack[Inventory.instance.backpack.Count - 1].Description);
    }

    // Hacer más grande o más pequeño
    private void MakeBigger(Collider other)
    {
        LeanTween.scale(other.gameObject, originalScale * scaleFactor, duration).setEase(LeanTweenType.easeOutElastic);

    }
    private void MakeSmaller(Collider other)
    {
        LeanTween.scale(other.gameObject, originalScale, duration).setEase(LeanTweenType.easeOutElastic);
    }

}
