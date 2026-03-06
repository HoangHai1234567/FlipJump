using UnityEngine;
using DG.Tweening;

public abstract class PopupBase : MonoBehaviour
{
    [Header("Popup Animation")]
    public CanvasGroup backgroundOverlay;
    public RectTransform panel;
    public RectTransform buttonsContainer;

    [Header("Timing")]
    public float panelScaleDuration = 0.4f;
    public float fadeDuration = 0.2f;
    public float buttonsDelay = 0.3f;

    protected virtual void OnEnable()
    {
        PlayEntranceAnimation();
    }

    private void PlayEntranceAnimation()
    {
        if (backgroundOverlay != null)
        {
            backgroundOverlay.alpha = 0f;
            backgroundOverlay.DOFade(1f, fadeDuration).SetUpdate(true);
        }

        if (panel != null)
        {
            panel.localScale = Vector3.zero;
            panel.DOScale(Vector3.one, panelScaleDuration).SetEase(Ease.OutBack).SetUpdate(true);
        }

        if (buttonsContainer != null)
        {
            buttonsContainer.localScale = Vector3.zero;
            buttonsContainer.DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack)
                .SetDelay(panelScaleDuration + buttonsDelay)
                .SetUpdate(true);
        }
    }

    protected void CloseAndAction(System.Action action)
    {
        if (panel != null)
        {
            panel.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
            {
                action?.Invoke();
                Destroy(gameObject);
            });
        }
        else
        {
            action?.Invoke();
            Destroy(gameObject);
        }
    }
}
