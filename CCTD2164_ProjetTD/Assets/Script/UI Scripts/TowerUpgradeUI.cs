using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TowerUpgradeUI : MonoBehaviour
{
    private TowerSpawner _towerUpgradeScript;
    [SerializeField] private Button _downgradeButton;
    [SerializeField] private Button _upgradeButton;

    public void Update()
    {
        if (_towerUpgradeScript._canAfford == false)
        {
            TooPoor();
        }

        else
        {
            CanAfford();
        }
    }

    public void SetUpgrade(TowerSpawner _upgradeScript)
    {
        //Debug.Log("SetUpgrade CALLED on: " + gameObject.name);
        _towerUpgradeScript = _upgradeScript;

        InitializeUI();
    }
    private void InitializeUI()
    {
        if (_towerUpgradeScript._levelUpgrade == 1)
        { 
            _downgradeButton.interactable = false; 
        }

        else
        {
            _downgradeButton.interactable = false;
        }

        if (_towerUpgradeScript._levelUpgrade == 3)
        {
            _upgradeButton.interactable = false;
        }
        else
        {
            _upgradeButton.interactable = true;
        }
    }

    public void ReInitialize()
    {
        if (_towerUpgradeScript != null)
        {
            InitializeUI();
        }
    }

    public void UpgradeLvl2Clicked()
    {
        _towerUpgradeScript.UpgradeTowerLevel2();
        gameObject.SetActive(false);
    }

    public void UpgradeLvl3Clicked()
    {
        _towerUpgradeScript.UpgradeTowerLevel3();
        gameObject.SetActive(false);
    }

    public void TooPoor()
    {
        //Debug.LogError("Not enough money to upgrade!");
        _upgradeButton.enabled = false;
    }

    public void CanAfford()
    {
        //Debug.LogError("C'est good patate");
        _upgradeButton.enabled = true;
    }


    //// Downgrade level 3 to level 2
    //public void DowngradeFromLvl3()
    //{
    //    _towerUpgradeScript.DowngradeToLevel2();
    //}

    //// Downgrade level 2 to level 1
    //public void DowngradeFromLvl2()
    //{
    //    _towerUpgradeScript.DowngradeToLevel1();
    //}

}
