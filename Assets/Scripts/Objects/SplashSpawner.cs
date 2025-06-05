using UnityEngine;

public class SplashSpawner : MonoBehaviour
{
    [Header("Prefab de Part�cula")]
    public GameObject splashEffect;

    [Header("Puntos de Spawn")]
    public Transform spawnPointA;
    public Transform spawnPointB;

    [Header("Tiempo de vida de la part�cula (segundos)")]
    public float particleLifetime = 2f;

    public void Splash()
    {
        if (splashEffect == null || spawnPointA == null || spawnPointB == null)
        {
            Debug.LogWarning("Faltan referencias en SplashSpawner.");
            return;
        }

        // Instanciar y destruir part�cula en el punto A
        GameObject splashA = Instantiate(splashEffect, spawnPointA.position, spawnPointA.rotation);
        Destroy(splashA, particleLifetime);

        // Instanciar y destruir part�cula en el punto B
        GameObject splashB = Instantiate(splashEffect, spawnPointB.position, spawnPointB.rotation);
        Destroy(splashB, particleLifetime);
    }
}
