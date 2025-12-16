using UnityEngine;
using UnityEngine.UI;

public class UI_ActivateDeactivate : MonoBehaviour
{
    [Header("UI Images")]
    [SerializeField] private Image _energyIconOn, _energyIconOff;
    [SerializeField] private Toggle _toggleActivation;

    private TowerSpawner _activationScript;

    private void Start()
    {
        _energyIconOn.enabled = true;
        _energyIconOff.enabled = false;
    }

    public void ToggleClicked()
    {
        if (_toggleActivation.isOn)
        {
            _activationScript = GetComponentInParent<TowerSpawner>();
            ButtonClicked(); 
        }
        else
        {
            ButtonUnclicked();
        }
    }

    private void ButtonClicked()
    {
        //if (_activationScript == null)
        //{
        //    Debug.LogError("TowerActivateDeactivate script not found on the GameObject.");
        //}

        _activationScript.DeactivateTower();
        _energyIconOn.enabled = false;
        _energyIconOff.enabled = true;
    }

    private void ButtonUnclicked()
    {
        _activationScript.ActivateTower();
        _energyIconOn.enabled = true;
        _energyIconOff.enabled = false;
    }
}
