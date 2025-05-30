using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StaminaUIHandler : MonoBehaviour
{
    public Image Slider;
    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration;
    private float targetAlpha = 1f; // Default is fully visible
    [SerializeField] private float fadeSpeed = 1f; 
    
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Slider.fillAmount < 1f)
        {
            FadeIn();
        }
        if (Slider.fillAmount >= 1f)
        {
            FadeOut();
        }

        FadeManager();
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
        fadeDuration = 0.2f;
        targetAlpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void FadeOut()
    {
        fadeDuration = 1f;
        targetAlpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
