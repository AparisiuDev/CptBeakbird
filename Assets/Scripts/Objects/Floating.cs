using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float amplitude = 0.5f;   // Qué tan alto sube y baja
    public float frequency = 1f;     // Qué tan rápido lo hace

    private Vector3 startPos;
    private float randomOffset;

    void Start()
    {
        startPos = transform.position;
        randomOffset = Random.Range(0f, 2 * Mathf.PI);  // Offset aleatorio de fase
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency + randomOffset) * amplitude;
        transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
    }
}
