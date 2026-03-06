using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIIngame : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public Button restartButton;
    public Button pauseButton;
    public GameObject hudContainer;

    private void Start()
    {
        if (levelText != null && GameManager.Instance != null)
            levelText.text = GameManager.Instance.GetLevelName();

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestart);

        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPause);
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        bool show = GameManager.Instance.State == GameManager.GameState.Playing;
        if (hudContainer != null && hudContainer.activeSelf != show)
            hudContainer.SetActive(show);
    }

    private void OnRestart()
    {
        if (GameManager.Instance != null && GameManager.Instance.State == GameManager.GameState.Playing)
            GameManager.Instance.Replay();
    }

    private void OnPause()
    {
        if (GameManager.Instance != null && GameManager.Instance.State == GameManager.GameState.Playing)
            GameManager.Instance.Pause();
    }
}
