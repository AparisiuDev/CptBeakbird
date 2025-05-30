using UnityEngine;
using DG.Tweening;
using System.Collections;

public class BubbleScaler : MonoBehaviour
{
    public float duration = 0.3f;
    public float visibleTime = 5f;
    public float bigScale;
    //public bool showOnlyOnce;
    private bool alreadyShown = false;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        bigScale = transform.localScale.x;
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup no encontrado. Añadiéndolo automáticamente.");
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Opcional: empezar invisible
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
    }

    public void Show()
    {
        if (alreadyShown /**&& showOnlyOnce**/) return;

        transform.DOScale(bigScale, duration).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1f, duration);

        StartCoroutine(AutoHide());

        alreadyShown = true;
    }

    public void Hide()
    {
        transform.DOScale(0f, duration).SetEase(Ease.InBack);
        canvasGroup.DOFade(0f, duration);
    }

    private IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(visibleTime);
        Hide();
    }
}
