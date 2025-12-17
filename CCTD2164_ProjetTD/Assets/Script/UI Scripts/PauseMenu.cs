using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("UI")]
    public RectTransform pauseMenuUI;
    public GameObject buttonsGroup;
    public GameObject gameHUDUI;

    [Header("Animation")]
    public float animationDuration = 1f;
    public float openScaleY = 1f;

    [Header("Effects")]
    public PauseBlurController blurController;

    private bool isAnimating = false;
    [SerializeField] private GameObject optionPanel;

    void Start()
    {
        ResumeInstant();
        optionPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isAnimating)
        {
            if (GameIsPaused)
                StartCoroutine(CloseMenu());
            else
                StartCoroutine(OpenMenu());
        }
    }

    // ----------- OPEN -----------

    IEnumerator OpenMenu()
    {
        isAnimating = true;
        GameIsPaused = true;

        pauseMenuUI.gameObject.SetActive(true);
        buttonsGroup.SetActive(false);

        if (gameHUDUI != null)
            gameHUDUI.SetActive(false);

        if (blurController != null)
            blurController.SetPauseBlur(true);

        Time.timeScale = 0f;

        pauseMenuUI.localScale = new Vector3(1f, 0f, 1f);

        float t = 0f;
        while (t < animationDuration)
        {
            t += Time.unscaledDeltaTime;
            float smoothT = Mathf.SmoothStep(0f, 1f, t / animationDuration);
            float y = Mathf.Lerp(0f, openScaleY, smoothT);
            pauseMenuUI.localScale = new Vector3(1f, y, 1f);
            yield return null;
        }

        pauseMenuUI.localScale = new Vector3(1f, openScaleY, 1f);
        buttonsGroup.SetActive(true);

        isAnimating = false;
    }

    // ----------- CLOSE -----------

    IEnumerator CloseMenu()
    {
        isAnimating = true;
        buttonsGroup.SetActive(false);

        float t = 0f;
        float startY = pauseMenuUI.localScale.y;

        while (t < animationDuration)
        {
            t += Time.unscaledDeltaTime;
            float smoothT = Mathf.SmoothStep(0f, 1f, t / animationDuration);
            float y = Mathf.Lerp(startY, 0f, smoothT);
            pauseMenuUI.localScale = new Vector3(1f, y, 1f);
            yield return null;
        }

        pauseMenuUI.localScale = new Vector3(1f, 0f, 1f);
        pauseMenuUI.gameObject.SetActive(false);

        if (blurController != null)
            blurController.SetPauseBlur(false);

        if (gameHUDUI != null)
            gameHUDUI.SetActive(true);

        Time.timeScale = 1f;
        GameIsPaused = false;

        isAnimating = false;
    }

    // ----------- RESET -----------

    public void ResumeInstant()
    {
        Time.timeScale = 1f;
        StartCoroutine(CloseMenu());
        GameIsPaused = false;

        //pauseMenuUI.localScale = new Vector3(1f, 0f, 1f);
        //pauseMenuUI.gameObject.SetActive(false);

        if (buttonsGroup != null)
            buttonsGroup.SetActive(false);

        if (gameHUDUI != null)
            gameHUDUI.SetActive(true);

        if (blurController != null)
            blurController.SetPauseBlur(false);
       
    }

    // ----------- BUTTONS -----------

    public void Options()
    {
        //Debug.Log("Options Menu Loaded");
        optionPanel.SetActive(true);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LA_Title_Menu");
    }

    public void Button()
    {
        StartCoroutine(OpenMenu());
    }
}

