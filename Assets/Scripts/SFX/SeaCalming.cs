using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaCalming : MonoBehaviour
{
    public AudioSource audioSource;
    public float maxVolume = 1f; // Full volume level
    public float duration;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Solo si el objeto tiene la etiqueta "Player"
        {
            StartCoroutine(FadeAudio(audioSource.volume, 0f, duration));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeAudio(audioSource.volume, maxVolume, duration));
        }
    }

    private IEnumerator FadeAudio(float from, float to, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            audioSource.volume = Mathf.Lerp(from, to, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = to;
    }
}
