using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ControlZoomCamara : MonoBehaviour
{
    [Header("Configuración de Zoom")]
    public float velocidadZoom = 1f;
    public float tamanoMinimo = 3f;
    public float tamanoMaximo = 10f;
    public bool invertirZoom = false;
    
    // Referencia a la cámara
    private Camera camaraPrincipal;
    // Referencia al script de configuración (opcional)
    private ConfiguracionCamaraOrtografica configuracionCamara;
    
    private void Start()
    {
        // Obtener referencia a la cámara
        camaraPrincipal = GetComponent<Camera>();
        
        // Intentar obtener el script de configuración
        configuracionCamara = GetComponent<ConfiguracionCamaraOrtografica>();
    }
    
    private void Update()
    {
        // Detectar entrada de la rueda del ratón
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        // Si no hay entrada, salir
        if (scrollInput == 0)
            return;
            
        // Invertir si es necesario
        if (invertirZoom)
            scrollInput = -scrollInput;
            
        // Calcular el nuevo tamaño ortográfico
        float nuevoTamano = camaraPrincipal.orthographicSize - (scrollInput * velocidadZoom);
        
        // Limitar el tamaño dentro del rango permitido
        nuevoTamano = Mathf.Clamp(nuevoTamano, tamanoMinimo, tamanoMaximo);
        
        // Aplicar el nuevo tamaño
        if (configuracionCamara != null)
        {
            // Si existe el script de configuración, usarlo
            configuracionCamara.CambiarTamanoOrtografico(nuevoTamano);
        }
        else
        {
            // Si no, aplicar directamente a la cámara
            camaraPrincipal.orthographicSize = nuevoTamano;
        }
    }
} 