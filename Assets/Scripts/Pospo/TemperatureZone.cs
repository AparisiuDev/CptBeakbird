using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TemperatureZone : MonoBehaviour
{
    public Transform targetObject;
    public float radius = 10f;
    public float maxTemperature = 70f;
    public float smoothingSpeed = 2f; // higher = faster transition

    private Volume postProcessVolume;
    private WhiteBalance whiteBalance;
    private float baseTemperature = 0f;
    private float currentTemperature;

    void Start()
    {
        postProcessVolume = FindObjectOfType<Volume>();

        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out whiteBalance))
        {
            baseTemperature = whiteBalance.temperature.value;
            currentTemperature = baseTemperature;
        }
        else
        {
            Debug.LogWarning("WhiteBalance override not found in Volume profile!");
        }
    }

    void Update()
    {
        if (whiteBalance == null) return;

        float distance = Vector3.Distance(transform.position, targetObject.position);
        float t = Mathf.Clamp01(1f - (distance / radius));

        // Optional: Apply easing (e.g. smoothstep for smooth in/out)
        float easedT = t * t * (3f - 2f * t);

        float targetTemp = Mathf.Lerp(baseTemperature, maxTemperature, easedT);

        // Smoothly interpolate toward target temperature
        currentTemperature = Mathf.Lerp(currentTemperature, targetTemp, Time.deltaTime * smoothingSpeed);
        whiteBalance.temperature.value = currentTemperature;
    }
}
