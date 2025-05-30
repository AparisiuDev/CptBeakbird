using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class ShowSpeechBubble : MonoBehaviour
{
    public BubbleScaler bubbleScaler;
    public TextMeshProUGUI textoUI;
    public string textToShow;
    public bool showOnce;
    private bool alreadyShown;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !alreadyShown)
        {
            if (textoUI != null) textoUI.text = textToShow;
            bubbleScaler.Show();
            if (showOnce) alreadyShown = true;
        }
    }
}
