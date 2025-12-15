using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public PauseBlurController blurController;
    public GameObject pauseMenuUI;


    void Start()
    {
        Resume();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        if (blurController != null)
        {
            blurController.SetPauseBlur(false);
        }
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        if (blurController != null)
        {
            blurController.SetPauseBlur(true);
        }
    }

    public void options()
    {
        Time.timeScale = 1f;
        Debug.Log("Options Menu Loaded");

        // Mettre le nom de la scène des options ici
        //SceneManager.LoadScene("");
    }

    public void Quit()

    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
