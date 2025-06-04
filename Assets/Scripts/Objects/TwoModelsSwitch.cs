using UnityEngine;

public class TwoModelsSwitch : MonoBehaviour
{
    public GameObject castilloRotoPrefab;
    public GameObject particulaPrefab; // Prefab con el Particle System
    private AddHapVal addHapVal;
    private ItemStatsContainer itemStatsContainer;

    private void Start()
    {
        itemStatsContainer = GetComponent<ItemStatsContainer>();
        addHapVal = GetComponent<AddHapVal>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Mano"))
        {
            Romper();
        }
    }

    void Romper()
    {
        // Copia el material
        Material materialOriginal = GetComponent<MeshRenderer>()?.material;

        // Instancia castillo roto
        GameObject roto = Instantiate(castilloRotoPrefab, transform.position, transform.rotation);

        // Asigna el material al modelo roto
        MeshRenderer rotoRenderer = roto.GetComponent<MeshRenderer>();
        if (rotoRenderer != null && materialOriginal != null)
        {
            rotoRenderer.material = materialOriginal;
        }

        // Instancia y reproduce la partícula
        if (particulaPrefab != null)
        {
            GameObject particula = Instantiate(particulaPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = particula.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
            // Destruir la partícula después de que termine
            Destroy(particula, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        addHapVal.AddValues(itemStatsContainer.ItemStats.Satisfaction, itemStatsContainer.ItemStats.Price);
        Destroy(gameObject);
    }

}
