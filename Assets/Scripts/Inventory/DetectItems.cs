using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectItems : MonoBehaviour
{
    private bool grabInRange = false;
    private bool waitForE = false;
    private bool isAdded;
    private ItemStatsContainer ItemStats;
    GameObject itemObj;

    [Header("Grande Pequeño Variables")]
    public float scaleFactor = 1.1f; // Factor de escala al entrar en el trigger
    public float duration = 0.2f; // Duración del escalado
    private Vector3 originalScale;

    // Timers
    private bool canEnter = true;
    private float cooldownTime = 0.3f;
    private float currentCooldownTime = 0f;

    private void Start()
    {
    }

    private void Update()
    {
        // If we are on cooldown, update the timer
        if (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;
        }
        else
        {
            canEnter = true; // Allow OnTriggerEnter again after cooldown
        }

        // Al inputear E, checkea que este dentro y todo bien, y destruye el item y guarda su valor
        if (waitForE)
        {
            StoreItem(ItemStats.ItemStats);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // Only process the collision if the timer is not running
        if (canEnter && CheckGrab(other))
        {
            originalScale = other.transform.localScale;
            MakeBigger(other);
            SeeItemStats(other);
            waitForE = true;

            CooldownHandler();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CooldownHandler();
        // Seguridad de resetear todo en falso y null
        grabInRange = false;
        waitForE = false;
        itemObj = null;
        ItemStats = null;
        // Hacer pequeño el item
        MakeSmaller(other);

    }

    private void CooldownHandler()
    {
        currentCooldownTime = cooldownTime;
        canEnter = false;
    }

    // Guardar Item en array y destruir al pulsar la E
    private void StoreItem(ItemStats NextItem)
    {
        isAdded = false;
        if (itemObj != null && Input.GetKeyDown(KeyCode.E))
        { 
            //Check if already added
            for (int i = 0; i < Inventory.instance.backpack.Count; i++) 
                if (Inventory.instance.backpack[i].Name == NextItem.Name) isAdded = true;

            // Add and disable object
            if (!isAdded) Inventory.instance.backpack.Add(NextItem);
            itemObj.SetActive(false);
            grabInRange = false;
            waitForE = false;
            itemObj = null;
            ItemStats = null;
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

    
    // Hacer más grande o más pequeño
    private void MakeBigger(Collider other)
    {
        LeanTween.scale(other.gameObject, originalScale * scaleFactor, duration).setEase(LeanTweenType.easeOutElastic).setEase(LeanTweenType.easeOutBack);

    }
    private void MakeSmaller(Collider other)
    {
        LeanTween.scale(other.gameObject, originalScale, duration).setEase(LeanTweenType.easeOutElastic).setEase(LeanTweenType.easeInBack);
    }

}