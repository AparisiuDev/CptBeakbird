using UnityEngine;

public class VisionConicaTower : MonoBehaviour
{
    [Header("Paràmetres de visió")]
    public float anguloVision = 60f;         // Angle del con en graus
    public float distanciaVision = 10f;      // Distància màxima de visió
    public LayerMask capaObjetivo;           // Quines capes poden ser detectades
    public LayerMask capaObstaculos;         // Quines capes bloquegen la visió
    public bool canSeePlayer;
    public bool activeChase;
    public DetectItems detectItems;
    public Transform player;

    [Header("Debug")]
    public bool mostrarGizmos = true;

    private void Start()
    {
        /*** DEBUG
        estilo = new GUIStyle();
        estilo.fontSize = 32;
        estilo.normal.textColor = Color.black;
        ***/
    }
    /// <summary>
    /// Comprova si un objectiu està dins del con de visió i sense obstacles.
    /// </summary>
    public bool EstaEnZonaDeVision(Transform objetivo)
    {
        // Calculem la direcció cap a l'objectiu
        Vector3 direccionAlObjetivo = (objetivo.position - transform.position).normalized;
        // Calculem l'angle entre la direcció del personatge i l'objectiu
        float angulo = Vector3.Angle(transform.forward, direccionAlObjetivo);

        // Si l'objectiu està dins de l'angle de visió...
        if (angulo < anguloVision / 2f)
        {
            // Calculem la distància fins a l'objectiu
            float distancia = Vector3.Distance(transform.position, objetivo.position);
            // Si està dins de la distància màxima...
            if (distancia < distanciaVision)
            {
                // Comprovem si hi ha obstacles entre el personatge i l'objectiu
                if (!Physics.Raycast(transform.position, direccionAlObjetivo, distancia, capaObstaculos))
                {
                    return true; // L'objectiu està dins del con de visió i sense obstacles
                }
            }
        }
        return false; // No està dins del con o hi ha obstacles
    }

    // Exemple d'ús: detectar si un objectiu està dins de la zona de visió
    void FixedUpdate()
    {
        Debug.Log(EstaEnZonaDeVision(player));
        if (!EstaEnZonaDeVision(player))
        {
            activeChase = false;
            canSeePlayer = false;
            return;
        }
        // Comprovem si cada objectiu està dins del con de visió
        if (EstaEnZonaDeVision(player))
        {
            canSeePlayer = true;
            //Debug.Log("Objectiu detectat: " + objetivo.name + "!");
        }
        else canSeePlayer = false;
        // Busquem tots els objectius dins de la distància màxima

    }

    // Visualització del con a l'escena
    void OnDrawGizmos()
    {
        if (!mostrarGizmos) return;

        Gizmos.color = Color.red;
        // Dibuixem els límits esquerre i dret del con
        Vector3 leftLimit = Quaternion.Euler(0, -anguloVision / 2, 0) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0, anguloVision / 2, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftLimit * distanciaVision);
        Gizmos.DrawRay(transform.position, rightLimit * distanciaVision);
        // Dibuixem una esfera per indicar la distància màxima
        Gizmos.DrawWireSphere(transform.position, distanciaVision);
    }

    /*** DEBUG
    private GUIStyle estilo;
    void OnGUI()
    {
        GUI.Label(new Rect(10, 40, 200, 30), "EstaEnZonaDeVision: " + EstaEnZonaDeVision(player).ToString(), estilo);
    }
    ***/
}
