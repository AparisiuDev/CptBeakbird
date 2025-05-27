using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelGoals : MonoBehaviour
{
    [Header("Happiness")]
    public Transform arrow;
    RectTransform rectTransform;
    [Range(0f, 1f)]
    public float happiness; // Felicidad entre 0 y 1
    private Quaternion targetRotation;
    private float rotationSpeed = 2.5f; // Velocidad de interpolación

    [Header("Time")]
    public float timerDuration; // Tiempo total en segundos
    private float currentTime;
    public PlayerHealth health;
    public Slider timer;
    private bool hasDied = false;
    [Range(0f, 1f)]
    public float darkness = 0.5f; // Nuevo valor para V (brillo)


    void Start()
    {
        DeclareArrowParameters();
        DeclareTime();
    }

    void Update()
    {
        ArrowManager();
        TimeManager();
        UpdateSliderColorBrightness(); 
    }

    void DeclareTime()
    {
        currentTime = timerDuration;
    }
    void DeclareArrowParameters()
    {
        rectTransform = arrow.GetComponent<RectTransform>();
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, 90f); // Se inicia en 90 grados
    }

    void TimeManager()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            timer.value = currentTime/timerDuration;
        }
        else if(!hasDied) 
        {
            hasDied = true;
            health.Die();
        }
    }

    void UpdateSliderColorBrightness()
    {
        ColorBlock colors = timer.colors;

        // Convertimos el color actual a HSV
        Color.RGBToHSV(colors.normalColor, out float h, out float s, out float _);

        // Escalamos el brillo (V) de 0.75 a 1 según el timer
        float newV = Mathf.Lerp(0.75f, 1f, timer.value);

        // Convertimos de vuelta a RGB
        Color newColor = Color.HSVToRGB(h, s, newV);

        // Asignamos el nuevo color
        colors.normalColor = newColor;
        timer.colors = colors;
    }


    void ArrowManager()
    {
        if (arrow != null)
        {
            // Calcula el ángulo de destino
            float targetAngle = (1f - happiness) * 90f;
            targetRotation = Quaternion.Euler(0f, 0f, -targetAngle);

            // Interpola suavemente hacia esa rotación
            rectTransform.localRotation = Quaternion.Lerp(
                rectTransform.localRotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }
}
