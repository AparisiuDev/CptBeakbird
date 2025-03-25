using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ConfiguracionCamaraOrtografica : MonoBehaviour
{
    [Header("Configuración Ortográfica")]
    public float tamanoOrtografico = 5f;
    public float distanciaClipCercana = 0.3f;
    public float distanciaClipLejana = 1000f;
    
    [Header("Configuración Visual")]
    public Color colorFondo = new Color(0.2f, 0.2f, 0.2f, 1f);
    
    // Referencia a la cámara
    private Camera camaraPrincipal;
    
    private void Awake()
    {
        // Obtener referencia a la cámara
        camaraPrincipal = GetComponent<Camera>();
        
        // Aplicar configuración
        AplicarConfiguracion();
    }
    
    private void AplicarConfiguracion()
    {
        if (camaraPrincipal != null)
        {
            // Asegurar que la cámara esté en modo ortográfico
            //camaraPrincipal.orthographic = false;
            
            // Aplicar configuración ortográfica
            camaraPrincipal.orthographicSize = tamanoOrtografico;
            camaraPrincipal.nearClipPlane = distanciaClipCercana;
            camaraPrincipal.farClipPlane = distanciaClipLejana;
            
            // Aplicar color de fondo
            camaraPrincipal.backgroundColor = colorFondo;
        }
    }
    
    // Este método permite cambiar el tamaño ortográfico en tiempo de ejecución
    public void CambiarTamanoOrtografico(float nuevoTamano)
    {
        tamanoOrtografico = nuevoTamano;
        if (camaraPrincipal != null)
        {
            camaraPrincipal.orthographicSize = tamanoOrtografico;
        }
    }
    
    // Para permitir actualizar la configuración desde el inspector durante el modo de edición
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (camaraPrincipal == null)
            camaraPrincipal = GetComponent<Camera>();
            
        AplicarConfiguracion();
    }
#endif
} 