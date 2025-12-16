using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance { get; private set; }

    [Header("UI")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Fade Effect")] 
    [Tooltip("Le Canvas Group sur le panneau noir pour le fondu.")]
    public CanvasGroup fadeGroup;
    public float fadeDuration = 1.5f;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;
            fadeGroup.blocksRaycasts = false;
            fadeGroup.interactable = false;
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        Health.OnNexusDied += HandleGameOver;
        WaveManager.OnGameVictory += HandleGameVictory;
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

        //Debug.Log("GAME OVER !");

        StartCoroutine(FadeToBlackAndShowGameOver());
    }

    private void HandleGameVictory()
    {
        if (isGameOver) return; 

        isGameOver = true;

        //Debug.Log("Le joueur a gagné !");

        StartCoroutine(FadeToBlackAndShowVictoryScreen());
    }

    private IEnumerator FadeToBlackAndShowVictoryScreen()
    {
        yield return StartCoroutine(FadeToBlack());


        Time.timeScale = 0f;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        if (fadeGroup != null)
        {
            fadeGroup.blocksRaycasts = false;
            fadeGroup.interactable = false;
        }
    }

    private IEnumerator FadeToBlack()
    {
        if (fadeGroup != null)
        {
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.unscaledDeltaTime;
                float alpha = Mathf.Clamp01(timer / fadeDuration);

                fadeGroup.alpha = alpha;

                yield return null;
            }
            fadeGroup.alpha = 1f;
        }
    }

    private IEnumerator FadeToBlackAndShowGameOver()
    {
        yield return StartCoroutine(FadeToBlack());

        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (fadeGroup != null)
        {
            fadeGroup.blocksRaycasts = false;
            fadeGroup.interactable = false;
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
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
