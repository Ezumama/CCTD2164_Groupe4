using UnityEngine;
using UnityEngine.UI;

public class UI_WorkerBuff : MonoBehaviour
{
    private Shooter _shooterScript;
    private Shooter_MultiTarget _shooterMultiScript;
    private Shooter_ThreeTargets _shooterThreeScript;
    [SerializeField] private Toggle _toggleBoost;

    private void Start()
    {
        // Finding which shooter script is attached
        _shooterScript = GetComponent<Shooter>();

        if (_shooterScript == null)
        {
            _shooterMultiScript = GetComponent<Shooter_MultiTarget>();
        }
        if (_shooterMultiScript == null)
        {
            _shooterThreeScript = GetComponent<Shooter_ThreeTargets>();
        }
    }

    // When toggle state changes
    public void ToggleClicked()
    {
        if (_toggleBoost.isOn)
        {
            ButtonClicked();
        }
        else
        {
            ButtonUnclicked();
        }
    }

    private void ButtonClicked()
    {
        if (_shooterScript != null)
        {
            _shooterScript.BuffDamage();
        }
        else if (_shooterMultiScript != null)
        {
            _shooterMultiScript.BuffDamage();
        }
        else if (_shooterThreeScript != null)
        {
            _shooterThreeScript.BuffDamage();
        }
    }

    private void ButtonUnclicked()
    {
        if (_shooterScript != null)
        {
            _shooterScript.StopBuff();
        }
        else if (_shooterMultiScript != null)
        {
            _shooterMultiScript.StopBuff();
        }
        else if (_shooterThreeScript != null)
        {
            _shooterThreeScript.StopBuff();
        }
    }
}