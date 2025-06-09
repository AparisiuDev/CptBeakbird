using System.Collections;
using UnityEngine;

public class NighTime : MonoBehaviour
{
    public Light luzDireccional;
    public Color colorAlEntrar = Color.red;
    public float duracionTransicion = 1f;

    private Color colorOriginal;
    private Coroutine transicionActual;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (luzDireccional != null && luzDireccional.type == LightType.Directional)
        {
            colorOriginal = luzDireccional.color;
            IniciarTransicion(colorAlEntrar);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (luzDireccional != null && luzDireccional.type == LightType.Directional)
        {
            IniciarTransicion(colorOriginal);
        }
    }

    private void IniciarTransicion(Color destino)
    {
        if (transicionActual != null)
            StopCoroutine(transicionActual);

        transicionActual = StartCoroutine(TransicionarColor(destino));
    }

    private IEnumerator TransicionarColor(Color destino)
    {
        Color inicio = luzDireccional.color;
        float tiempo = 0f;

        while (tiempo < duracionTransicion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracionTransicion;
            luzDireccional.color = Color.Lerp(inicio, destino, t);
            yield return null;
        }

        luzDireccional.color = destino;
        transicionActual = null;
    }
}
