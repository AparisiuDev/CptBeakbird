using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriolBubbles : MonoBehaviour
{
    [SerializeField] private ParticleSystem targetParticles;

    // Called via Animation Event
    public void WakeUp()
    {
        if (targetParticles != null)
        {
            var emission = targetParticles.emission;
            emission.enabled = false;
            //Debug.Log("Particles stopped.");
        }
    }

    // Called via Animation Event
    public void FallSleep()
    {
        if (targetParticles != null)
        {
            var emission = targetParticles.emission;
            emission.enabled = true;
            //Debug.Log("Particles started.");
        }
    }
}
