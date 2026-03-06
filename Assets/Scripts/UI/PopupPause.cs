using UnityEngine;
using UnityEngine.UI;

public class PopupPause : PopupBase
{
    [Header("Buttons")]
    public Button buttonResume;
    public Button buttonHome;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (buttonResume != null)
            buttonResume.onClick.AddListener(() => CloseAndAction(() => GameManager.Instance.Resume()));

        if (buttonHome != null)
            buttonHome.onClick.AddListener(() => CloseAndAction(() => GameManager.Instance.Home()));
    }
}
