using UnityEngine;
using UnityEngine.UI;

public class PopupWin : PopupBase
{
    [Header("Buttons")]
    public Button buttonNext;
    public Button buttonReplay;
    public Button buttonHome;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (buttonNext != null)
            buttonNext.onClick.AddListener(() => CloseAndAction(() => GameManager.Instance.NextLevel()));

        if (buttonReplay != null)
            buttonReplay.onClick.AddListener(() => CloseAndAction(() => GameManager.Instance.Replay()));

        if (buttonHome != null)
            buttonHome.onClick.AddListener(() => CloseAndAction(() => GameManager.Instance.Home()));
    }
}
