using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class ShowKey : MonoBehaviour
{
    public KeyScaler keyScaler;
    public TextMeshProUGUI textoUI;
    public string textToShow;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (textoUI != null) textoUI.text = textToShow;
            keyScaler.Show();
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        keyScaler.Hide();
    }


}
