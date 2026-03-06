using UnityEngine;
using UnityEngine.UI;

public class PopupLose : PopupBase
{
    [Header("Buttons")]
    public Button buttonReplay;
    public Button buttonHome;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (buttonReplay != null)
            buttonReplay.onClick.AddListener(() => CloseAndAction(() => GameManager.Instance.Replay()));

        if (buttonHome != null)
            buttonHome.onClick.AddListener(() => CloseAndAction(() => GameManager.Instance.Home()));
    }
}
