using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private const bool Value = false;
    [SerializeField] private GameObject optionPanel;

    private void Start()
    {
        optionPanel.SetActive(false);
    }

    #region main menu panel
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OptionMenu()
    {
        optionPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    { 
        Application.Quit();
    }
    #endregion
}
