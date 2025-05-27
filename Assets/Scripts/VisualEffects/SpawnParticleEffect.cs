using UnityEngine;

public class SpawnParticleEffect : MonoBehaviour
{
    public GameObject particlePrefab; // Assign your particle prefab here
    public Transform spawnPoint;      // Optional: where to spawn the particles

    public void SpawnParticle()
    {
        Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;
        Instantiate(particlePrefab, position, Quaternion.identity);
    }
}
