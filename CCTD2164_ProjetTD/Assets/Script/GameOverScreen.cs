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

    [Header("Fade Effect")] 
    [Tooltip("Le Canvas Group sur le panneau noir pour le fondu.")]
    public CanvasGroup fadeGroup;
    public float fadeDuration = 1.5f; // Durée du fondu en secondes

    private bool isGameOver = false;

    void Awake()
    {
        // Singleton pattern
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
        // Masquer le panneau de Game Over au démarrage
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Initialiser l'effet de fondu
        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f; // Complètement transparent au début
            // S'assurer qu'il ne bloque pas les clics pendant le jeu
            fadeGroup.blocksRaycasts = false;
            fadeGroup.interactable = false;
        }

        // S'abonner à l'événement de mort du Nexus
        Health.OnNexusDied += HandleGameOver;
    }

    void OnDestroy()
    {
        Health.OnNexusDied -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("GAME OVER ! Le Nexus a été détruit.");

        // 🔥 Démarrer la coroutine de fondu au lieu d'arrêter le temps immédiatement
        StartCoroutine(FadeToBlackAndShowGameOver());
    }

    private IEnumerator FadeToBlackAndShowGameOver()
    {
        if (fadeGroup != null)
        {
            float timer = 0f;

            // Boucle de fondu : de 0 (transparent) à 1 (noir)
            while (timer < fadeDuration)
            {
                timer += Time.unscaledDeltaTime;
                float alpha = Mathf.Clamp01(timer / fadeDuration);

                fadeGroup.alpha = alpha;

                yield return null;
            }
            fadeGroup.alpha = 1f; // Assure que le fond est complètement noir
        }

        // --- Une fois le fondu terminé (l'écran est noir) ---

        // 1. Arrêter le temps
        Time.timeScale = 0f;

        // 2. Afficher l'écran de Game Over (boutons, titres, etc.)
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 3. 🔥 CORRECTION : Empêcher le panneau de fondu de bloquer les clics.
        if (fadeGroup != null)
        {
            fadeGroup.blocksRaycasts = false;
            fadeGroup.interactable = false;
        }

        // Rendre le panneau de Game Over cliquable si vous utilisez un Canvas Group dessus.
        // Si votre GameOverPanel contient un CanvasGroup, assurez-vous qu'il est cliquable (interactable = true).
    }


    // --- Fonctions appelées par les Boutons UI (inchangées) ---

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
