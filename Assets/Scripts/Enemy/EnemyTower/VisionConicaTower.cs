using UnityEngine;

public class VisionConicaTower : MonoBehaviour
{
    [Header("Paràmetres de visió")]
    public float anguloVision = 60f;         // Angle del con en graus
    public float distanciaVision = 10f;      // Distància màxima de visió
    // NUEVO: Offset per desplaçar el con lateralment
    [Range(-180f, 180f)]
    public float offsetAngulo = 0f;
    public LayerMask capaObjetivo;           // Capes detectables
    public LayerMask capaObstaculos;         // Capes que bloquegen visió
    public bool canSeePlayer;
    public bool activeChase;
    public Transform player;

    [Header("Debug")]
    public bool mostrarGizmos = true;


    private void Start()
    {
    }

    public bool EstaEnZonaDeVision(Transform objetivo)
    {
        Vector3 direccionAlObjetivo = (objetivo.position - transform.position).normalized;

        // Direcció base del con amb l'offset aplicat
        Vector3 direccionCono = Quaternion.Euler(0, offsetAngulo, 0) * transform.forward;

        float angulo = Vector3.Angle(direccionCono, direccionAlObjetivo);

        if (angulo < anguloVision / 2f)
        {
            float distancia = Vector3.Distance(transform.position, objetivo.position);
            if (distancia < distanciaVision)
            {
                if (!Physics.Raycast(transform.position, direccionAlObjetivo, distancia, capaObstaculos))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void FixedUpdate()
    {
        if (!EstaEnZonaDeVision(player))
        {
            activeChase = false;
            canSeePlayer = false;
            return;
        }

        if (EstaEnZonaDeVision(player))
        {
            canSeePlayer = true;
        }
        else
        {
            canSeePlayer = false;
        }
    }

    void OnDrawGizmos()
    {
        if (!mostrarGizmos) return;

        Gizmos.color = Color.red;

        // Direcció base del con amb offset
        Vector3 direccionCono = Quaternion.Euler(0, offsetAngulo, 0) * transform.forward;

        Vector3 leftLimit = Quaternion.Euler(0, -anguloVision / 2, 0) * direccionCono;
        Vector3 rightLimit = Quaternion.Euler(0, anguloVision / 2, 0) * direccionCono;

        Gizmos.DrawRay(transform.position, leftLimit * distanciaVision);
        Gizmos.DrawRay(transform.position, rightLimit * distanciaVision);

        Gizmos.DrawWireSphere(transform.position, distanciaVision);
    }
}
