using UnityEngine;
using UnityEngine.UI;

public class UI_WorkerBuff : MonoBehaviour
{
    [Header("UI Images")]
    [SerializeField] private Toggle _toggleBoost;
    [SerializeField] private Image _boostIconOn, _boostIconOff;
    [SerializeField] private Image _boostTextOn, _boostTextOff;

    private TowerSpawner _towerSpawner;

    private void Start()
    {
        _boostIconOff.enabled = true;
        _boostIconOn.enabled = false;
        _boostTextOff.enabled = true;
        _boostTextOn.enabled = false;
        _towerSpawner = GetComponentInParent<TowerSpawner>();

        if (_towerSpawner == null)
        {
            //Debug.LogError("TowerSpawner component not found in parent.");
        }
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
        _towerSpawner.OnBuff();
    }

    private void ButtonUnclicked()
    {
        _towerSpawner.OnDebuff();
    }
}