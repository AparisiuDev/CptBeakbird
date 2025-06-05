using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMissingObj : MonoBehaviour
{
    public GameObject inactiveObject;      // Objeto que debe volverse inactivo
    public Animator animator;              // Animator del objeto
    public string boolToActivate = "hasStolen"; // Bool del Animator a activar
    public Transform player;               // Referencia al jugador
    public float rotationSpeed = 2f;       // Velocidad de rotación suave

    [SerializeField] private bool shouldLookAtPlayer = false; // Bool privado para controlar si mira

    private bool actionDone = false;       // Controla que solo se haga una vez
    private bool isRotating = false;       // Controla si está rotando
    private Quaternion targetRotation;

    void Update()
    {
        // Ejecutar solo una vez cuando el objeto se vuelve inactivo
        if (!inactiveObject.activeInHierarchy && !actionDone)
        {
            animator.SetBool(boolToActivate, true);

            if (shouldLookAtPlayer)
            {
                StartSmoothLookAtPlayer();
            }

            actionDone = true;
        }

        if (isRotating)
        {
            SmoothRotateToTarget();
        }
    }

    void StartSmoothLookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // Evitar inclinación vertical
        targetRotation = Quaternion.LookRotation(direction);
        isRotating = true;
    }

    void SmoothRotateToTarget()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Cuando casi ha terminado de rotar, detener
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }

    // Llamado externamente para activar el bool
    public void SetShouldLookAtPlayer(bool value)
    {
        shouldLookAtPlayer = value;
    }
}
