using UnityEngine;

public class SplashSpawner : MonoBehaviour
{
    [Header("Prefab de Partícula")]
    public GameObject splashEffect;

    [Header("Puntos de Spawn")]
    public Transform spawnPointA;
    public Transform spawnPointB;

    [Header("Tiempo de vida de la partícula (segundos)")]
    public float particleLifetime = 2f;

    public void Splash()
    {
        if (splashEffect == null || spawnPointA == null || spawnPointB == null)
        {
            Debug.LogWarning("Faltan referencias en SplashSpawner.");
            return;
        }

        // Instanciar y destruir partícula en el punto A
        GameObject splashA = Instantiate(splashEffect, spawnPointA.position, spawnPointA.rotation);
        Destroy(splashA, particleLifetime);

        // Instanciar y destruir partícula en el punto B
        GameObject splashB = Instantiate(splashEffect, spawnPointB.position, spawnPointB.rotation);
        Destroy(splashB, particleLifetime);
    }
}
