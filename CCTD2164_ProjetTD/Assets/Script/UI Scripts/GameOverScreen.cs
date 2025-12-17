using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Fade Effects")]
    [Tooltip("Le Canvas Group de l'image de fond pour la DEFAITE.")]
    public CanvasGroup gameOverFadeGroup;

    [Tooltip("Le Canvas Group de l'image de fond pour la VICTOIRE.")]
    public CanvasGroup victoryFadeGroup;

    public float fadeDuration = 1.5f;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        // Initialisation des panneaux
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        // Initialisation des fondus (tout à zéro)
        InitFadeGroup(gameOverFadeGroup);
        InitFadeGroup(victoryFadeGroup);

        Health.OnNexusDied += HandleGameOver;
        WaveManager.OnGameVictory += HandleGameVictory;
    }

    private void InitFadeGroup(CanvasGroup group)
    {
        if (group != null)
        {
            group.alpha = 0f;
            group.blocksRaycasts = false;
            group.interactable = false;
        }
    }

    void OnDestroy()
    {
        Health.OnNexusDied -= HandleGameOver;
        WaveManager.OnGameVictory -= HandleGameVictory;
    }

    private void HandleGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        StartCoroutine(FadeAndShowUI(gameOverFadeGroup, gameOverPanel));
    }

    private void HandleGameVictory()
    {
        if (isGameOver) return;
        isGameOver = true;
        StartCoroutine(FadeAndShowUI(victoryFadeGroup, victoryPanel));
    }

    // Une seule coroutine générique pour gérer les deux cas
    private IEnumerator FadeAndShowUI(CanvasGroup targetFade, GameObject targetPanel)
    {
        if (targetFade != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.unscaledDeltaTime;
                targetFade.alpha = Mathf.Clamp01(timer / fadeDuration);
                yield return null;
            }
            targetFade.alpha = 1f;
        }

        // On fige le jeu APRES le fondu
        Time.timeScale = 0f;

        if (targetPanel != null)
        {
            targetPanel.SetActive(true);
        }

        // On permet de cliquer à travers le fondu pour atteindre les boutons du panel
        if (targetFade != null)
        {
            targetFade.blocksRaycasts = false;
            targetFade.interactable = false;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
