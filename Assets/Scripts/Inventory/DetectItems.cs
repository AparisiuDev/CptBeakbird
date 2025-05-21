using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DetectItems : MonoBehaviour
{
    private bool grabInRange = false;
    private bool waitForE = false;
    private bool isAdded;
    public bool hasStolenPublic;
    private string itemSize;
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
    private float stolenTimer;
    private bool timerActive = false;

    [Header("UI")]
    //Cooldown when getting items
    public KeyCode grabItemKey = KeyCode.E;
    public float segundosNecesarios;  // Tiempo que debe mantenerse pulsado
    private float tiempoPresionado = 0f;
    private bool accionEjecutada = false;
    // Slider
    public Slider Slider;
    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    private float targetAlpha = 1f; // Default is fully visible
    [SerializeField] private float fadeSpeed = 1f;


    private void Start()
    {
        /***DEBUG
        estilo = new GUIStyle();
        estilo.fontSize = 32;
        estilo.normal.textColor = Color.black;
        ***/
        canvasGroup = Slider.GetComponent<CanvasGroup>();
    }


    private void Update()
    {
        Debug.Log(itemSize);
        FadeManager();
        timerStolen();
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
            if (Input.GetKey(grabItemKey))
            {
                if (!accionEjecutada)
                {
                    //Animacion
                    AnimationHandlerIn(itemSize);
                    //Barra de cojer
                    FadeIn();
                    tiempoPresionado += Time.deltaTime;
                    Slider.value = tiempoPresionado / segundosNecesarios;
                    if (tiempoPresionado >= segundosNecesarios)
                    {
                        FadeOut();
                        StoreItem(ItemStats.ItemStats);
                        accionEjecutada = true;
                        AnimationHandlerOut();
                    }
                }
            }
        }
        else
        {
            // Si se suelta la tecla antes del tiempo, reinicia
            tiempoPresionado = 0f;
            accionEjecutada = false;
            Slider.value = Mathf.MoveTowards(Slider.value, 0f, 0.5f * Time.deltaTime);
            if (Slider.value <= 0f) FadeOut();
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
        if (itemObj != null /***&& Input.GetKeyDown(KeyCode.E)***/)
        {
            stolenTimer = 3f;

            hasStolenPublic = true;
            timerActive = true;

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
        segundosNecesarios = other.GetComponent<ItemStatsContainer>().timeToGrab;
        itemSize = other.GetComponent<ItemStatsContainer>().size;

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

    private void justStolenItem(float duration)
    {
        hasStolenPublic = true;
        timerActive = true;
    }
    private void timerStolen()
    {
        if (hasStolenPublic)
        {
            stolenTimer -= Time.deltaTime;
            if(stolenTimer <= 0f)
            {
                hasStolenPublic = false;
            }
        }
    }

    public void FadeManager()
    {
        // Smoothly move current alpha toward target alpha
        if (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
        }
    }
    public void FadeIn()
    {
        targetAlpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void FadeOut()
    {
        targetAlpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void AnimationHandlerIn(string size)
    {
        switch (size)
        {
            case "SMALL":
                Animations.AnimatorManager.myAnimator.SetBool("grabSmall", true);
                break;
            case "MID":
                break;
            case "BIG":
                break;
            default:
                break;
        }
    }

    private void AnimationHandlerOut()
    {
        Animations.AnimatorManager.myAnimator.SetBool("grabSmall", false);

    }

    /***DEBUG
    private GUIStyle estilo;
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), "hasStolenPublic: " + hasStolenPublic.ToString(), estilo); 
    }
    ***/
}