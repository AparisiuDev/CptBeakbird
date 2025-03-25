using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraSeguidora : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;

    [Header("Configuración de Seguimiento")]
    public float suavizadoMovimiento = 0.125f;
    public Vector3 offsetPosicion = new Vector3(0, 10, -10);
    
    [Header("Configuración de Rotación")]
    public float velocidadRotacion = 45f;
    public float suavizadoRotacion = 0.1f;

    [Header("Camara Ortografica si o no")]
    public bool ortoCheck = false;

    // Variables privadas
    private float anguloRotacionActual = 45f; // Ángulo inicial (45 grados)
    private Quaternion rotacionObjetivo;
    private Vector3 posicionObjetivo;
    private Camera camaraPrincipal;
    
    private void Start()
    {
        // Verificar si no se asignó el jugador
        if (jugador == null)
        {
            // Intentar encontrar al jugador por tag
            GameObject objetoJugador = GameObject.FindGameObjectWithTag("Player");
            if (objetoJugador != null)
            {
                jugador = objetoJugador.transform;
            }
            else
            {
                Debug.LogError("No se encontró el jugador. Asigna el transform del jugador en el inspector.");
            }
        }
        
        // Obtener referencia a la cámara
        camaraPrincipal = GetComponent<Camera>();
        if (camaraPrincipal != null)
        {
            // Asegurar que la cámara esté en modo ortográfico o no
            camaraPrincipal.orthographic = ortoCheck;
        }
        
        // Configurar la rotación inicial
        transform.rotation = Quaternion.Euler(30, anguloRotacionActual, 0);
    }
    
    private void LateUpdate()
    {
        if (jugador == null)
            return;
            
        // Detectar entrada para rotación
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anguloRotacionActual += velocidadRotacion;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            anguloRotacionActual -= velocidadRotacion;
        }
        
        // Calcular la rotación objetivo
        rotacionObjetivo = Quaternion.Euler(30, anguloRotacionActual, 0);
        
        // Suavizar la rotación actual
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, suavizadoRotacion);
        
        // Calcular el offset rotado basado en el ángulo actual
        Vector3 offsetRotado = Quaternion.Euler(0, anguloRotacionActual, 0) * offsetPosicion;
        
        // Calcular la posición objetivo
        posicionObjetivo = jugador.position + offsetRotado;
        
        // Suavizar el movimiento de la cámara
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, suavizadoMovimiento);
    }
} 