using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using LevelLocker;
using EasyTransition;
public class DetectItems : MonoBehaviour
{
    private bool grabInRange = false;
    private bool waitForE = false;
    private bool isAdded;
    public bool hasStolenPublic;
    private string itemSize;
    private bool itemVanish;
    private bool isVanishing;
    private bool canEraseVanish;
    private ItemStatsContainer ItemStats;

    //public DeadCamCulling player;

    GameObject itemObj;

    [Header("Grande Pequeño Variables")]
    public float scaleFactor = 1.3f; // Factor de escala al entrar en el trigger
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
    public LevelGoals levelGoals;
    public KeyCode grabItemKey = KeyCode.E;
    public float segundosNecesarios;  // Tiempo que debe mantenerse pulsado
    private float tiempoPresionado = 0f;
    private bool accionEjecutada = false;
    // Slider
    public Slider Slider;
    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    private float targetAlpha = 0f; // Default is fully visible
    [SerializeField] private float fadeSpeed = 1f;
    private SpawnParticleEffect spawnEffect;
    // Saco
    public float addSpeed = 1f; // velocidad del suavizado


    private string objectTag;
    private Collider typeOfCollider;
    public TransitionSettings transition;


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
        //Debug.Log(LevelLocker.VariablesGlobales._leaveTut);
        FadeManager();
        // If we are on cooldown, update the timer
        if (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;
        }
        else
        {
            canEnter = true; // Allow OnTriggerEnter again after cooldown
        }

        switch (objectTag)
        {
            case "Items":
                TypeItem();
                break;

            case "Barco":
                TypeBarco();
                break;

            case "Chest":
                break;

            case "NPC":
                break;

            default:
                break;
        }
    }

    /*** TYPES OF INTERACTIONS ***/

    public void TypeItem()
    {
        //Debug.Log(itemSize);
        timerStolen();

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
                        Animations.AnimatorManager.myAnimator.SetTrigger("grabCompleted");
                        HappinessMeterController();
                        LootMeterController();
                        FadeOut();
                        StoreItem(ItemStats.ItemStats);
                        //ItemStats.ItemStats.Satisfaction;
                        accionEjecutada = true;
                        AnimationHandlerOut();
                        
                    }
                }
            }
        }
        else
        {
            StopPressButton();
        }
    }

    public void TypeBarco()
    {
        if (!LevelLocker.VariablesGlobales._leaveTut) return;
        if (waitForE)
        {
            if (Input.GetKey(grabItemKey))
            {
                if (!accionEjecutada)
                {
                    //Animacion
                    AnimationHandlerIn("BARCO");
                    //Barra de cojer
                    FadeIn();
                    tiempoPresionado += Time.deltaTime;
                    Slider.value = tiempoPresionado / segundosNecesarios;
                    if (tiempoPresionado >= segundosNecesarios)
                    {
                        GoToLevelSelect();
                        FadeOut();
                        accionEjecutada = true;
                        AnimationHandlerOut();

                        //VFX
                        if (spawnEffect != null)
                            spawnEffect.SpawnParticle();

                    }
                }
            }
            else
            {
                StopPressButton();
            }
        }
    }

   
    private void OnTriggerEnter(Collider other)
    {
        objectTag = other.tag;
        typeOfCollider = other;
        if (other.GetComponent<SpawnParticleEffect>() != null) spawnEffect = other.GetComponent<SpawnParticleEffect>();
        // Logic for type of interactions
        switch (objectTag)
        {
            case "Items":
                OnEnterItems(other);
                break;
            case "Barco":
                OnEnterShip(other);
                break;

            case "Chest":
                break;

            case "NPC":
                break;

            default:
                break;
        }
        
    }

    /*** ON TRIGGER ENTER STUFF ***/
    private void OnEnterItems(Collider other)
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
    private void OnEnterShip(Collider other)
    {
        if (LevelLocker.VariablesGlobales._leaveTut && canEnter)
        {
            originalScale = other.transform.localScale;
            MakeBigger(other);
            waitForE = true;
            CooldownHandler();
        }
    }
    /*** TRIGGER EXIT ***/
    private void OnTriggerExit(Collider other)
    {
        CooldownHandler();
        // Seguridad de resetear todo en falso y null
        if (!isVanishing)
        {
            grabInRange = false;
            waitForE = false;
            itemObj = null;
            ItemStats = null;
        }
        else
        {
            StartCoroutine(putNull());
        }
            // objectTag = null;
            typeOfCollider = null;
        //Anims
        Animations.AnimatorManager.myAnimator.SetBool("grabSmall", false);
        Animations.AnimatorManager.myAnimator.SetBool("grabMid", false);
        Animations.AnimatorManager.myAnimator.SetBool("grabBig", false);
        //VFX
        spawnEffect = null;
        // Hacer pequeño el item y el barco solo si se ha pasado
        if (other.tag == "Barco" )
        {
            if (levelGoals.goalsCompleted || LevelLocker.VariablesGlobales._leaveTut)
            {
                MakeSmaller(other);
            }
            else return;
        }
        MakeSmaller(other);

    }
    private IEnumerator putNull()
    {
        // Wait until itemVanish is true
        yield return new WaitUntil(() => canEraseVanish);

        grabInRange = false;
        waitForE = false;
        itemObj = null;
        ItemStats = null;
    }

    /*** OTHER LOGIC ***/

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
            SmoothRemove(typeOfCollider);
            StartCoroutine(WaitAndVanish());

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
        if (!isVanishing)
        LeanTween.scale(other.gameObject, originalScale, duration).setEase(LeanTweenType.easeOutElastic).setEase(LeanTweenType.easeInBack);
    }

    private void SmoothRemove(Collider other)
    {
        if (other.GetComponent<SpawnParticleEffect>() != null) spawnEffect = other.GetComponent<SpawnParticleEffect>();
        canEraseVanish = false;
        isVanishing = true;
        Vector3 down = new Vector3(0.1f, 0.1f, 0.1f);
        Vector3 limitDown = new Vector3(0.2f, 0.2f, 0.2f);
        LeanTween.scale(other.gameObject, down, duration).setEase(LeanTweenType.easeOutElastic).setEase(LeanTweenType.easeInBack);
        StartCoroutine(SmallEnough(duration));
    }

    IEnumerator SmallEnough(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            itemVanish = true;
        }
    }

    private IEnumerator WaitAndVanish()
    {
        // Wait until itemVanish is true
        yield return new WaitUntil(() => itemVanish);

        if (spawnEffect != null)
            spawnEffect.SpawnParticle();
        itemObj.SetActive(false);
        itemVanish = false;
        isVanishing = false;
        canEraseVanish = true;
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
                segundosNecesarios = 1.6f;
                Animations.AnimatorManager.myAnimator.SetBool("grabSmall", true);
                break;
            case "MID":
                segundosNecesarios = 4.5f;
                Animations.AnimatorManager.myAnimator.SetBool("grabMid", true);
                break;
            case "BIG":
                segundosNecesarios = 6.5f;
                Animations.AnimatorManager.myAnimator.SetBool("grabBig", true);
                break;
            case "BARCO":
                segundosNecesarios = 1.6f;
                break;
            default:
                break;
        }
    }

    private void StopPressButton()
    {
        // Si se suelta la tecla antes del tiempo, reinicia
        tiempoPresionado = 0f;
        accionEjecutada = false;
        Slider.value = Mathf.MoveTowards(Slider.value, 0f, 0.5f * Time.deltaTime);
        if (Slider.value <= 0f) FadeOut();
        //Anims
        Animations.AnimatorManager.myAnimator.SetBool("grabSmall", false);
    }

    private void AnimationHandlerOut()
    {
        Animations.AnimatorManager.myAnimator.SetBool("grabSmall", false);
        Animations.AnimatorManager.myAnimator.SetBool("grabMid", false);
        Animations.AnimatorManager.myAnimator.SetBool("grabBig", false);


    }

    public void HappinessMeterController()
    {
        //Debug.Log(ItemStats.ItemStats.Satisfaction);
        levelGoals.happiness += (ItemStats.ItemStats.Satisfaction) / 100;
    }

    public void LootMeterController()
    {

        //Debug.Log(ItemStats.ItemStats.Satisfaction);
        levelGoals.lootValue += (ItemStats.ItemStats.Price) / 100;
        //levelGoals.lootValue = Mathf.Lerp(levelGoals.lootValue, (ItemStats.ItemStats.Price)/100, Time.deltaTime *addSpeed);
    }

    public void GoToLevelSelect()
    {
        TransitionManager.Instance().Transition("LevelSelect", transition, 0f);
    }

    /***DEBUG
    private GUIStyle estilo;
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), "hasStolenPublic: " + hasStolenPublic.ToString(), estilo); 
    }
    ***/
}