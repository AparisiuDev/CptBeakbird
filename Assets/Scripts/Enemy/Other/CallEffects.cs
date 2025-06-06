using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CallEffects : MonoBehaviour
{
    private bool alreadyAddedWhale = false;
    private bool alreadySaidHi = false;
    [SerializeField] private ParticleSystem targetParticles;
    [SerializeField] private GameObject sand;
    [SerializeField] private AddHapVal addSat;
    [SerializeField] private AddHapVal addSatHello;
    [SerializeField] private ItemStatsContainer sandVAL;
    [SerializeField] private ItemStatsContainer helloVAL;

    public Transform spawnPoint;      // Optional: where to spawn the particles
    private Vector3 position;

    public void Start()
    {
        position = spawnPoint != null ? spawnPoint.position : transform.position;
        if (addSat != null )
            sandVAL = addSat.gameObject.GetComponent<ItemStatsContainer>();
        if(addSatHello != null)
            helloVAL = addSatHello.gameObject.GetComponent<ItemStatsContainer>();
    }
    // Called via Animation Event
    public void WakeUp()
    {
        Debug.Log("WakeUp called on " + gameObject.name);
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

    public void CallVFX()
    {
        //Debug.Log("CallVFX");
        //VFX
        if (sand != null)
        Instantiate(sand, position, Quaternion.identity);
        if (alreadyAddedWhale) return;
        addSat.AddValues(sandVAL.Satisfaction, sandVAL.Price); // Adjust values as needed
        alreadyAddedWhale = true; // Ensure this is only called once

    }

    public void Saludo()
    {
        //Debug.Log("YOLOOOOOOOOOOOO");
        if (alreadySaidHi) return;
        addSat.AddValues(helloVAL.Satisfaction, helloVAL.Price); // Adjust values as needed
        alreadySaidHi = true; // Ensure this is only called once

    }
}
