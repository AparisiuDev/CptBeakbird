using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaviotoSFXManager : MonoBehaviour
{

    [Header("Audio Source")]
    public AudioSource audioSource;
    [Header("Audio Clips")]
    public AudioClip walk;

    public void WalkingSFX()
    {
        if (audioSource.clip != walk)
        {
            audioSource.clip = walk;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (!audioSource.isPlaying)
        {
            audioSource.Play(); // If clip is already set but stopped
        }
    }

    public void IdleSFX()
    {
        audioSource.clip = null;
        audioSource.Stop();
        audioSource.loop = false;
    }
}
