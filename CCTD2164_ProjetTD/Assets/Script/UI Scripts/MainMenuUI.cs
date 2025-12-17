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
    //[SerializeField] private GameObject CreditsBG;
    [SerializeField] private Image _fuckingcredits;

    private void Start()
    {
        optionPanel.SetActive(false);
        //CreditsBG.SetActive(false);
        _fuckingcredits.enabled = false;
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
        //CreditsBG.SetActive(true);
        _fuckingcredits.enabled = false;
    }

    public void QuitGame()
    { 
        Application.Quit();
    }
    #endregion
}
