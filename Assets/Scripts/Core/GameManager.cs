using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, Won, Lost, Paused }

    public static GameManager Instance { get; private set; }
    public static int CurrentLevelIndex;

    public GameState State { get; private set; }

    [Header("UI Prefabs")]
    public GameObject popupWinPrefab;
    public GameObject popupLosePrefab;
    public GameObject popupPausePrefab;

    [Header("Levels")]
    public LevelCollection levelCollection;

    [Header("References")]
    public LevelLoader levelLoader;

    [Header("Lose Settings")]
    public float losePopupDelay = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (levelLoader != null && levelCollection != null && levelCollection.Count > 0)
        {
            var entry = levelCollection.GetLevel(CurrentLevelIndex);
            if (entry != null)
                levelLoader.levelJson = entry.levelJson;
        }
    }

    private void Start()
    {
        State = GameState.Playing;
        InputGate.locked = false;
    }

    public void Win()
    {
        if (State != GameState.Playing) return;
        State = GameState.Won;

        if (popupWinPrefab != null)
            Instantiate(popupWinPrefab);
    }

    public void Lose()
    {
        if (State != GameState.Playing) return;
        State = GameState.Lost;
        StartCoroutine(ShowLoseDelayed());
    }

    private IEnumerator ShowLoseDelayed()
    {
        yield return new WaitForSeconds(losePopupDelay);

        if (popupLosePrefab != null)
            Instantiate(popupLosePrefab);
    }

    public void Pause()
    {
        if (State != GameState.Playing) return;
        State = GameState.Paused;
        Time.timeScale = 0f;

        if (popupPausePrefab != null)
            Instantiate(popupPausePrefab);
    }

    public void Resume()
    {
        if (State != GameState.Paused) return;
        Time.timeScale = 1f;
        State = GameState.Playing;
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        InputGate.locked = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        if (levelCollection != null && CurrentLevelIndex < levelCollection.Count - 1)
            CurrentLevelIndex++;

        Time.timeScale = 1f;
        InputGate.locked = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        CurrentLevelIndex = 0;
        Time.timeScale = 1f;
        InputGate.locked = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public string GetLevelName()
    {
        if (levelCollection != null && levelCollection.Count > 0)
        {
            var entry = levelCollection.GetLevel(CurrentLevelIndex);
            if (entry != null && !string.IsNullOrEmpty(entry.name))
                return entry.name;
        }
        return "Level " + (CurrentLevelIndex + 1);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
