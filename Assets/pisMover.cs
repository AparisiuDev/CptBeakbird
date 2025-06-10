using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pisMover : MonoBehaviour
{
    [SerializeField] private float scrollSpeedY = 0.5f; // velocidad de desplazamiento vertical
    private Renderer rend;
    private Material mat;
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogWarning("No Renderer found on this GameObject.");
            enabled = false;
            return;
        }

        // Instancia el material para no afectar a otros objetos con el mismo material
        mat = rend.material;
        offset = mat.mainTextureOffset;
    }

    void Update()
    {
        offset.y += scrollSpeedY * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}
