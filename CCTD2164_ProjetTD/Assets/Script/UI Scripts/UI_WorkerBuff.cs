using UnityEngine;
using UnityEngine.UI;

public class UI_WorkerBuff : MonoBehaviour
{
    private Shooter _shooterScript;
    private Shooter_MultiTarget _shooterMultiScript;
    private Shooter_ThreeTargets _shooterThreeScript;

    [Header("UI Images")]
    [SerializeField] private Toggle _toggleBoost;
    [SerializeField] private Image _boostIconOn, _boostIconOff;
    [SerializeField] private Image _boostTextOn, _boostTextOff;

    private void Start()
    {
        _boostIconOff.enabled = true;
        _boostIconOn.enabled = false;
        _boostTextOff.enabled = true;
        _boostTextOn.enabled = false;

        #region setting shooter script
        // Finding which shooter script is attached
        _shooterScript = GetComponent<Shooter>();

        if (_shooterScript == null)
        {
            Debug.Log("Shooter script not found, checking for Shooter_MultiTarget script.");
            _shooterMultiScript = GetComponent<Shooter_MultiTarget>();
        }
        if (_shooterMultiScript == null)
        {
            Debug.Log("Shooter_MultiTarget script not found, checking for Shooter_ThreeTargets script.");
            _shooterThreeScript = GetComponent<Shooter_ThreeTargets>();
        }
        if (_shooterThreeScript == null)
        {
            Debug.Log("ShooterThree script not found.");
        }
        #endregion
    }

    // When toggle state changes
    public void ToggleClicked()
    {
        if (_toggleBoost.isOn)
        {
            ButtonClicked();
            _boostIconOff.enabled = false;
            _boostIconOn.enabled = true;
            _boostTextOff.enabled = false;
            _boostTextOn.enabled = true;
        }
        else
        {
            ButtonUnclicked();
            _boostIconOff.enabled = true;
            _boostIconOn.enabled = false;
            _boostTextOff.enabled = true;
            _boostTextOn.enabled = false;
        }
    }

    private void ButtonClicked()
    {
        if (_shooterScript != null)
        {
            _shooterScript.BuffDamage();
            GameManager.Instance.FireWorker(1);
        }
        else if (_shooterMultiScript != null)
        {
            _shooterMultiScript.BuffDamage();
            GameManager.Instance.FireWorker(1);
        }
        else if (_shooterThreeScript != null)
        {
            _shooterThreeScript.BuffDamage();
            GameManager.Instance.FireWorker(1);
        }
    }

    private void ButtonUnclicked()
    {
        if (_shooterScript != null)
        {
            _shooterScript.StopBuff();
            GameManager.Instance.HireWorker(1);
        }
        else if (_shooterMultiScript != null)
        {
            _shooterMultiScript.StopBuff();
            GameManager.Instance.HireWorker(1);
        }
        else if (_shooterThreeScript != null)
        {
            _shooterThreeScript.StopBuff();
            GameManager.Instance.HireWorker(1);
        }
    }
}