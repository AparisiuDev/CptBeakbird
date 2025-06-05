using UnityEngine;

public class ShipFloatingAway : MonoBehaviour
{
    public GameObject targetObject;
    public float moveSpeed = 2f;
    public float moveDuration = 15f;

    [Header("Rotation Oscillation Settings")]
    public float rotationMinX = -5f;      // Ángulo mínimo en grados (inclinación hacia atrás)
    public float rotationMaxX = 5f;       // Ángulo máximo en grados (inclinación hacia adelante)
    public float rotationSpeed = 0.5f;    // Velocidad de la oscilación (baja = movimiento más lento)

    private float moveTimer = 0f;
    private bool isMoving = false;
    private bool alreadyTriggered = false;

    void Update()
    {
        if (targetObject != null && !targetObject.activeInHierarchy && !alreadyTriggered)
        {
            isMoving = true;
            alreadyTriggered = true;
            moveTimer = 0f;
        }

        if (isMoving)
        {
            if (moveTimer < moveDuration)
            {
                moveTimer += Time.deltaTime;

                // Movimiento en X local
                transform.position += transform.forward * moveSpeed * Time.deltaTime;

                // Oscilación suave entre los dos ángulos
                float t = (Mathf.Sin(Time.time * rotationSpeed * 2f * Mathf.PI) + 1f) / 2f; // Normaliza entre 0 y 1
                float angleX = Mathf.Lerp(rotationMinX, rotationMaxX, t);
                Vector3 currentEuler = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(angleX, currentEuler.y, currentEuler.z);
            }
            else
            {
                isMoving = false;
            }
        }
    }
}
