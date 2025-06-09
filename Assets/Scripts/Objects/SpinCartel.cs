using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinCartel : MonoBehaviour
{
    public float rotationDuration = 3f; // Tiempo total de rotación hasta frenar
    public float initialSpeed = 720f;   // Velocidad inicial de rotación en grados/segundo

    private Quaternion originalRotation;
    private bool isRotating = false;

    public GameObject cartel1;
    public GameObject cartel2;

    public void StartRotationSequence()
    {
        // Intercambiar materiales entre los carteles
        SwapMaterials(cartel1, cartel2);
        if (!isRotating)
            StartCoroutine(RotateAndReturn());
    }

    private System.Collections.IEnumerator RotateAndReturn()
    {
        isRotating = true;
        originalRotation = transform.rotation;
        originalRotation.y += 180f; // Ajustar la rotación original para que sea 180 grados más
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            float speed = Mathf.Lerp(initialSpeed, 0f, t);
            transform.Rotate(Vector3.up, speed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Suavemente volver a la rotación original
        float returnDuration = 1f;
        Quaternion currentRotation = transform.rotation;
        elapsed = 0f;

        while (elapsed < returnDuration)
        {
            float t = elapsed / returnDuration;
            transform.rotation = Quaternion.Slerp(currentRotation, originalRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = originalRotation;
        isRotating = false;
    }

    public void SwapMaterials(GameObject a, GameObject b)
    {
        Renderer rendererA = a.GetComponent<Renderer>();
        Renderer rendererB = b.GetComponent<Renderer>();

        if (rendererA == null || rendererB == null)
        {
            Debug.LogWarning("Uno de los objetos no tiene Renderer.");
            return;
        }

        Material temp = rendererA.material;
        rendererA.material = rendererB.material;
        rendererB.material = temp;
    }
}
