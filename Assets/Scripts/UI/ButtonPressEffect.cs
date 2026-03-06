using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonPressEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float pressScale = 0.9f;
    public float pressDuration = 0.08f;
    public float releaseDuration = 0.1f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(originalScale * pressScale, pressDuration).SetEase(Ease.OutQuad).SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(originalScale, releaseDuration).SetEase(Ease.OutBack).SetUpdate(true);
    }
}
